using System;
using System.Runtime.Versioning;
using System.Threading;

namespace reShutCLI
{
    internal class RegInit
    {
        [SupportedOSPlatform("windows")]
        public static void Populate(bool skipCreation)
        {
            if (!skipCreation)
            {
                if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryPopulated") != "1")
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryVersion", "STRING", Variables.registryVersion);
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "reShutVersion", "STRING", Variables.version);
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryPopulated", "STRING", "1");
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "FirstStartupTime", "STRING", DateTime.Now.ToString("HH:mm:ss (dd.MM.yyyy)"));

                    // Settings
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableUpdateSearch", "STRING", "1");
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EULAAccepted", "STRING", "0");
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SetupComplete", "STRING", "0");
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme", "STRING", "default");
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SkipConfirmation", "STRING", "0");
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "Language", "STRING", "en-US");
                }
            }

            // Delete no longer required keys.
            RegistryWorker.DeleteFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableFastStartup"); // Removed in 2.0.0.0
            RegistryWorker.DeleteFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableSounds"); // Removed in 1.0.4.0

            if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryVersion") != Variables.registryVersion)
            {
                Console.Clear();
                Console.Title = "Invalid Registry Version";
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Red;
                UpdateChecker.DisplayCenteredMessage("Looks like reShutCLI has been updated and the registry version changed.");
                UpdateChecker.DisplayCenteredMessage($"Registry Version: {RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryVersion")} ");
                UpdateChecker.DisplayCenteredMessage($"Your Version: {Variables.registryVersion}");
                UpdateChecker.DisplayCenteredMessage($"");
                UpdateChecker.DisplayCenteredMessage($"To update the registry, reShut CLI needs to reset. The reset will start in 10 seconds.");
                Thread.Sleep(10000);
                RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryPopulated", "STRING", "0");
                RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SetupComplete", "STRING", "0");
                Populate(false);
                FirstTimeSetup.FirstStartup();
                Console.Clear();
            }
        }
    }

}
