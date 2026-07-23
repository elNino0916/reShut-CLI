using System.Text.Json;
using reShutCLI.Helpers;

namespace reShutCLI.Services;

internal static class UpdateChecker
{
    private static bool _updateCheckPerformed;
    private static string _updateResultMessage = string.Empty;

    public static async Task MainCheck()
    {
        // Only hit the network once per session; afterwards replay the result.
        if (_updateCheckPerformed)
        {
            UIDraw.TextColor = Variables.MenuColor;
            UIDraw.DrawCenteredLine(_updateResultMessage);
            UIDraw.TextColor = ConsoleColor.Gray;
            return;
        }

        using var response = await Http.Client.GetAsync(Constants.GitHubLatestReleaseApiUrl);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var release = JsonSerializer.Deserialize(json, ApiJsonContext.Default.GitHubRelease);

            if (IsNewerVersionAvailable(Variables.Version, release?.TagName))
            {
                UIDraw.TextColor = Variables.MenuColor;
                UIDraw.DrawBoxedMessage(Localization.Get("UpdateAvailable"));
                Variables.IsUpToDate = false;
                UIDraw.TextColor = ConsoleColor.Gray;

                if (RegistryWorker.ReadFromRegistry(Constants.RegistryPathConfig, Constants.RegistryValueAutoUpdateOnStart)
                    == Constants.EnabledValue)
                {
                    await AutoUpdater.PerformUpdate();
                }
            }
            else
            {
                Variables.IsUpToDate = true;
                UIDraw.TextColor = Variables.MenuColor;
                _updateResultMessage = Variables.Motd();
                UIDraw.DrawCenteredLine(_updateResultMessage);
                UIDraw.TextColor = ConsoleColor.Gray;
            }
        }
        else
        {
            UIDraw.TextColor = ConsoleColor.Red;
            UIDraw.DrawBoxedMessage($"Failed to check for updates: {response.StatusCode}. Restart the application to try again.");
            UIDraw.TextColor = ConsoleColor.Gray;
        }

        _updateCheckPerformed = true;
    }

    public static bool IsNewerVersionAvailable(string? currentVersion, string? latestVersion)
    {
        if (string.IsNullOrEmpty(currentVersion) || string.IsNullOrEmpty(latestVersion))
            return false;

        return System.Version.TryParse(latestVersion.TrimStart('v', 'V'), out var latest)
               && System.Version.TryParse(currentVersion.TrimStart('v', 'V'), out var current)
               && latest > current;
    }
}
