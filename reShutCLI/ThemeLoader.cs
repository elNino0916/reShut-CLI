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
                Console.ForegroundColor = ConsoleColor.DarkGray;
                UIDraw.DrawBoxedMessage("Trying to fetch theme data from the API...");
                var fetchTask = client.GetStringAsync("http://api.elnino0916.de/api/v1/reshutcli/theme/default");

                    // API call finished within 2 seconds
                    var response = await fetchTask; // get result
                    var theme = JsonSerializer.Deserialize<ApiTheme>(response);

                    Variables.MenuColor = ConvertToConsoleColor(theme.MenuColor);
                    Variables.LogoColor = ConvertToConsoleColor(theme.LogoColor);
                    Variables.SecondaryColor = ConvertToConsoleColor(theme.SecondaryColor);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                Thread.Sleep(1000);
                Console.Clear();
                    UIDraw.DrawBoxedMessage("Theme data has been downloaded!");
            }
            catch (Exception)
            {
                setDefaultThemeFB();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Clear();
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

        private static ConsoleColor ConvertToConsoleColor(string color)
        {
            return color?.ToLower() switch
            {
                "black" => ConsoleColor.Black,
                "darkred" => ConsoleColor.DarkRed,
                "red" => ConsoleColor.Red,
                "darkgreen" => ConsoleColor.DarkGreen,
                "green" => ConsoleColor.Green,
                "darkblue" => ConsoleColor.DarkBlue,
                "blue" => ConsoleColor.Blue,
                "cyan" => ConsoleColor.Cyan,
                "darkcyan" => ConsoleColor.DarkCyan,
                "yellow" => ConsoleColor.Yellow,
                "magenta" => ConsoleColor.Magenta,
                "white" => ConsoleColor.White,
                "gray" => ConsoleColor.Gray,
                "darkgray" => ConsoleColor.DarkGray,
                _ => ConsoleColor.White
            };
        }

        private class ApiTheme
        {
            public string MenuColor { get; set; }
            public string LogoColor { get; set; }
            public string SecondaryColor { get; set; }
        }
    }
}
