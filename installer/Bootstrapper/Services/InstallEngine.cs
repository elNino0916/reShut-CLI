using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Microsoft.Win32;

namespace reShutCLI.Bootstrapper.Services;

/// <summary>Orchestrates a full install: runtime dependency, legacy migration, files, shortcuts, registry.</summary>
internal static class InstallEngine
{
    public static string DefaultInstallDir =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "reShut CLI");

    public static async Task RunAsync(InstallRequest request, IProgress<InstallProgress>? progress, CancellationToken ct)
    {
        progress ??= new Progress<InstallProgress>();
        Directory.CreateDirectory(request.InstallDir);

        if (DotNetRuntimeService.IsInstalled())
        {
            progress.Report(new InstallProgress(60, $".NET {AppConstants.DotNetMajorVersion} runtime already installed."));
        }
        else
        {
            await DotNetRuntimeService.InstallAsync(Scoped(progress, 5, 60), ct);
        }

        await LegacyMigrationService.MigrateAsync(Scoped(progress, 60, 70), ct);

        progress.Report(new InstallProgress(72, "Copying application files..."));
        ExtractPayload(request.InstallDir, Scoped(progress, 72, 90));

        if (request.CreateShortcut)
        {
            progress.Report(new InstallProgress(91, "Creating shortcuts..."));
            CreateShortcuts(request.InstallDir);
        }

        progress.Report(new InstallProgress(94, "Registering application..."));
        WriteUninstallEntry(request.InstallDir);

        progress.Report(new InstallProgress(97, "Finishing up..."));
        WriteUninstaller(request.InstallDir);

        progress.Report(new InstallProgress(100, "Done."));
    }

    private static void ExtractPayload(string installDir, IProgress<InstallProgress> progress)
    {
        using var payload = Assembly.GetExecutingAssembly().GetManifestResourceStream("payload.zip")
                             ?? throw new InvalidOperationException(
                                 "No application payload is embedded in this installer build. " +
                                 "Build via build-installer.ps1, which publishes the app and embeds it first.");

        using var archive = new ZipArchive(payload, ZipArchiveMode.Read);
        var entries = archive.Entries.Where(e => !string.IsNullOrEmpty(e.Name)).ToList();

        for (var i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];
            var destination = Path.Combine(installDir, entry.FullName.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            entry.ExtractToFile(destination, overwrite: true);

            progress.Report(new InstallProgress((double)(i + 1) / entries.Count * 100, "Copying application files..."));
        }
    }

    private static void CreateShortcuts(string installDir)
    {
        var exePath = Path.Combine(installDir, AppConstants.AppExeName);
        var startMenuDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs", AppConstants.AppName);
        Directory.CreateDirectory(startMenuDir);

        ShortcutService.CreateShortcut(Path.Combine(startMenuDir, $"{AppConstants.AppName}.lnk"), exePath);
        ShortcutService.CreateShortcut(
            Path.Combine(startMenuDir, $"Uninstall {AppConstants.AppName}.lnk"),
            Path.Combine(installDir, AppConstants.UninstallExeName));
    }

    private static void WriteUninstallEntry(string installDir)
    {
        using var key = Registry.LocalMachine.CreateSubKey(AppConstants.UninstallKey);
        key.SetValue("DisplayName", AppConstants.AppName);
        key.SetValue("DisplayVersion", AppConstants.Version);
        key.SetValue("Publisher", AppConstants.Publisher);
        key.SetValue("URLInfoAbout", AppConstants.ProjectUrl);
        key.SetValue("HelpLink", AppConstants.ProjectUrl);
        key.SetValue("InstallLocation", installDir);
        key.SetValue("DisplayIcon", Path.Combine(installDir, AppConstants.AppExeName));
        key.SetValue("UninstallString", $"\"{Path.Combine(installDir, AppConstants.UninstallExeName)}\"");
        key.SetValue("QuietUninstallString", $"\"{Path.Combine(installDir, AppConstants.UninstallExeName)}\" /S");
        key.SetValue("NoModify", 1, RegistryValueKind.DWord);
        key.SetValue("NoRepair", 1, RegistryValueKind.DWord);

        long sizeBytes;
        try
        {
            sizeBytes = Directory.GetFiles(installDir, "*", SearchOption.AllDirectories).Sum(f => new FileInfo(f).Length);
        }
        catch
        {
            sizeBytes = 0;
        }
        key.SetValue("EstimatedSize", (int)(sizeBytes / 1024), RegistryValueKind.DWord);
    }

    private static void WriteUninstaller(string installDir)
    {
        var currentExePath = Assembly.GetExecutingAssembly().Location;
        var uninstallerPath = Path.Combine(installDir, AppConstants.UninstallExeName);
        File.Copy(currentExePath, uninstallerPath, overwrite: true);
    }

    /// <summary>Remaps an inner [0,100] progress range onto an outer [from,to] slice.</summary>
    private static IProgress<InstallProgress> Scoped(IProgress<InstallProgress> outer, double from, double to) =>
        new Progress<InstallProgress>(p => outer.Report(new InstallProgress(from + p.Percent / 100 * (to - from), p.Status)));
}
