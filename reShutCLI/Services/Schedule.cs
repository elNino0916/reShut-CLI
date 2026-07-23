using System.Diagnostics;
using System.Text.RegularExpressions;
using reShutCLI.Helpers;

namespace reShutCLI.Services;

internal static partial class Schedule
{
    [GeneratedRegex(@"in\s+(\d+)\s*(hour|hours|minute|minutes)", RegexOptions.IgnoreCase)]
    private static partial Regex RelativeTimeRegex();

    public static bool Plan()
    {
        while (true)
        {
            var action = PromptAction();
            if (action is null) return false;

            var targetTime = PromptTargetTime();

            switch (ConfirmAndExecute(action.Value, targetTime))
            {
                case ConfirmResult.Done:
                    return true;
                case ConfirmResult.Cancelled:
                    return false;
                case ConfirmResult.Reenter:
                    continue;
            }
        }
    }

    private enum ConfirmResult
    {
        Done,
        Cancelled,
        Reenter,
    }

    private enum PowerAction
    {
        Shutdown,
        Reboot,
    }

    /// <summary>Returns the selected action, or null when the user cancels/goes back.</summary>
    private static PowerAction? PromptAction()
    {
        while (true)
        {
            Console.Clear();
            UIDraw.TextColor = Variables.MenuColor;
            UIDraw.DrawMenu(null,
                [
                    Localization.Get("Schedule_PromptAction"),
                    Localization.Get("Schedule_SelectOption"),
                    "1) " + Localization.Get("Schedule_ShutdownOption"),
                    "2) " + Localization.Get("Schedule_RebootOption"),
                ],
                [
                    "0) " + Localization.Get("Schedule_Cancel"),
                    "9) " + Localization.Get("Back"),
                ]);

            switch (Console.ReadKey().KeyChar)
            {
                case '0':
                    // Abort a pending scheduled shutdown.
                    PowerManager.AbortPendingShutdown();
                    Console.Clear();
                    UIDraw.TextColor = ConsoleColor.Green;
                    UIDraw.DrawBoxedMessage(Localization.Get("Schedule_ActionCancelled"));
                    Thread.Sleep(500);
                    return null;
                case '1':
                    return PowerAction.Shutdown;
                case '2':
                    return PowerAction.Reboot;
                case '9':
                    Console.Clear();
                    return null;
                default:
                    UIDraw.TextColor = ConsoleColor.Red;
                    UIDraw.DrawBoxedMessage(Localization.Get("ErrorOccurred"));
                    UIDraw.TextColor = ConsoleColor.White;
                    break;
            }
        }
    }

    private static DateTime PromptTargetTime()
    {
        while (true)
        {
            Console.Clear();
            UIDraw.TextColor = Variables.MenuColor;

            var use24HoursFormat = DateTime.Now.ToString("tt").Length == 0;
            UIDraw.DrawBoxedMessage($" {Localization.Get("Schedule_CurrentTime")} {Time.GetTime(use24HoursFormat)} ");
            UIDraw.DrawBoxedMessage("Enter time (minutes, 'in 2 hours', or 'yyyy-MM-dd HH:mm')");
            UIDraw.TextColor = ConsoleColor.White;
            UIDraw.Draw(Localization.Get("Schedule_InputPrompt") + " ");

            var inputStr = Console.ReadLine() ?? "";

            if (int.TryParse(inputStr, out var minutes))
                return DateTime.Now.AddMinutes(minutes);

            var match = RelativeTimeRegex().Match(inputStr);
            if (match.Success)
            {
                var val = int.Parse(match.Groups[1].Value);
                return match.Groups[2].Value.StartsWith("hour", StringComparison.OrdinalIgnoreCase)
                    ? DateTime.Now.AddHours(val)
                    : DateTime.Now.AddMinutes(val);
            }

            if (DateTime.TryParse(inputStr, out var targetTime))
                return targetTime;

            UIDraw.TextColor = ConsoleColor.Red;
            UIDraw.DrawBoxedMessage("Could not parse time input.");
        }
    }

    private static ConfirmResult ConfirmAndExecute(PowerAction action, DateTime targetTime)
    {
        while (true)
        {
            Console.Clear();
            var seconds = Math.Max(0, (int)(targetTime - DateTime.Now).TotalSeconds);
            var minutes = (int)Math.Ceiling((targetTime - DateTime.Now).TotalMinutes);
            var hours = minutes / 60;

            UIDraw.TextColor = Variables.MenuColor;

            var translatedType = Localization.Get(action == PowerAction.Shutdown ? "Shutdown" : "Reboot");

            UIDraw.DrawMenu(null,
                [
                    Localization.Get("Schedule_ConfirmActionSeconds", translatedType, minutes),
                    Localization.Get("Schedule_TimeBreakdown", hours),
                ],
                [
                    "1) " + Localization.Get("Schedule_ConfirmYes", translatedType),
                    "2) " + Localization.Get("Schedule_ConfirmNoReenter"),
                    "0) " + Localization.Get("BackToMainMenu"),
                ]);

            UIDraw.TextColor = ConsoleColor.White;
            UIDraw.Draw(Localization.Get("Schedule_InputPrompt") + " ");

            var confirmKey = Console.ReadKey().KeyChar;
            Console.Clear();

            switch (confirmKey)
            {
                case '0':
                    return ConfirmResult.Cancelled;
                case '2':
                    return ConfirmResult.Reenter;
                case '1':
                    return ExecuteSchedule(action, targetTime, seconds) ? ConfirmResult.Done : ConfirmResult.Cancelled;
                default:
                    UIDraw.TextColor = ConsoleColor.Red;
                    UIDraw.DrawBoxedMessage(Localization.Get("Schedule_ErrorOccurred"));
                    UIDraw.TextColor = ConsoleColor.White;
                    break;
            }
        }
    }

    private static bool ExecuteSchedule(PowerAction action, DateTime targetTime, int seconds)
    {
        try
        {
            UIDraw.TextColor = Variables.MenuColor;
            UIDraw.DrawBoxedMessage("Recurring schedule? (d)aily, (w)eekly, (n)one");
            UIDraw.TextColor = ConsoleColor.White;
            var recur = char.ToLowerInvariant(Console.ReadKey().KeyChar);
            Console.Clear();

            if (recur is 'd' or 'w')
            {
                TaskSchedulerService.Register(
                    recur == 'd' ? TaskSchedulerService.Recurrence.Daily : TaskSchedulerService.Recurrence.Weekly,
                    targetTime,
                    action == PowerAction.Shutdown ? "/s" : "/r",
                    $"reShut CLI scheduled {(action == PowerAction.Shutdown ? "shutdown" : "reboot")}");
            }
            else
            {
                // A one-off delay is just a shutdown with a grace period - the same thing
                // "shutdown /t" asks the system for, without spawning anything.
                var delay = (uint)Math.Max(0, seconds);
                if (action == PowerAction.Shutdown) PowerManager.Shutdown(delay);
                else PowerManager.Reboot(delay);
            }

            UIDraw.TextColor = ConsoleColor.Green;
            UIDraw.DrawBoxedMessage(Localization.Get("Schedule_ActionScheduled"));
            UIDraw.TextColor = ConsoleColor.White;
            return true;
        }
        catch (Exception ex)
        {
            // The Task Scheduler reports failures as COM HRESULTs, which are worth
            // surfacing - the previous catch discarded them entirely.
            UIDraw.TextColor = ConsoleColor.Red;
            UIDraw.DrawBoxedMessage($"{Localization.Get("Schedule_ErrorOccurred")}\n{ex.Message}");
            UIDraw.TextColor = ConsoleColor.White;
            return false;
        }
    }
}
