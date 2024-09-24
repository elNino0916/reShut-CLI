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
            UpdateChecker.DisplayCenteredMessage("╭────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│        Settings        │"); // Has to be changed when more languages are added.
            UpdateChecker.DisplayCenteredMessage("├────────────────────────┤");
            for (var i = 1; i < menuItems.Length - 1; i++)
                UpdateChecker.DisplayCenteredMessage("│ " + i + ") " + menuItems[i - 1].PadRight(20) + "│");
            UpdateChecker.DisplayCenteredMessage("│ 3) " + menuItems[2].PadRight(20) + "│");
            UpdateChecker.DisplayCenteredMessage("├────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 9) " + menuItems[3].PadRight(20) + "│");

            UpdateChecker.DisplayCenteredMessage("╰────────────────────────╯");
            /*
            UpdateChecker.DisplayCenteredMessage("╭────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│            Settings:           │");
            UpdateChecker.DisplayCenteredMessage("├────────────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 1) General                     │");
            UpdateChecker.DisplayCenteredMessage("│ 2) Menus and Messages          │");
            UpdateChecker.DisplayCenteredMessage("│ 3) About reShut CLI...         │");
            UpdateChecker.DisplayCenteredMessage("├────────────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 9) Back                        │");
            UpdateChecker.DisplayCenteredMessage("╰────────────────────────────────╯"); */
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
                    string[] menuItemsGen = [rm.GetString("Update", culture), rm.GetString("Startup", culture), rm.GetString("Theme", culture), rm.GetString("ResetAllSettings", culture), rm.GetString("Language", culture), rm.GetString("Back", culture)];
                    UpdateChecker.DisplayCenteredMessage("╭────────────────────────╮");
                    UpdateChecker.DisplayCenteredMessage("│        General         │"); // Has to be changed when more languages are added.
                    UpdateChecker.DisplayCenteredMessage("├────────────────────────┤");

                    for (var i = 0; i < menuItemsGen.Length - 2; i++) // Iterate until the second-to-last element
                        UpdateChecker.DisplayCenteredMessage("│ " + (i + 1) + ") " + menuItemsGen[i].PadRight(20) + "│");

                    UpdateChecker.DisplayCenteredMessage("│ 5) " + menuItemsGen[4].PadRight(20) + "│");
                    UpdateChecker.DisplayCenteredMessage("├────────────────────────┤");
                    UpdateChecker.DisplayCenteredMessage("│ 9) " + menuItemsGen[5].PadRight(20) + "│");

                    UpdateChecker.DisplayCenteredMessage("╰────────────────────────╯");


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
                            // Get the translated string
                            string invalidText1 = rm.GetString("InvalidInput", culture);

                            // Calculate the maximum length (either the message or the box)
                            int boxWidth1 = Math.Max(invalidText1.Length + 2, 44); // Ensure minimum width of 44
                            string topBorder1 = "╭" + new string('─', boxWidth1) + "╮";
                            string bottomBorder1 = "╰" + new string('─', boxWidth1) + "╯";

                            // Center the message within the box
                            int paddingLeft1 = (boxWidth1 - invalidText1.Length) / 2;
                            string paddedMessage1 = "│" + new string(' ', paddingLeft1) + invalidText1 + new string(' ', boxWidth1 - invalidText1.Length - paddingLeft1) + "│";

                            // Center the entire box within the console window
                            int windowWidth1 = Console.WindowWidth;

                            // Print the confirmation message centered on the console
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

                    // Calculate the maximum length (either the message or the box)
                    int boxWidthStateDouble = Math.Max(doubleText.Length + 2, 44); // Ensure minimum width of 44
                    string topBorderD = "╭" + new string('─', boxWidthStateDouble) + "╮";
                    string bottomBorderD = "╰" + new string('─', boxWidthStateDouble) + "╯";

                    // Center the message within the box
                    int paddingLeftD = (boxWidthStateDouble - doubleText.Length) / 2;
                    string paddedMessageD = "│" + new string(' ', paddingLeftD) + doubleText + new string(' ', boxWidthStateDouble - doubleText.Length - paddingLeftD) + "│";

                    // Print the confirmation message centered on the console
                    Console.ForegroundColor = Variables.SecondaryColor;
                    UpdateChecker.DisplayCenteredMessage(topBorderD);
                    UpdateChecker.DisplayCenteredMessage(paddedMessageD);
                    UpdateChecker.DisplayCenteredMessage(bottomBorderD);
                    Console.ForegroundColor = Variables.MenuColor;

                    string[] menuItemsMsg = { rm.GetString("DoubleConfiguration", culture), rm.GetString("Back", culture) };
                    UpdateChecker.DisplayCenteredMessage("╭────────────────────────────────────────╮");
                    UpdateChecker.DisplayCenteredMessage("│           Menus and Messages           │"); // Has to be changed when more languages are added.
                    UpdateChecker.DisplayCenteredMessage("├────────────────────────────────────────┤");

                    // Display the Double Configuration option
                    UpdateChecker.DisplayCenteredMessage(" │ 1) " + menuItemsMsg[0].PadRight(36) + "│"); // Double Configuration

                    // Add a separating line before the Back option
                    UpdateChecker.DisplayCenteredMessage("├────────────────────────────────────────┤");

                    // Display the Back option
                    UpdateChecker.DisplayCenteredMessage(" │ 9) " + menuItemsMsg[1].PadRight(36) + "│"); // Back

                    UpdateChecker.DisplayCenteredMessage("╰────────────────────────────────────────╯");


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
                    // Get the translated string
                    string invalidText = rm.GetString("InvalidInput", culture);

                    // Calculate the maximum length (either the message or the box)
                    int boxWidth = Math.Max(invalidText.Length + 2, 44); // Ensure minimum width of 44
                    string topBorder = "╭" + new string('─', boxWidth) + "╮";
                    string bottomBorder = "╰" + new string('─', boxWidth) + "╯";

                    // Center the message within the box
                    int paddingLeft = (boxWidth - invalidText.Length) / 2;
                    string paddedMessage = "│" + new string(' ', paddingLeft) + invalidText + new string(' ', boxWidth - invalidText.Length - paddingLeft) + "│";

                    // Print the confirmation message centered on the console
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
