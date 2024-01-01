using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace reShutLegacy
{
    internal class UpdateChecker
    {
        private static bool updateCheckPerformed = false;
        private static string updateResultMessage = string.Empty;
        private static string updateResultMessageLine2 = string.Empty;

        public static void DisplayCenteredMessage(string message)
        {
            var consoleWidth = Console.WindowWidth;
            var padding = (consoleWidth - message.Length) / 2;

            Console.WriteLine(new string(' ', padding) + message);
        }

        public static async Task MainCheck()
        {
            // Check if update has already been checked
            if (updateCheckPerformed)
            {
                // Output the stored result
                Console.ForegroundColor = Variables.MenuColor;
                DisplayCenteredMessage(updateResultMessage);
                DisplayCenteredMessage(updateResultMessageLine2);
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            const string repositoryUrl = "https://api.github.com/repos/elnino0916/reshut-legacy/releases/latest";
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("reShutLegacy_UpdateSearch");

            var response = await client.GetAsync(repositoryUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var release = JsonSerializer.Deserialize<GitHubRelease>(json);

                var latestVersion = release.tag_name;
                var currentVersion = Variables.version;

                if (IsNewerVersionAvailable(currentVersion, latestVersion))
                {
                    Console.ForegroundColor = Variables.MenuColor;
                    updateResultMessage = $"A new version ({latestVersion}) of reShut-Legacy is available! Please download the update from";
                    DisplayCenteredMessage(updateResultMessage);
                    updateResultMessageLine2 = "https://github.com/elnino0916/reShut-Legacy/releases/latest";
                    DisplayCenteredMessage(updateResultMessageLine2);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.ForegroundColor = Variables.MenuColor;
                    updateResultMessage = Variables.Motd();
                    DisplayCenteredMessage(updateResultMessage);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                updateResultMessage = $"Failed to check for updates: {response.StatusCode}. Restart reShut-Legacy to refresh.";
                DisplayCenteredMessage(updateResultMessage);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            // Set the flags to indicate that update check has been performed
            updateCheckPerformed = true;
        }

        static bool IsNewerVersionAvailable(string currentVersion, string latestVersion)
        {
            if (string.IsNullOrEmpty(currentVersion) || string.IsNullOrEmpty(latestVersion))
                return false;

            return new Version(latestVersion) > new Version(currentVersion);
        }

        public class GitHubRelease
        {
            #pragma warning disable IDE1006
            public string tag_name { get; set; }
            #pragma warning restore IDE1006
        }
    }
}