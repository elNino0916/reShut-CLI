using System;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text.RegularExpressions;

namespace reShutCLI
{
    internal class Settings
    {
        public static void Show()
        {
            Console.Clear();
        settings:
            // Settings
            CultureInfo culture = new CultureInfo(Variables.lang);
            ResourceManager rm = new ResourceManager("reShutCLI.Resources.Strings", typeof(Program).Assembly);
            // Prints the settings menu
            Console.ForegroundColor = Variables.MenuColor;
            // New UI
            string[] menuItems = [rm.GetString("General", culture), rm.GetString("MenuAndText", culture), rm.GetString("About", culture), rm.GetString("Back", culture)];
            int settingsMenuWidth = menuItems.Max(s => s.Length) + 6; // Add padding for "i) " and " │"
            settingsMenuWidth = Math.Max(settingsMenuWidth, "Settings".Length + 4); // Ensure title fits

            string settingsTopBorder = "╭" + new string('─', settingsMenuWidth) + "╮";
            string settingsTitle = "│ " + "Settings".PadRight(settingsMenuWidth - 2) + " │";
            string settingsSeparator = "├" + new string('─', settingsMenuWidth) + "┤";
            string settingsBottomBorder = "╰" + new string('─', settingsMenuWidth) + "╯";

            UpdateChecker.DisplayCenteredMessage(settingsTopBorder);
            UpdateChecker.DisplayCenteredMessage(settingsTitle);
            UpdateChecker.DisplayCenteredMessage(settingsSeparator);
            for (var i = 0; i < menuItems.Length - 2; i++) // Loop for General, MenuAndText
                UpdateChecker.DisplayCenteredMessage("│ " + (i + 1) + ") " + menuItems[i].PadRight(settingsMenuWidth - 6) + " │");
            UpdateChecker.DisplayCenteredMessage("│ 3) " + menuItems[2].PadRight(settingsMenuWidth - 6) + " │"); // About
            UpdateChecker.DisplayCenteredMessage(settingsSeparator);
            UpdateChecker.DisplayCenteredMessage("│ 9) " + menuItems[3].PadRight(settingsMenuWidth - 6) + " │"); // Back

            UpdateChecker.DisplayCenteredMessage(settingsBottomBorder);
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
                    string[] menuItemsGen = [rm.GetString("Update", culture), rm.GetString("Theme", culture), rm.GetString("ResetAllSettings", culture), rm.GetString("Language", culture), rm.GetString("Back", culture)];
                    int generalMenuWidth = menuItemsGen.Max(s => s.Length) + 6; // Add padding for "i) " and " │"
                    generalMenuWidth = Math.Max(generalMenuWidth, rm.GetString("General", culture).Length + 4); // Ensure title fits

                    string generalTopBorder = "╭" + new string('─', generalMenuWidth) + "╮";
                    string generalTitle = "│ " + rm.GetString("General", culture).PadRight(generalMenuWidth - 2) + " │";
                    string generalSeparator = "├" + new string('─', generalMenuWidth) + "┤";
                    string generalBottomBorder = "╰" + new string('─', generalMenuWidth) + "╯";

                    UpdateChecker.DisplayCenteredMessage(generalTopBorder);
                    UpdateChecker.DisplayCenteredMessage(generalTitle);
                    UpdateChecker.DisplayCenteredMessage(generalSeparator);

                    for (var i = 0; i < menuItemsGen.Length - 2; i++) // Update, Theme, ResetAllSettings
                        UpdateChecker.DisplayCenteredMessage("│ " + (i + 1) + ") " + menuItemsGen[i].PadRight(generalMenuWidth - 6) + " │");

                    UpdateChecker.DisplayCenteredMessage("│ 4) " + menuItemsGen[3].PadRight(generalMenuWidth - 6) + " │"); // Language
                    UpdateChecker.DisplayCenteredMessage(generalSeparator);
                    UpdateChecker.DisplayCenteredMessage("│ 9) " + menuItemsGen[4].PadRight(generalMenuWidth - 6) + " │"); // Back

                    UpdateChecker.DisplayCenteredMessage(generalBottomBorder);

                    /*
                    UpdateChecker.DisplayCenteredMessage("╭───────────────────────────────╮");
                    UpdateChecker.DisplayCenteredMessage("│            General            │");
                    UpdateChecker.DisplayCenteredMessage("├───────────────────────────────┤");
                    UpdateChecker.DisplayCenteredMessage("│ 1) Update                     │");
                    UpdateChecker.DisplayCenteredMessage("│ 2) Startup                    │");
                    UpdateChecker.DisplayCenteredMessage("│ 3) Theme                      │");
                    UpdateChecker.DisplayCenteredMessage("│ 4) Reset all settings         │");
                    UpdateChecker.DisplayCenteredMessage("│ 5) Language                   │");
                    UpdateChecker.DisplayCenteredMessage("├───────────────────────────────┤");
                    UpdateChecker.DisplayCenteredMessage("│ 9) Back                       │");
                    UpdateChecker.DisplayCenteredMessage("╰───────────────────────────────╯");
                    */

                    var setInfoGen = Console.ReadKey();
                    var setGen = setInfoGen.KeyChar.ToString();
                    switch (setGen)
                    {
                        case "1":
                            StartupSettings.UpdateSettings();
                            goto GeneralInit;
                        case "2":
                            Console.Clear();
                            Console.ForegroundColor = Variables.MenuColor;
                            ThemeSettings.OpenSettings();
                            goto GeneralInit;

                        case "3":
                            ConfigManager.Reset();
                            goto GeneralInit;

                        case "4":
                            Console.Clear();
                            Console.ForegroundColor = Variables.MenuColor;
                            UpdateChecker.DisplayCenteredMessage("╭──────────╮");
                            UpdateChecker.DisplayCenteredMessage("│ Language │");
                            UpdateChecker.DisplayCenteredMessage("╰──────────╯");
                            UpdateChecker.DisplayCenteredMessage("");
                            UpdateChecker.DisplayCenteredMessage("╭──────────────────────────────────────────────────────────────────────────────────────╮");
                            UpdateChecker.DisplayCenteredMessage("│ Help translate reShut CLI to your language! https://github.com/elnino0916/reshut-cli │");
                            UpdateChecker.DisplayCenteredMessage("╰──────────────────────────────────────────────────────────────────────────────────────╯");
                            UpdateChecker.DisplayCenteredMessage("");
                            UpdateChecker.DisplayCenteredMessage("╭────────────────────────────────────────────╮");
                            UpdateChecker.DisplayCenteredMessage("│ Select the language you would like to use. │");
                            UpdateChecker.DisplayCenteredMessage("╰────────────────────────────────────────────╯");
                            UpdateChecker.DisplayCenteredMessage("");
                            UpdateChecker.DisplayCenteredMessage("╭──────────────────────────────────╮");
                            UpdateChecker.DisplayCenteredMessage("│ 1) English (US) [100%]           │");
                            UpdateChecker.DisplayCenteredMessage("├──────────────────────────────────┤");
                            UpdateChecker.DisplayCenteredMessage("│ 2) German (Deutsch) [80%]        │");
                            UpdateChecker.DisplayCenteredMessage("├──────────────────────────────────┤");
                            UpdateChecker.DisplayCenteredMessage("│ 3) French (Français) [50%]       │");
                            UpdateChecker.DisplayCenteredMessage("├──────────────────────────────────┤");
                            UpdateChecker.DisplayCenteredMessage("│ 4) Spanish (Español) [50%]       │");
                            UpdateChecker.DisplayCenteredMessage("├──────────────────────────────────┤");
                            UpdateChecker.DisplayCenteredMessage("│ 5) Portuguese (Português) [50%]  │");
                            UpdateChecker.DisplayCenteredMessage("╰──────────────────────────────────╯");
                            var keyInfoL = Console.ReadKey();
                            var key = keyInfoL.KeyChar.ToString();
                            switch (key)
                            {
                                case "1":
                                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "Language", "STRING", "en-US");
                                    break;

                                case "2":
                                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "Language", "STRING", "de-DE");
                                    break;

                                case "3":
                                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "Language", "STRING", "fr-FR");
                                    break;

                                case "4":
                                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "Language", "STRING", "es-ES");
                                    break;

                                case "5":
                                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "Language", "STRING", "pt-PT");
                                    break;
                                default:
                                    break;
                            }
                            AutoRestart.Init();
                            goto GeneralInit;

                        case "9":
                            Console.Clear();
                            goto settings;
                        default:
                            Console.Clear();
                            string invalidText1 = rm.GetString("InvalidInput", culture);
                            int boxWidth1 = Math.Max(invalidText1.Length + 2, 44); 
                            string topBorder1 = "╭" + new string('─', boxWidth1) + "╮";
                            string bottomBorder1 = "╰" + new string('─', boxWidth1) + "╯";
                            int paddingLeft1 = (boxWidth1 - invalidText1.Length) / 2;
                            string paddedMessage1 = "│" + new string(' ', paddingLeft1) + invalidText1 + new string(' ', boxWidth1 - invalidText1.Length - paddingLeft1) + "│";
                            int windowWidth1 = Console.WindowWidth;
                            Console.ForegroundColor = Variables.SecondaryColor;
                            UpdateChecker.DisplayCenteredMessage(topBorder1);
                            UpdateChecker.DisplayCenteredMessage(paddedMessage1);
                            UpdateChecker.DisplayCenteredMessage(bottomBorder1);
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
                    string doubleText = "DoubleConfirmoff";
                    if (skipValue == "0")
                    {
                        doubleText = rm.GetString("DoubleConfirmOff", culture);
                    }
                    else 
                    {
                        doubleText = rm.GetString("DoubleConfirmOn", culture); 
                    }

                    int boxWidthStateDouble = Math.Max(doubleText.Length + 2, 44);
                    string topBorderD = "╭" + new string('─', boxWidthStateDouble) + "╮";
                    string bottomBorderD = "╰" + new string('─', boxWidthStateDouble) + "╯";
                    int paddingLeftD = (boxWidthStateDouble - doubleText.Length) / 2;
                    string paddedMessageD = "│" + new string(' ', paddingLeftD) + doubleText + new string(' ', boxWidthStateDouble - doubleText.Length - paddingLeftD) + "│";
                    Console.ForegroundColor = Variables.SecondaryColor;
                    UpdateChecker.DisplayCenteredMessage(topBorderD);
                    UpdateChecker.DisplayCenteredMessage(paddedMessageD);
                    UpdateChecker.DisplayCenteredMessage(bottomBorderD);
                    Console.ForegroundColor = Variables.MenuColor;

                    string[] menuItemsMsg = { rm.GetString("DoubleConfiguration", culture), rm.GetString("Back", culture) };
                    int messagesMenuWidth = menuItemsMsg.Max(s => s.Length) + 7; // Add padding for " i) " and "│"
                    messagesMenuWidth = Math.Max(messagesMenuWidth, rm.GetString("MenuAndText", culture).Length + 4); // Ensure title fits

                    string messagesTopBorder = "╭" + new string('─', messagesMenuWidth) + "╮";
                    string messagesTitle = "│ " + rm.GetString("MenuAndText", culture).PadRight(messagesMenuWidth - 2) + " │";
                    string messagesSeparator = "├" + new string('─', messagesMenuWidth) + "┤";
                    string messagesBottomBorder = "╰" + new string('─', messagesMenuWidth) + "╯";

                    UpdateChecker.DisplayCenteredMessage(messagesTopBorder);
                    UpdateChecker.DisplayCenteredMessage(messagesTitle);
                    UpdateChecker.DisplayCenteredMessage(messagesSeparator);
                    UpdateChecker.DisplayCenteredMessage(" │ 1) " + menuItemsMsg[0].PadRight(messagesMenuWidth - 7) + "│");
                    UpdateChecker.DisplayCenteredMessage(messagesSeparator);
                    UpdateChecker.DisplayCenteredMessage(" │ 9) " + menuItemsMsg[1].PadRight(messagesMenuWidth - 7) + "│");
                    UpdateChecker.DisplayCenteredMessage(messagesBottomBorder);

                    /*
                    UpdateChecker.DisplayCenteredMessage("╭───────────────────────────────╮");
                    UpdateChecker.DisplayCenteredMessage("│       Menus and Messages      │");
                    UpdateChecker.DisplayCenteredMessage("├───────────────────────────────┤");
                    UpdateChecker.DisplayCenteredMessage("│ 1) Toggle Double-Confirmation │");
                    UpdateChecker.DisplayCenteredMessage("├───────────────────────────────┤");
                    UpdateChecker.DisplayCenteredMessage("│ 9) Back                       │");
                    UpdateChecker.DisplayCenteredMessage("╰───────────────────────────────╯");
                    */
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
                            Console.Clear();
                            goto settings;

                        default:
                            goto MenuAndTextInit;
                    }
                case "3":

                    AboutPage.Show();
                    goto settings;

                // Go back when invalid key has been pressed
                default:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    string invalidText = rm.GetString("InvalidInput", culture);
                    int boxWidth = Math.Max(invalidText.Length + 2, 44);
                    string topBorder = "╭" + new string('─', boxWidth) + "╮";
                    string bottomBorder = "╰" + new string('─', boxWidth) + "╯";
                    int paddingLeft = (boxWidth - invalidText.Length) / 2;
                    string paddedMessage = "│" + new string(' ', paddingLeft) + invalidText + new string(' ', boxWidth - invalidText.Length - paddingLeft) + "│";
                    Console.ForegroundColor = Variables.SecondaryColor;
                    UpdateChecker.DisplayCenteredMessage(topBorder);
                    UpdateChecker.DisplayCenteredMessage(paddedMessage);
                    UpdateChecker.DisplayCenteredMessage(bottomBorder);
                    Console.ForegroundColor = ConsoleColor.White;
                    goto settings;

            }
        }
    }
}
