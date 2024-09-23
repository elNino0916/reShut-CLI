using System;

namespace reShutCLI
{
    internal class Settings
    {
        public static void Show()
        {
            Console.Clear();
        settings:
            // Settings

            // Prints the settings menu
            Console.ForegroundColor = Variables.MenuColor;
            // New UI
            UpdateChecker.DisplayCenteredMessage("╭────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│            Settings:           │");
            UpdateChecker.DisplayCenteredMessage("├────────────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 1) General                     │");
            UpdateChecker.DisplayCenteredMessage("│ 2) Menus and Messages          │");
            UpdateChecker.DisplayCenteredMessage("│ 3) About reShut CLI...         │");
            UpdateChecker.DisplayCenteredMessage("├────────────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 9) Back                        │");
            UpdateChecker.DisplayCenteredMessage("╰────────────────────────────────╯");
            Console.ForegroundColor = ConsoleColor.White;
            // Key detect
            var setInfo = Console.ReadKey();
            var set = setInfo.KeyChar.ToString();

            switch (set)
            {
                case "1":
                // General
                GeneralInit:
                    Console.Clear();
                    Console.ForegroundColor = Variables.MenuColor;
                    UpdateChecker.DisplayCenteredMessage("╭───────────────────────────────╮");
                    UpdateChecker.DisplayCenteredMessage("│            General            │");
                    UpdateChecker.DisplayCenteredMessage("├───────────────────────────────┤");
                    UpdateChecker.DisplayCenteredMessage("│ 1) Update                     │");
                    UpdateChecker.DisplayCenteredMessage("│ 2) Startup                    │");
                    UpdateChecker.DisplayCenteredMessage("│ 3) Theme                      │");
                    UpdateChecker.DisplayCenteredMessage("│ 4) Reset all settings         │");
                    UpdateChecker.DisplayCenteredMessage("│ 5) ??? (Available in v.2.0.0) │"); // you wont find anything in the code... (yet)
                    UpdateChecker.DisplayCenteredMessage("├───────────────────────────────┤");
                    UpdateChecker.DisplayCenteredMessage("│ 9) Back                       │");
                    UpdateChecker.DisplayCenteredMessage("╰───────────────────────────────╯");
                    var setInfoGen = Console.ReadKey();
                    var setGen = setInfoGen.KeyChar.ToString();
                    switch (setGen)
                    {
                        case "1":
                            StartupSettings.UpdateSettings();
                            goto GeneralInit;

                        case "2":
                            StartupSettings.Show();
                            goto GeneralInit;

                        case "3":
                            Console.Clear();
                            Console.ForegroundColor = Variables.MenuColor;
                            ThemeSettings.OpenSettings();
                            goto GeneralInit;

                        case "4":
                            ConfigManager.Reset();
                            goto GeneralInit;

                        case "9":
                            Console.Clear();
                            goto settings;
                        default:
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            UpdateChecker.DisplayCenteredMessage("╭────────────────╮");
                            UpdateChecker.DisplayCenteredMessage("│ Invalid input. │");
                            UpdateChecker.DisplayCenteredMessage("╰────────────────╯");
                            Console.ForegroundColor = ConsoleColor.White;
                            goto GeneralInit;
                    }
                case "9":
                    Console.Clear();
                    return;
                case "2":
                MenuAndTextInit:
                    Console.Clear();
                    string skipValue = RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SkipConfirmation");
                    Console.ForegroundColor = Variables.LogoColor;
                    UpdateChecker.DisplayCenteredMessage("╭─────────────────────────────────╮");
                    UpdateChecker.DisplayCenteredMessage(skipValue == "1" ? "│ Double-Confirmation is enabled. │" : "│ Double-Confirmation is disabled.│");
                    UpdateChecker.DisplayCenteredMessage("╰─────────────────────────────────╯");
                    Console.ForegroundColor = Variables.MenuColor;
                    UpdateChecker.DisplayCenteredMessage("╭───────────────────────────────╮");
                    UpdateChecker.DisplayCenteredMessage("│       Menus and Messages      │");
                    UpdateChecker.DisplayCenteredMessage("├───────────────────────────────┤");
                    UpdateChecker.DisplayCenteredMessage("│ 1) Toggle Double-Confirmation │");
                    UpdateChecker.DisplayCenteredMessage("├───────────────────────────────┤");
                    UpdateChecker.DisplayCenteredMessage("│ 9) Back                       │");
                    UpdateChecker.DisplayCenteredMessage("╰───────────────────────────────╯");
                    var setInfoMenu = Console.ReadKey();
                    var setGenMenu = setInfoMenu.KeyChar.ToString();
                    switch (setGenMenu)
                    {
                        case "1":
                            if (skipValue == "1")
                            {
                                RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SkipConfirmation", "STRING", "0");
                            }
                            else
                            {
                                RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SkipConfirmation", "STRING", "1");
                            }
                            goto MenuAndTextInit;

                        case "9":
                            goto GeneralInit;

                        default:
                            goto MenuAndTextInit;
                    }
                    goto GeneralInit;
                case "3":

                    AboutPage.Show();
                    goto settings;

                // Go back when invalid key has been pressed
                default:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    UpdateChecker.DisplayCenteredMessage("╭────────────────╮");
                    UpdateChecker.DisplayCenteredMessage("│ Invalid input. │");
                    UpdateChecker.DisplayCenteredMessage("╰────────────────╯");
                    Console.ForegroundColor = ConsoleColor.White;
                    goto settings;

            }
        }
    }
}
