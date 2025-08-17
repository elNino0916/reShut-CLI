using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace reShutCLI
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

                var response = await LoadingSpinner.RunAsync(
                    () => client.GetStringAsync("http://api.elnino0916.de/api/v2/reshutcli/theme/default"),
                    "Fetching V2 theme from API...");

                var theme = JsonSerializer.Deserialize<ApiTheme>(response);
                Variables.MenuColor = theme.MenuColor;
                Variables.LogoColor = theme.LogoColor;
                Variables.SecondaryColor = theme.SecondaryColor;
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
        }
    }
}
