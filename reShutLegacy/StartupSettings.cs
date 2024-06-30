using System;
using System.Drawing;

namespace reShutCLI
{
    internal class StartupSettings
    {
        public static void Show()
        {
        // NEW: Startup Settings
        invalidstartup:
            Console.Clear();

            // Read current settings from registry
            string fastStartupValue = RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableFastStartup");
            string updateSearchValue = RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableUpdateSearch");

            Console.ForegroundColor = Variables.LogoColor;
            UpdateChecker.DisplayCenteredMessage("╭──────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage(fastStartupValue == "1" ? "│ Fast Startup is enabled. │" : " │ Fast Startup is disabled.│");
            UpdateChecker.DisplayCenteredMessage("╰──────────────────────────╯");

            // Display Update Search status
            UpdateChecker.DisplayCenteredMessage("╭────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage(updateSearchValue == "1" ? "│ Update Search is enabled.  │" : "│ Update Search is disabled. │");
            UpdateChecker.DisplayCenteredMessage("╰────────────────────────────╯");
            // Display current settings
            Console.ForegroundColor = Variables.MenuColor;
            UpdateChecker.DisplayCenteredMessage("╭────────────────────────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│                       Startup:                     │");
            UpdateChecker.DisplayCenteredMessage("├────────────────────────────────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 1) Toggle Fast Startup (slows down some features)  │");
            UpdateChecker.DisplayCenteredMessage("│ 2) Toggle Update Search                            │");
            UpdateChecker.DisplayCenteredMessage("├────────────────────────────────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 9) Back                                            │");
            UpdateChecker.DisplayCenteredMessage("╰────────────────────────────────────────────────────╯");

            var keyInfo = Console.ReadKey();
            string key = keyInfo.KeyChar.ToString();

            switch (key)
            {
                case "1":
                    // Toggle Fast Startup
                    string newFastValue = (fastStartupValue == "1") ? "0" : "1";
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableFastStartup", "STRING", newFastValue);
                    goto invalidstartup;

                case "2":
                    // Toggle Update Search
                    string newUpdateValue = (updateSearchValue == "1") ? "0" : "1";
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableUpdateSearch", "STRING", newUpdateValue);
                    goto invalidstartup;

                case "9":
                    Console.Clear();
                    return;

                default:
                    goto invalidstartup;
            }
        }
    }
}
