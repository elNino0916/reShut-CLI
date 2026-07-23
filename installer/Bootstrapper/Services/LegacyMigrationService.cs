using System.Diagnostics;
using Microsoft.Win32;

namespace reShutCLI.Bootstrapper.Services;

/// <summary>Finds and silently removes a pre-2.1 Inno Setup based installation.</summary>
internal static class LegacyMigrationService
{
    public static async Task MigrateAsync(IProgress<InstallProgress> progress, CancellationToken ct)
    {
        var command = FindUninstallCommand();
        if (command is null) return;

        progress.Report(new InstallProgress(0, "Removing previous version..."));

        var (fileName, arguments) = command.Value;
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using (var process = Process.Start(psi))
        {
            if (process is not null)
            {
                await process.WaitForExitAsync(ct);
            }
        }

        // Inno uninstallers respawn from a temp copy, so the process above can
        // exit before the uninstall has actually finished on disk. Give it a
        // moment to complete before we start laying down the new install.
        await Task.Delay(5000, ct);
    }

    private static (string FileName, string Arguments)? FindUninstallCommand()
    {
        return TryReadFrom(RegistryHive.LocalMachine, RegistryView.Registry64)
               ?? TryReadFrom(RegistryHive.LocalMachine, RegistryView.Registry32)
               ?? TryReadFrom(RegistryHive.CurrentUser, RegistryView.Registry64);
    }

    private static (string FileName, string Arguments)? TryReadFrom(RegistryHive hive, RegistryView view)
    {
        using var baseKey = RegistryKey.OpenBaseKey(hive, view);
        using var key = baseKey.OpenSubKey(AppConstants.OldInnoUninstallKeyName);
        if (key is null) return null;

        if (key.GetValue("QuietUninstallString") is string quiet && !string.IsNullOrWhiteSpace(quiet))
            return SplitCommand(quiet, extraArgs: null);

        if (key.GetValue("UninstallString") is string plain && !string.IsNullOrWhiteSpace(plain))
            return SplitCommand(plain, extraArgs: "/VERYSILENT /SUPPRESSMSGBOXES /NORESTART");

        return null;
    }

    /// <summary>
    /// Inno's registered uninstall strings are always a quoted exe path, optionally
    /// followed by arguments (e.g. `"C:\...\unins000.exe" /SILENT`). Splits the
    /// leading quoted executable from the rest so Process.Start gets a clean path.
    /// </summary>
    private static (string FileName, string Arguments) SplitCommand(string raw, string? extraArgs)
    {
        raw = raw.Trim();
        string fileName;
        string rest;

        if (raw.StartsWith("\"", StringComparison.Ordinal))
        {
            var closingQuote = raw.IndexOf('"', 1);
            if (closingQuote > 0)
            {
                fileName = raw.Substring(1, closingQuote - 1);
                rest = raw.Substring(closingQuote + 1).Trim();
            }
            else
            {
                fileName = raw.Trim('"');
                rest = "";
            }
        }
        else
        {
            var spaceIndex = raw.IndexOf(' ');
            if (spaceIndex > 0)
            {
                fileName = raw.Substring(0, spaceIndex);
                rest = raw.Substring(spaceIndex + 1).Trim();
            }
            else
            {
                fileName = raw;
                rest = "";
            }
        }

        var arguments = string.IsNullOrEmpty(extraArgs) ? rest : (rest.Length > 0 ? $"{rest} {extraArgs}" : extraArgs ?? "");
        return (fileName, arguments);
    }
}
