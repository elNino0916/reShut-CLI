using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace reShutLegacy
{
    internal class UpdateChecker
    {
        static void DisplayCenteredMessage(string message)
        {
            int consoleWidth = Console.WindowWidth;

            int padding = (consoleWidth - message.Length) / 2;

            Console.WriteLine(new string(' ', padding) + message);
        }

        public static async Task MainCheck()
        {
            string repositoryUrl = "https://api.github.com/repos/elnino0916/reshut-legacy/releases/latest";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("reShut-Legacy");

                HttpResponseMessage response = await client.GetAsync(repositoryUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var release = JsonSerializer.Deserialize<GitHubRelease>(json);

                    string latestVersion = release.tag_name;
                    string currentVersion = Variables.version;

                    Console.WriteLine("");
                    if (IsNewerVersionAvailable(currentVersion, latestVersion))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        DisplayCenteredMessage($"A new version ({latestVersion}) of reShut-Legacy is available! Please download the update from:");
                        DisplayCenteredMessage($"https://github.com/elnino0916/reShut-Legacy");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        DisplayCenteredMessage(Variables.Motd());
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Failed to check for updates: {response.StatusCode}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
            static bool IsNewerVersionAvailable(string currentVersion, string latestVersion)
            {
                if (string.IsNullOrEmpty(currentVersion) || string.IsNullOrEmpty(latestVersion))
                    return false;

                return new Version(latestVersion) > new Version(currentVersion);
            }
        }

        public class GitHubRelease
        {
            public string tag_name { get; set; }
        }
    }
}

