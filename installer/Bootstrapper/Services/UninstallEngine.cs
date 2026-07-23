using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace reShutCLI.Bootstrapper.Services;

/// <summary>Removes a reShut CLI installation: files, shortcuts, registry, optionally user settings.</summary>
internal static class UninstallEngine
{
    public static Task RunAsync(bool removeUserSettings, IProgress<InstallProgress> progress, CancellationToken ct)
    {
        progress.Report(new InstallProgress(10, "Removing shortcuts..."));
        var installDir = ResolveInstallDir();
        var stuckShortcutDirs = RemoveShortcuts();

        progress.Report(new InstallProgress(30, "Removing scheduled tasks..."));
        RemoveScheduledTasks();

        progress.Report(new InstallProgress(40, "Removing registry entries..."));
        Registry.LocalMachine.DeleteSubKeyTree(AppConstants.UninstallKey, throwOnMissingSubKey: false);

        if (removeUserSettings)
        {
            progress.Report(new InstallProgress(55, "Removing settings..."));
            Registry.CurrentUser.DeleteSubKeyTree(AppConstants.UserSettingsKey, throwOnMissingSubKey: false);
            Registry.CurrentUser.DeleteSubKeyTree(AppConstants.UserPoliciesKey, throwOnMissingSubKey: false);
        }

        progress.Report(new InstallProgress(75, "Removing application files..."));
        RemoveDeferred(installDir, stuckShortcutDirs);

        progress.Report(new InstallProgress(100, "Done."));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Where the application files live. The registry is authoritative, but an uninstall
    /// that got part way through leaves the key deleted and the files in place, which
    /// strands the installation for good. Our own directory is the correct fallback -
    /// we run as InstallDir\uninstall.exe - and the name check keeps that fallback from
    /// firing for a setup.exe someone is running out of their Downloads folder.
    /// </summary>
    private static string? ResolveInstallDir()
    {
        using var key = Registry.LocalMachine.OpenSubKey(AppConstants.UninstallKey);
        if (key?.GetValue("InstallLocation") is string registered && !string.IsNullOrWhiteSpace(registered))
        {
            return registered;
        }

        var exePath = Assembly.GetExecutingAssembly().Location;
        return string.Equals(Path.GetFileName(exePath), AppConstants.UninstallExeName, StringComparison.OrdinalIgnoreCase)
            ? Path.GetDirectoryName(exePath)
            : null;
    }

    /// <summary>
    /// Every Start Menu folder an install could have put shortcuts in. The current
    /// installer writes to the machine-wide menu, but installs from earlier builds
    /// landed in the per-user one, and cleaning only the former leaves those behind
    /// forever - the uninstaller that would remove them has itself been deleted.
    /// </summary>
    private static IEnumerable<string> ShortcutDirectories()
    {
        foreach (var root in new[] { Environment.SpecialFolder.CommonStartMenu, Environment.SpecialFolder.StartMenu })
        {
            var basePath = Environment.GetFolderPath(root);

            // GetFolderPath returns "" for a folder it cannot resolve, and Path.Combine
            // would quietly turn that into a relative path under the working directory.
            if (string.IsNullOrEmpty(basePath)) continue;

            yield return Path.Combine(basePath, "Programs", AppConstants.AppName);
        }
    }

    /// <summary>
    /// Removes the Task Scheduler folder holding any recurring shutdowns the user
    /// scheduled. Left behind, those tasks would keep firing at an executable that no
    /// longer exists. Best effort throughout - a stubborn task is not worth failing an
    /// uninstall over, and the folder only exists at all if something was scheduled.
    /// </summary>
    private static void RemoveScheduledTasks()
    {
        object? service = null;
        try
        {
            var serviceType = Type.GetTypeFromProgID("Schedule.Service");
            if (serviceType is null) return;

            service = Activator.CreateInstance(serviceType);
            if (service is null) return;

            dynamic scheduler = service;
            scheduler.Connect();

            dynamic folder;
            try
            {
                folder = scheduler.GetFolder(AppConstants.TaskFolder);
            }
            catch
            {
                // No folder means nothing was ever scheduled.
                return;
            }

            // DeleteFolder refuses a folder that still holds tasks, so empty it first.
            // Names are collected up front rather than deleted mid-enumeration.
            // TASK_ENUM_HIDDEN (1) so hidden tasks are included in the sweep.
            var taskNames = new List<string>();
            foreach (dynamic task in folder.GetTasks(1))
            {
                taskNames.Add((string)task.Name);
            }

            foreach (var name in taskNames)
            {
                try
                {
                    folder.DeleteTask(name, 0);
                }
                catch
                {
                    // Skip it; the folder delete below will simply fail and be ignored.
                }
            }

            dynamic root = scheduler.GetFolder(@"\");
            root.DeleteFolder(AppConstants.TaskFolder, 0);
        }
        catch
        {
            // Task Scheduler unavailable, or access denied - nothing else depends on this.
        }
        finally
        {
            if (service is not null) Marshal.ReleaseComObject(service);
        }
    }

    /// <summary>Deletes the Start Menu folders, returning any that could not be fully removed.</summary>
    private static List<string> RemoveShortcuts() =>
        ShortcutDirectories().Where(dir => Directory.Exists(dir) && !TryDeleteDirectory(dir)).ToList();

    private static bool TryDeleteDirectory(string dir)
    {
        // Files go one at a time instead of relying solely on Directory.Delete(recursive: true):
        // that overload aborts on the first file it cannot delete, which left the folder
        // half-emptied - the app shortcut gone, but "Uninstall reShut CLI.lnk" still there,
        // since the shell holds the very shortcut this process was launched from.
        foreach (var file in Directory.GetFiles(dir))
        {
            try
            {
                File.Delete(file);
            }
            catch
            {
                // Handled by the deferred pass, which runs once this process has exited.
            }
        }

        try
        {
            Directory.Delete(dir, recursive: true);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>How long the detached helper keeps retrying, at roughly one attempt per second.</summary>
    private const int DeferredAttempts = 90;

    /// <summary>
    /// We're currently running as InstallDir\uninstall.exe, so we can't delete our own
    /// directory while executing. Spawns a detached helper that retries the removal until
    /// this process has exited and released its locks. "ping localhost" is the classic
    /// console-free delay trick, since `timeout` refuses to run without a console.
    ///
    /// It retries rather than waiting a fixed few seconds because this runs while the
    /// "Uninstall complete" page is still on screen: the process lives until the user
    /// clicks Finish, so a one-shot delete fires too early and strips the directory of
    /// everything except the running uninstall.exe. Repeating a delete that has already
    /// succeeded costs nothing.
    ///
    /// Start Menu folders that resisted deletion ride along, for the same reason: when
    /// the uninstaller was started from its own Start Menu shortcut, the shell keeps that
    /// .lnk busy for as long as this process lives.
    /// </summary>
    private static void RemoveDeferred(string? installDir, IEnumerable<string> shortcutDirs)
    {
        var targets = (installDir is null ? shortcutDirs : new[] { installDir }.Concat(shortcutDirs))
            // A trailing separator would escape the closing quote in the cmd line below.
            .Select(path => path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))
            .Where(path => !string.IsNullOrEmpty(path))
            .ToList();

        if (targets.Count == 0) return;

        var removals = string.Join(" & ", targets.Select(path => $"rmdir /s /q \"{path}\" 2>nul"));

        Process.Start(new ProcessStartInfo
        {
            FileName = "cmd.exe",
            // Single %% is correct here: %%i would only apply inside a batch file.
            Arguments = $"/c for /l %i in (1,1,{DeferredAttempts}) do (ping -n 2 127.0.0.1 >nul & {removals})",
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
        });
    }
}
