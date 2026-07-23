using reShutCLI.Helpers;
using reShutCLI.Services;

namespace reShutCLI;

internal static class Settings
{
    public static void Show()
    {
        Console.Clear();
        while (true)
        {
            UIDraw.TextColor = Variables.MenuColor;
            Console.Title = Localization.Get("ConsoleTitle") + " " + Variables.FullVersion;

            UIDraw.DrawMenu(Localization.Get("Settings"),
                [
                    "1) " + Localization.Get("General"),
                    "2) " + Localization.Get("MenuAndText"),
                    "3) " + Localization.Get("About"),
                ],
                ["9) " + Localization.Get("Back")]);
            UIDraw.TextColor = ConsoleColor.White;

            switch (Console.ReadKey().KeyChar)
            {
                case '1':
                    ShowGeneral();
                    break;
                case '2':
                    ShowMenuAndText();
                    break;
                case '3':
                    AboutPage.Show();
                    break;
                case '9':
                    Console.Clear();
                    return;
                default:
                    Console.Clear();
                    UIDraw.TextColor = Variables.SecondaryColor;
                    UIDraw.DrawBoxedMessage(Localization.Get("InvalidInput"));
                    UIDraw.TextColor = ConsoleColor.White;
                    break;
            }
        }
    }

    private static void ShowGeneral()
    {
        while (true)
        {
            Console.Clear();
            UIDraw.TextColor = Variables.MenuColor;
            UIDraw.DrawMenu(Localization.Get("General"),
                [
                    "1) " + Localization.Get("Update"),
                    "2) " + Localization.Get("Theme"),
                    "3) " + Localization.Get("ResetAllSettings"),
                    "4) " + Localization.Get("Language"),
                ],
                ["9) " + Localization.Get("Back")]);

            switch (Console.ReadKey().KeyChar)
            {
                case '1':
                    ShowUpdateSettings();
                    break;
                case '2':
                    Console.Clear();
                    UIDraw.TextColor = Variables.MenuColor;
                    ThemeSettings.OpenSettings();
                    break;
                case '3':
                    ConfigManager.Reset();
                    break;
                case '4':
                    ShowLanguageSelection();
                    break;
                case '9':
                    Console.Clear();
                    return;
                default:
                    Console.Clear();
                    UIDraw.TextColor = Variables.SecondaryColor;
                    UIDraw.DrawBoxedMessage(Localization.Get("InvalidInput"));
                    UIDraw.TextColor = ConsoleColor.White;
                    break;
            }
        }
    }

    private static void ShowLanguageSelection()
    {
        Console.Clear();
        UIDraw.TextColor = Variables.MenuColor;
        UIDraw.DrawBoxedMessage(Localization.Get("Language"));
        UIDraw.DrawCenteredLine("");
        UIDraw.DrawBoxedMessage(Localization.Get("HelpTranslate") + " " + Constants.GitHubRepoUrl);
        UIDraw.DrawCenteredLine("");
        UIDraw.DrawBoxedMessage(Localization.Get("SelectLang"));
        UIDraw.DrawCenteredLine("");
        UIDraw.DrawMenu(null,
            ["1) English (US) [100%]"],
            ["2) German (Deutsch) [100%]"]);

        switch (Console.ReadKey().KeyChar)
        {
            case '1':
                RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueLanguage, Constants.RegistryValueTypeString, Constants.LanguageEnglish);
                break;
            case '2':
                RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueLanguage, Constants.RegistryValueTypeString, Constants.LanguageGerman);
                break;
            default:
                return;
        }

        AutoRestart.Init();
    }

    private static void ShowMenuAndText()
    {
        while (true)
        {
            Console.Clear();
            var skipValue = RegistryWorker.ReadFromRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSkipConfirmation);
            var doubleText = Localization.Get(skipValue == "0" ? "DoubleConfirmOn" : "DoubleConfirmOff");

            UIDraw.TextColor = Variables.SecondaryColor;
            UIDraw.DrawBoxedMessage(doubleText);
            UIDraw.TextColor = Variables.MenuColor;

            UIDraw.DrawMenu(Localization.Get("MenuAndText"),
                ["1) " + Localization.Get("DoubleConfiguration")],
                ["9) " + Localization.Get("Back")]);

            switch (Console.ReadKey().KeyChar)
            {
                case '1':
                    var newValue = skipValue == "1" ? "0" : "1";
                    RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSkipConfirmation, Constants.RegistryValueTypeString, newValue);
                    break;
                case '9':
                    Console.Clear();
                    return;
            }
        }
    }

    private static void ShowUpdateSettings()
    {
        while (true)
        {
            Console.Clear();
            var updateSearchValue = RegistryWorker.ReadFromRegistry(Constants.RegistryPathConfig, Constants.RegistryValueEnableUpdateSearch);
            var autoUpdateValue = RegistryWorker.ReadFromRegistry(Constants.RegistryPathConfig, Constants.RegistryValueAutoUpdateOnStart);

            UIDraw.DrawBoxedMessage(Localization.Get(updateSearchValue == "1"
                ? "UpdateSettings_UpdateSearchEnabled"
                : "UpdateSettings_UpdateSearchDisabled"));
            UIDraw.DrawLine("");
            UIDraw.DrawBoxedMessage(Localization.Get(autoUpdateValue == "1"
                ? "UpdateSettings_AutoUpdateEnabled"
                : "UpdateSettings_AutoUpdateDisabled"));
            UIDraw.DrawLine("");

            UIDraw.TextColor = Variables.MenuColor;
            UIDraw.DrawMenu(Localization.Get("UpdateSettings_Title"),
                [
                    "1) " + Localization.Get("UpdateSettings_ToggleUpdateSearch"),
                    "2) " + Localization.Get("UpdateSettings_ToggleAutoUpdate"),
                ],
                ["9) " + Localization.Get("Back")]);

            switch (Console.ReadKey().KeyChar)
            {
                case '1':
                    RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueEnableUpdateSearch, Constants.RegistryValueTypeString,
                        updateSearchValue == "1" ? "0" : "1");
                    break;
                case '2':
                    RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueAutoUpdateOnStart, Constants.RegistryValueTypeString,
                        autoUpdateValue == "1" ? "0" : "1");
                    break;
                case '9':
                    Console.Clear();
                    return;
            }
        }
    }
}
