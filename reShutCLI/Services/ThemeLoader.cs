using System.Text.Json;
using reShutCLI.Helpers;

namespace reShutCLI.Services;

internal static class ThemeLoader
{
    public static void LoadTheme()
    {
        var selectedTheme = RegistryWorker.ReadFromRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSelectedTheme);

        switch (selectedTheme)
        {
            case "default":
                SetDefaultTheme();
                break;
            case "red":
                SetTheme(menu: ConsoleColor.Red, logo: ConsoleColor.DarkRed, secondary: ConsoleColor.Magenta);
                break;
            case "blue":
                SetTheme(menu: ConsoleColor.Blue, logo: ConsoleColor.DarkBlue, secondary: ConsoleColor.DarkGreen);
                break;
            case "green":
                SetTheme(menu: ConsoleColor.Green, logo: ConsoleColor.DarkGreen, secondary: ConsoleColor.Blue);
                break;
            case "nord":
                SetTheme(menu: ConsoleColor.DarkCyan, logo: ConsoleColor.Cyan, secondary: ConsoleColor.DarkGray);
                break;
            default:
                SetFallbackTheme();
                break;
        }
    }

    /// <summary>Loads the dynamic default theme from the theme API.</summary>
    public static void SetDefaultTheme() => SetThemeFromApiAsync().GetAwaiter().GetResult();

    public static void SetFallbackTheme() =>
        SetTheme(menu: ConsoleColor.White, logo: ConsoleColor.Gray, secondary: ConsoleColor.Red);

    private static void SetTheme(CliColor menu, CliColor logo, CliColor secondary)
    {
        Variables.MenuColor = menu;
        Variables.LogoColor = logo;
        Variables.SecondaryColor = secondary;
    }

    private static async Task SetThemeFromApiAsync()
    {
        try
        {
            Console.Title = "reShutCLI - Loading Theme...";
            Console.ForegroundColor = ConsoleColor.DarkGray;
            using var cts = new CancellationTokenSource();

            // Spinner while the theme is fetched.
            var spinnerTask = Task.Run(async () =>
            {
                char[] frames = ['⠋', '⠙', '⠹', '⠸', '⠼', '⠴', '⠦', '⠧', '⠇', '⠏'];
                var i = 0;
                while (!cts.Token.IsCancellationRequested)
                {
                    UIDraw.DrawCentered($"\r{frames[i++ % frames.Length]} Fetching theme...");
                    await Task.Delay(100, cts.Token).ContinueWith(_ => { });
                }
            }, cts.Token);

            var fetchTask = Http.Client.GetStringAsync(Variables.ApiUrl);
            await Task.WhenAll(fetchTask, Task.Delay(1000));

            var theme = JsonSerializer.Deserialize(await fetchTask, ApiJsonContext.Default.ApiTheme)
                        ?? throw new InvalidOperationException("Theme API returned no data.");

            Variables.MenuColor = new CliColor(theme.MenuColor);
            Variables.LogoColor = new CliColor(theme.LogoColor);
            Variables.SecondaryColor = new CliColor(theme.SecondaryColor);
            Variables.BackgroundColor = new CliColor(theme.SecondaryColor);
            Variables.UpdatedDefaultThemeName = theme.ThemeName ?? "";

            await cts.CancelAsync();
            await spinnerTask;
            Console.Clear();
        }
        catch (Exception)
        {
            Console.Clear();
            SetFallbackTheme();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            UIDraw.DrawBoxedMessage("Using fallback theme!");
        }
    }
}
