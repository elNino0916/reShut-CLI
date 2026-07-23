using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.Win32;

namespace reShutCLI.Bootstrapper.Services;

/// <summary>Detects and, if missing, installs the .NET runtime the main app needs.</summary>
internal static class DotNetRuntimeService
{
    /// <summary>
    /// True when a .NET <see cref="AppConstants.DotNetMajorVersion"/>.x shared runtime is
    /// registered. The .NET host installer records every installed shared framework version
    /// as a value name under the sharedfx key; we always read the 64-bit registry view since
    /// that's where the x64 runtime we install is recorded, regardless of our own bitness.
    /// </summary>
    public static bool IsInstalled()
    {
        using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        using var key = baseKey.OpenSubKey(AppConstants.DotNetSharedFxRegKey);
        if (key is null) return false;

        var prefix = AppConstants.DotNetMajorVersion + ".";
        return key.GetValueNames().Any(name => name.StartsWith(prefix, StringComparison.Ordinal));
    }

    public static async Task InstallAsync(IProgress<InstallProgress> progress, CancellationToken ct)
    {
        progress.Report(new InstallProgress(0, $"Downloading .NET {AppConstants.DotNetMajorVersion} runtime..."));

        var installerPath = Path.Combine(Path.GetTempPath(), "dotnet-runtime-win-x64.exe");
        await DownloadAsync(AppConstants.DotNetRuntimeDownloadUrl, installerPath, progress, ct);

        progress.Report(new InstallProgress(75, $"Installing .NET {AppConstants.DotNetMajorVersion} runtime..."));

        var psi = new ProcessStartInfo
        {
            FileName = installerPath,
            Arguments = "/install /quiet /norestart",
            UseShellExecute = false,
        };

        using (var process = Process.Start(psi) ?? throw new InvalidOperationException("Failed to start the .NET runtime installer."))
        {
            await process.WaitForExitAsync(ct);

            // 0 = success, 3010 = success but reboot required, 1638 = a newer version is already installed.
            if (process.ExitCode is not (0 or 3010 or 1638))
            {
                throw new InvalidOperationException(
                    $"The .NET {AppConstants.DotNetMajorVersion} runtime installer failed (exit code {process.ExitCode}).");
            }
        }

        try { File.Delete(installerPath); } catch { /* best effort */ }

        progress.Report(new InstallProgress(100, $".NET {AppConstants.DotNetMajorVersion} runtime installed."));
    }

    private static async Task DownloadAsync(string url, string destinationPath, IProgress<InstallProgress> progress, CancellationToken ct)
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, ct);
        response.EnsureSuccessStatusCode();
        var totalBytes = response.Content.Headers.ContentLength;

        // .NET Framework 4.8 lacks the CancellationToken overload of ReadAsStreamAsync
        // and the Memory<byte>-based Stream.Read/WriteAsync overloads, so this sticks to
        // the classic byte[]+offset+count APIs available since .NET Framework 4.5.
        using var source = await response.Content.ReadAsStreamAsync();
        using var target = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);

        var buffer = new byte[81920];
        long readSoFar = 0;
        int read;
        while ((read = await source.ReadAsync(buffer, 0, buffer.Length, ct)) > 0)
        {
            await target.WriteAsync(buffer, 0, read, ct);
            readSoFar += read;

            if (totalBytes is > 0)
            {
                // Download maps to the first 70% of this step's progress bar.
                var percent = (double)readSoFar / totalBytes.Value * 70;
                progress.Report(new InstallProgress(percent, $"Downloading .NET {AppConstants.DotNetMajorVersion} runtime..."));
            }
        }
    }
}
