using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using reShutCLI.Helpers;

namespace reShutCLI.Services
{
    internal class ThemeLoader
    {
        private static readonly HttpClient client = new HttpClient();

        public static void loadTheme()
        {
            var selectedTheme = RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme");

            switch (selectedTheme)
            {
                case "default":
                    setThemeFromApiAsync().Wait();
                    break;
                case "red":
                    setRedTheme();
                    break;
                case "blue":
                    setBlueTheme();
                    break;
                case "green":
                    setGreenTheme();
                    break;
                case "nord":
                    setNordTheme();
                    break;
                default:
                    setDefaultThemeFB();
                    break;
            }
        }

        private static async Task setThemeFromApiAsync()
        {
            try
            {
                Console.Title = "reShutCLI - Loading Theme...";
                Console.ForegroundColor = ConsoleColor.DarkGray;
                using var cts = new CancellationTokenSource();

                // Start spinner in background
                var spinnerTask = Task.Run(async () =>
                {
                    char[] frames = { '\u280B', '\u2819', '\u2839', '\u2838', '\u283C', '\u2834', '\u2826', '\u2827', '\u2807', '\u280F' };
                    int i = 0;
                    while (!cts.Token.IsCancellationRequested)
                    {
                        UIDraw.DrawCentered($"\r{frames[i++ % frames.Length]} Fetching theme...");
                        await Task.Delay(100, cts.Token).ContinueWith(_ => { });
                    }
                }, cts.Token);

                // Perform API call
                var fetchTask = client.GetStringAsync(Variables.apiString);

                await Task.WhenAll(fetchTask, Task.Delay(1000));

                var response = await fetchTask;
                var theme = JsonSerializer.Deserialize<ApiTheme>(response);

                Variables.MenuColor = theme.MenuColor;
                Variables.LogoColor = theme.LogoColor;
                Variables.SecondaryColor = theme.SecondaryColor;
                Variables.BackgroundColor = theme.SecondaryColor;
                Variables.UpdatedDefaultThemeName = theme.ThemeName;

                // Stop spinner
                cts.Cancel();
                await spinnerTask;
                Console.Clear();
            }
            catch (Exception)
            {
                Console.Clear();
                setDefaultThemeFB();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                UIDraw.DrawBoxedMessage("Using fallback theme!");
            }
        }

        public static void setDefaultTheme()
        {
            setThemeFromApiAsync().Wait();
        }

        public static void setDefaultThemeFB()
        {
            Variables.MenuColor = ConsoleColor.White;
            Variables.LogoColor = ConsoleColor.Gray;
            Variables.SecondaryColor = ConsoleColor.Red;
        }

        public static void setRedTheme()
        {
            Variables.MenuColor = ConsoleColor.Red;
            Variables.LogoColor = ConsoleColor.DarkRed;
            Variables.SecondaryColor = ConsoleColor.Magenta;
        }

        public static void setBlueTheme()
        {
            Variables.MenuColor = ConsoleColor.Blue;
            Variables.LogoColor = ConsoleColor.DarkBlue;
            Variables.SecondaryColor = ConsoleColor.DarkGreen;
        }

        public static void setGreenTheme()
        {
            Variables.MenuColor = ConsoleColor.Green;
            Variables.LogoColor = ConsoleColor.DarkGreen;
            Variables.SecondaryColor = ConsoleColor.Blue;
        }

        public static void setNordTheme()
        {
            Variables.MenuColor = ConsoleColor.DarkCyan;
            Variables.LogoColor = ConsoleColor.Cyan;
            Variables.SecondaryColor = ConsoleColor.DarkGray;
        }

        private class ApiTheme
        {
            public string MenuColor { get; set; }
            public string LogoColor { get; set; }
            public string SecondaryColor { get; set; }
            public string ThemeName { get; set; }
        }
    }
}
