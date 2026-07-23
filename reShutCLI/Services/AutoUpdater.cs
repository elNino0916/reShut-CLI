using System.Diagnostics;
using System.Text.Json;
using reShutCLI.Helpers;

namespace reShutCLI.Services;

internal static class AutoUpdater
{
    public static async Task PerformUpdate()
    {
        try
        {
            using var response = await Http.Client.GetAsync(Constants.GitHubLatestReleaseApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                ShowUpdateError($"Failed to check for updates: {response.StatusCode}");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var release = JsonSerializer.Deserialize(json, ApiJsonContext.Default.GitHubRelease);

            if (release is null || !UpdateChecker.IsNewerVersionAvailable(Variables.Version, release.TagName))
            {
                UIDraw.TextColor = Variables.MenuColor;
                ErrorHandler.ShowError("Tried to update to the same version currently installed.", true);
                return;
            }

            // Prefer an installer executable; fall back to the first asset.
            var asset = release.Assets.FirstOrDefault(a =>
                            a.Name?.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) == true)
                        ?? release.Assets.FirstOrDefault();

            if (asset?.BrowserDownloadUrl is not { } downloadUrl)
            {
                ShowUpdateError("The latest release has no downloadable installer.");
                return;
            }

            var installerPath = await DownloadInstaller(downloadUrl);
            if (!string.IsNullOrEmpty(installerPath))
            {
                StartInstaller(installerPath);
            }
            else
            {
                ShowUpdateError("Failed to download the installer.");
            }
        }
        catch (Exception ex)
        {
            ShowUpdateError($"Failed to update: {ex.Message}");
        }
    }

    private static async Task<string> DownloadInstaller(string downloadUrl)
    {
        try
        {
            var fileName = Path.GetFileName(new Uri(downloadUrl).LocalPath);
            var filePath = Path.Combine(Path.GetTempPath(), fileName);

            using var response = await Http.Client.GetAsync(downloadUrl);
            response.EnsureSuccessStatusCode();

            await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await response.Content.CopyToAsync(fileStream);

            return filePath;
        }
        catch (Exception ex)
        {
            ShowUpdateError($"Failed to download the installer: {ex.Message}");
            return string.Empty;
        }
    }

    private static void StartInstaller(string installerPath)
    {
        try
        {
            // /S is the NSIS silent flag (installers >= 2.1). Older Inno Setup
            // installers ignore it and show their UI, which is acceptable.
            Process.Start(new ProcessStartInfo
            {
                FileName = installerPath,
                Arguments = "/S",
                UseShellExecute = true,
            });

            Environment.Exit(Constants.ExitCodeSuccess);
        }
        catch (Exception ex)
        {
            ShowUpdateError($"Failed to start the installer: {ex.Message}");
        }
    }

    private static void ShowUpdateError(string message)
    {
        UIDraw.TextColor = ConsoleColor.Red;
        ErrorHandler.ShowError(message, true);
        UIDraw.TextColor = ConsoleColor.Gray;
    }
}
