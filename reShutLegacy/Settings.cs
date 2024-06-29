using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            UpdateChecker.DisplayCenteredMessage("╭───────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│     Settings:     │");
            UpdateChecker.DisplayCenteredMessage("├───────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 1) About...       │");
            UpdateChecker.DisplayCenteredMessage("│ 2) Startup        │");
            UpdateChecker.DisplayCenteredMessage("│ 9) Back           │");
            UpdateChecker.DisplayCenteredMessage("╰───────────────────╯");
            Console.ForegroundColor = ConsoleColor.White;
            // Key detect
            var setInfo = Console.ReadKey();
            var set = setInfo.KeyChar.ToString();

            switch (set)
            {
                // Go back
                // About screen
                case "9":
                    Console.Clear();
                    return;
                // What's new screen
                case "1":
                    
                        AboutPage.Show();
                        goto settings;
                    
                // Go back when invalid key has been pressed
                case "2":
                    
                        StartupSettings.Show();
                        goto settings;

                default:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    UpdateChecker.DisplayCenteredMessage("╭───────────────────╮");
                    UpdateChecker.DisplayCenteredMessage("│   Invalid input.  │");
                    UpdateChecker.DisplayCenteredMessage("╰───────────────────╯");
                    Console.ForegroundColor = ConsoleColor.White;
                    goto settings;

            }
        }
    }
}
