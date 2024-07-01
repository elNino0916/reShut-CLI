using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace reShutCLI
{
    internal class RegInit
    {
        public static void Populate(bool skipCreation) 
        {
            if (!skipCreation)
            {
                if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryPopulated") != "1")
                {
                    // Populate registry with default values
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryVersion", "STRING", Variables.registryVersion);
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "reShutVersion", "STRING", Variables.version);
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryPopulated", "STRING", "1");
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "FirstStartupTime", "STRING", DateTime.Now.ToString("HH:mm:ss (dd.MM.yyyy)"));
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableUpdateSearch", "STRING", "1");
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableFastStartup", "STRING", "0");
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EULAAccepted", "STRING", "0");
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SetupComplete", "STRING", "0");
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableSounds", "STRING", "1");
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme", "STRING", "default");
                }
            }
            if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI","RegistryVersion") != Variables.registryVersion)
            {
                Console.Clear();
                Console.Title = "Invalid Registry Version";
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Red;
                SoundManager.PlayError(true);
                UpdateChecker.DisplayCenteredMessage("Looks like reShutCLI has been updated and the registry version changed.");
                UpdateChecker.DisplayCenteredMessage($"Registry Version: {RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryVersion")} ");
                UpdateChecker.DisplayCenteredMessage($"Your Version: {Variables.registryVersion}");
                UpdateChecker.DisplayCenteredMessage($"");
                UpdateChecker.DisplayCenteredMessage($"To update the registry, reShut CLI needs to reset. The reset will start in 10 seconds.");
                Thread.Sleep(10000);
                RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryPopulated", "STRING", "0");
                RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SetupComplete", "STRING", "0");
                Populate(false);
                Welcome.FirstStartup();
                Console.Clear();
            }
        }
    }
}
