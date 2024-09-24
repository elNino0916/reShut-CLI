using System;

namespace reShutCLI
{
    internal class StartupSettings
    {
        public static void UpdateSettings()
        {
        UpdateSettingsInit:
            Console.Clear();
            string updateSearchValue = RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableUpdateSearch");
            string autoUpdateValue = RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "AutoUpdateOnStart");
            UpdateChecker.DisplayCenteredMessage("╭────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage(updateSearchValue == "1" ? "│ Update Search is enabled.  │" : "│ Update Search is disabled. │");
            UpdateChecker.DisplayCenteredMessage("╰────────────────────────────╯");

            UpdateChecker.DisplayCenteredMessage("╭────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage(autoUpdateValue == "yes" ? "│  Auto Update is enabled.   │" : "│  Auto Update is disabled.  │");
            UpdateChecker.DisplayCenteredMessage("╰────────────────────────────╯");

            // Display current settings
            Console.ForegroundColor = Variables.MenuColor;
            UpdateChecker.DisplayCenteredMessage("╭────────────────────────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│                  Update Settings:                  │");
            UpdateChecker.DisplayCenteredMessage("├────────────────────────────────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 1) Toggle Update Search                            │");
            UpdateChecker.DisplayCenteredMessage("│ 2) Toggle Auto Update on Startup                   │");
            UpdateChecker.DisplayCenteredMessage("├────────────────────────────────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 9) Back                                            │");
            UpdateChecker.DisplayCenteredMessage("╰────────────────────────────────────────────────────╯");
            var keyInfo = Console.ReadKey();
            string key = keyInfo.KeyChar.ToString();
            switch (key)
            {
                case "1":
                    // Toggle Update Search
                    string newUpdateValue = (updateSearchValue == "1") ? "0" : "1";
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableUpdateSearch", "STRING", newUpdateValue);
                    goto UpdateSettingsInit;

                case "2":
                    // Toggle Update Search
                    string newAutoUpdateValue = (autoUpdateValue == "yes") ? "no" : "yes";
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "AutoUpdateOnStart", "STRING", newAutoUpdateValue);
                    goto UpdateSettingsInit;

                case "9":
                    Console.Clear();
                    return;

                default:
                    goto UpdateSettingsInit;
            }
        }
    }
}
