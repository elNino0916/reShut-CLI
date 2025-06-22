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
            string settingsHeaderText = rm.GetString("Settings", culture);

            // Calculate lengths of fully formatted items like "1) General"
            int m1Length = ("1) " + menuItems[0]).Length;
            int m2Length = ("2) " + menuItems[1]).Length;
            int m3Length = ("3) " + menuItems[2]).Length; // About
            int m9Length = ("9) " + menuItems[3]).Length; // Back

            int maxFormattedItemLength = new[] { m1Length, m2Length, m3Length, m9Length }.Max();

            int dynamicInnerWidth = Math.Max(settingsHeaderText.Length, maxFormattedItemLength);
            dynamicInnerWidth += 2; // Add 2 for single space padding on left and right

            if (dynamicInnerWidth % 2 != 0) dynamicInnerWidth++; // Ensure even width for better centering

            string topBorder = "╭" + new string('─', dynamicInnerWidth) + "╮";
            string bottomBorder = "╰" + new string('─', dynamicInnerWidth) + "╯";
            string separator = "├" + new string('─', dynamicInnerWidth) + "┤";

            // Centering the header text
            int headerPaddingTotal = dynamicInnerWidth - settingsHeaderText.Length;
            int headerPaddingLeft = headerPaddingTotal / 2;
            string titleLine = "│" + new string(' ', headerPaddingLeft) + settingsHeaderText + new string(' ', dynamicInnerWidth - settingsHeaderText.Length - headerPaddingLeft) + "│";

            UpdateChecker.DisplayCenteredMessage(topBorder);
            UpdateChecker.DisplayCenteredMessage(titleLine);
            UpdateChecker.DisplayCenteredMessage(separator);

            // Items
            string item1Text = "1) " + menuItems[0];
            UpdateChecker.DisplayCenteredMessage("│" + item1Text.PadRight(dynamicInnerWidth) + "│");
            string item2Text = "2) " + menuItems[1];
            UpdateChecker.DisplayCenteredMessage("│" + item2Text.PadRight(dynamicInnerWidth) + "│");
            string item3Text = "3) " + menuItems[2]; // About
            UpdateChecker.DisplayCenteredMessage("│" + item3Text.PadRight(dynamicInnerWidth) + "│");
            UpdateChecker.DisplayCenteredMessage(separator);
            string item9Text = "9) " + menuItems[3]; // Back
            UpdateChecker.DisplayCenteredMessage("│" + item9Text.PadRight(dynamicInnerWidth) + "│");
            UpdateChecker.DisplayCenteredMessage(bottomBorder);
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
                    string generalHeaderText = rm.GetString("General", culture);

                    int gen_m1Length = ("1) " + menuItemsGen[0]).Length;
                    int gen_m2Length = ("2) " + menuItemsGen[1]).Length;
                    int gen_m3Length = ("3) " + menuItemsGen[2]).Length;
                    int gen_m4Length = ("4) " + menuItemsGen[3]).Length; // Language
                    int gen_m9Length = ("9) " + menuItemsGen[4]).Length; // Back

                    int gen_maxFormattedItemLength = new[] { gen_m1Length, gen_m2Length, gen_m3Length, gen_m4Length, gen_m9Length }.Max();

                    int gen_dynamicInnerWidth = Math.Max(generalHeaderText.Length, gen_maxFormattedItemLength);
                    gen_dynamicInnerWidth += 2; // Add 2 for single space padding

                    if (gen_dynamicInnerWidth % 2 != 0) gen_dynamicInnerWidth++; // Ensure even width

                    string gen_topBorder = "╭" + new string('─', gen_dynamicInnerWidth) + "╮";
                    string gen_bottomBorder = "╰" + new string('─', gen_dynamicInnerWidth) + "╯";
                    string gen_separator = "├" + new string('─', gen_dynamicInnerWidth) + "┤";

                    int gen_headerPaddingTotal = gen_dynamicInnerWidth - generalHeaderText.Length;
                    int gen_headerPaddingLeft = gen_headerPaddingTotal / 2;
                    string gen_titleLine = "│" + new string(' ', gen_headerPaddingLeft) + generalHeaderText + new string(' ', gen_dynamicInnerWidth - generalHeaderText.Length - gen_headerPaddingLeft) + "│";

                    UpdateChecker.DisplayCenteredMessage(gen_topBorder);
                    UpdateChecker.DisplayCenteredMessage(gen_titleLine);
                    UpdateChecker.DisplayCenteredMessage(gen_separator);

                    UpdateChecker.DisplayCenteredMessage("│" + ("1) " + menuItemsGen[0]).PadRight(gen_dynamicInnerWidth) + "│");
                    UpdateChecker.DisplayCenteredMessage("│" + ("2) " + menuItemsGen[1]).PadRight(gen_dynamicInnerWidth) + "│");
                    UpdateChecker.DisplayCenteredMessage("│" + ("3) " + menuItemsGen[2]).PadRight(gen_dynamicInnerWidth) + "│");
                    UpdateChecker.DisplayCenteredMessage("│" + ("4) " + menuItemsGen[3]).PadRight(gen_dynamicInnerWidth) + "│"); // Language
                    UpdateChecker.DisplayCenteredMessage(gen_separator);
                    UpdateChecker.DisplayCenteredMessage("│" + ("9) " + menuItemsGen[4]).PadRight(gen_dynamicInnerWidth) + "│"); // Back

                    UpdateChecker.DisplayCenteredMessage(gen_bottomBorder);

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
                        doubleText = rm.GetString("DoubleConfirmOn", culture);
                    }
                    else 
                    {
                        doubleText = rm.GetString("DoubleConfirmOff", culture); 
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
                    string messagesHeaderText = rm.GetString("MenuAndText", culture);

                    int msg_m1Length = ("1) " + menuItemsMsg[0]).Length;
                    int msg_m9Length = ("9) " + menuItemsMsg[1]).Length;

                    int msg_maxFormattedItemLength = Math.Max(msg_m1Length, msg_m9Length);

                    int msg_dynamicInnerWidth = Math.Max(messagesHeaderText.Length, msg_maxFormattedItemLength);
                    msg_dynamicInnerWidth += 2; // Add 2 for single space padding

                    if (msg_dynamicInnerWidth % 2 != 0) msg_dynamicInnerWidth++; // Ensure even width

                    string msg_topBorder = "╭" + new string('─', msg_dynamicInnerWidth) + "╮";
                    string msg_bottomBorder = "╰" + new string('─', msg_dynamicInnerWidth) + "╯";
                    string msg_separator = "├" + new string('─', msg_dynamicInnerWidth) + "┤";

                    int msg_headerPaddingTotal = msg_dynamicInnerWidth - messagesHeaderText.Length;
                    int msg_headerPaddingLeft = msg_headerPaddingTotal / 2;
                    string msg_titleLine = "│" + new string(' ', msg_headerPaddingLeft) + messagesHeaderText + new string(' ', msg_dynamicInnerWidth - messagesHeaderText.Length - msg_headerPaddingLeft) + "│";

                    UpdateChecker.DisplayCenteredMessage(msg_topBorder);
                    UpdateChecker.DisplayCenteredMessage(msg_titleLine);
                    UpdateChecker.DisplayCenteredMessage(msg_separator);
                    UpdateChecker.DisplayCenteredMessage("│" + ("1) " + menuItemsMsg[0]).PadRight(msg_dynamicInnerWidth) + "│");
                    UpdateChecker.DisplayCenteredMessage(msg_separator);
                    UpdateChecker.DisplayCenteredMessage("│" + ("9) " + menuItemsMsg[1]).PadRight(msg_dynamicInnerWidth) + "│");
                    UpdateChecker.DisplayCenteredMessage(msg_bottomBorder);

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
                    string invalidInput_Text = rm.GetString("InvalidInput", culture);
                    int invalidInput_BoxWidth = Math.Max(invalidInput_Text.Length + 2, 44);
                    string invalidInput_TopBorder = "╭" + new string('─', invalidInput_BoxWidth) + "╮";
                    string invalidInput_BottomBorder = "╰" + new string('─', invalidInput_BoxWidth) + "╯";
                    int invalidInput_PaddingLeft = (invalidInput_BoxWidth - invalidInput_Text.Length) / 2;
                    string invalidInput_PaddedMessage = "│" + new string(' ', invalidInput_PaddingLeft) + invalidInput_Text + new string(' ', invalidInput_BoxWidth - invalidInput_Text.Length - invalidInput_PaddingLeft) + "│";
                    Console.ForegroundColor = Variables.SecondaryColor;
                    UpdateChecker.DisplayCenteredMessage(invalidInput_TopBorder);
                    UpdateChecker.DisplayCenteredMessage(invalidInput_PaddedMessage);
                    UpdateChecker.DisplayCenteredMessage(invalidInput_BottomBorder);
                    Console.ForegroundColor = ConsoleColor.White;
                    goto settings;

            }
        }
    }
}
