using reShutCLI.Helpers;
using reShutCLI.Services;

namespace reShutCLI;

internal static class ThemeSettings
{
    private static readonly (ConsoleKey Digit, ConsoleKey NumPad, string Theme)[] ThemeKeys =
    [
        (ConsoleKey.D1, ConsoleKey.NumPad1, "default"),
        (ConsoleKey.D2, ConsoleKey.NumPad2, "red"),
        (ConsoleKey.D3, ConsoleKey.NumPad3, "blue"),
        (ConsoleKey.D4, ConsoleKey.NumPad4, "green"),
        (ConsoleKey.D5, ConsoleKey.NumPad5, "nord"),
    ];

    public static void OpenSettings()
    {
        while (true)
        {
            Console.Clear();
            UIDraw.DrawBoxedMessage(Localization.Get("SelectTheme"));
            UIDraw.DrawCenteredLine("");
            UIDraw.TextColor = Variables.SecondaryColor;
            UIDraw.DrawBoxedMessage("Current default theme name: " + Variables.UpdatedDefaultThemeName);
            UIDraw.TextColor = Variables.MenuColor;
            UIDraw.DrawMenu(null,
                ["1) Default"],
                ["2) Red", "3) Blue", "4) Green", "5) Nord"],
                ["9) Back"]);
            UIDraw.DrawCenteredLine("");

            var keyInfo = Console.ReadKey();
            if (keyInfo.Key is ConsoleKey.D9 or ConsoleKey.NumPad9) return;

            var selection = ThemeKeys.FirstOrDefault(t => keyInfo.Key == t.Digit || keyInfo.Key == t.NumPad);
            if (selection.Theme is null) continue;

            RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSelectedTheme, Constants.RegistryValueTypeString, selection.Theme);
            ThemeLoader.LoadTheme();
            Console.Clear();
            AutoRestart.Init();
            return;
        }
    }
}
