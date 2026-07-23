using System.Runtime.InteropServices;

namespace reShutCLI.Services;

/// <summary>
/// Registers recurring power actions with the Windows Task Scheduler through its
/// COM API (taskschd.dll, "Schedule.Service"), rather than by shelling out to
/// schtasks.exe and parsing its output.
///
/// Bound late through <c>dynamic</c>: the Task Scheduler object graph is deep
/// (<c>task.Triggers.Create(...).StartBoundary = ...</c>), and declaring the dozen
/// COM interfaces involved would dwarf the logic. The same late-bound approach the
/// installer's ShortcutService already uses for WScript.Shell.
/// </summary>
internal static class TaskSchedulerService
{
    /// <summary>
    /// Tasks live in their own folder so they can be listed and removed as a set.
    /// The previous schtasks call wrote randomly named tasks straight into the root
    /// folder, where they accumulated with no way to find them again.
    /// </summary>
    public const string TaskFolder = @"\reShut CLI";

    private const int TASK_TRIGGER_DAILY = 2;
    private const int TASK_TRIGGER_WEEKLY = 3;
    private const int TASK_ACTION_EXEC = 0;
    private const int TASK_CREATE_OR_UPDATE = 6;
    private const int TASK_LOGON_INTERACTIVE_TOKEN = 3;

    /// <summary>Run with the user's normal token - no elevation is needed for a shutdown.</summary>
    private const int TASK_RUNLEVEL_LUA = 0;

    public enum Recurrence
    {
        Daily,
        Weekly,
    }

    /// <summary>
    /// Registers a recurring task that runs reShut CLI with the given switch.
    /// Returns the created task's name.
    /// </summary>
    /// <param name="argument">The command-line switch to invoke, e.g. "/s" or "/r".</param>
    public static string Register(Recurrence recurrence, DateTime firstRun, string argument, string description)
    {
        // Point the task at reShut CLI itself rather than shutdown.exe, so a scheduled
        // run goes through the same PowerManager path as an interactive one.
        var executable = Environment.ProcessPath
                         ?? throw new InvalidOperationException("Could not determine the reShut CLI executable path.");

        object? service = null;
        try
        {
            var serviceType = Type.GetTypeFromProgID("Schedule.Service")
                              ?? throw new InvalidOperationException("The Windows Task Scheduler service is unavailable.");
            service = Activator.CreateInstance(serviceType)
                      ?? throw new InvalidOperationException("Could not create the Task Scheduler COM object.");

            dynamic scheduler = service;
            scheduler.Connect();

            dynamic folder = GetOrCreateFolder(scheduler);
            dynamic definition = scheduler.NewTask(0);

            definition.RegistrationInfo.Description = description;
            definition.RegistrationInfo.Author = "reShut CLI";
            definition.Principal.LogonType = TASK_LOGON_INTERACTIVE_TOKEN;
            definition.Principal.RunLevel = TASK_RUNLEVEL_LUA;

            // Without this a task whose start time passed while the machine was off is
            // simply skipped instead of running at the next opportunity.
            definition.Settings.StartWhenAvailable = true;

            dynamic trigger = definition.Triggers.Create(
                recurrence == Recurrence.Daily ? TASK_TRIGGER_DAILY : TASK_TRIGGER_WEEKLY);

            // The Task Scheduler expects ISO 8601 local time; anything else is rejected
            // outright with a bare E_INVALIDARG.
            trigger.StartBoundary = firstRun.ToString("yyyy-MM-ddTHH:mm:ss");

            if (recurrence == Recurrence.Daily)
            {
                trigger.DaysInterval = 1;
            }
            else
            {
                trigger.WeeksInterval = 1;
                // DaysOfWeek is a bitmask with Sunday as bit 0.
                trigger.DaysOfWeek = (short)(1 << (int)firstRun.DayOfWeek);
            }

            dynamic action = definition.Actions.Create(TASK_ACTION_EXEC);
            action.Path = executable;
            action.Arguments = argument;
            action.WorkingDirectory = Path.GetDirectoryName(executable);

            var taskName = $"reShutCLI_{recurrence}_{firstRun:HHmm}_{Guid.NewGuid():N}"[..40];

            folder.RegisterTaskDefinition(taskName, definition, TASK_CREATE_OR_UPDATE,
                null, null, TASK_LOGON_INTERACTIVE_TOKEN, null);

            return taskName;
        }
        finally
        {
            if (service is not null) Marshal.ReleaseComObject(service);
        }
    }

    private static dynamic GetOrCreateFolder(dynamic scheduler)
    {
        try
        {
            return scheduler.GetFolder(TaskFolder);
        }
        // GetFolder throws rather than returning null when the folder is absent, and the
        // late-binding layer translates the HRESULT before it reaches us: 0x80070002
        // arrives as FileNotFoundException, not COMException. Catching only the latter
        // silently skipped folder creation entirely.
        catch (Exception ex) when (ex is COMException or IOException)
        {
            dynamic root = scheduler.GetFolder(@"\");
            return root.CreateFolder(TaskFolder, null);
        }
    }
}
