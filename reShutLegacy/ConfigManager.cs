using System;
using System.Globalization;
using System.Resources;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using System.Threading;

namespace reShutCLI
{
    internal class ConfigManager
    {
        [SupportedOSPlatform("windows")]
        public static void Reset()
        {
            Console.Clear();
            CultureInfo culture = new CultureInfo(Variables.lang);
            ResourceManager rm = new ResourceManager("reShutCLI.Resources.Strings", typeof(Program).Assembly);
            Console.ForegroundColor = Variables.SecondaryColor;
            UpdateChecker.DisplayCenteredMessage("╭─────────────────────────────────────────────────────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ Do you really want to reset reShut CLI? Enter RESET and press Enter to confirm. │");
            UpdateChecker.DisplayCenteredMessage("╰─────────────────────────────────────────────────────────────────────────────────╯");
            UpdateChecker.DisplayCenteredMessage("╭────────────────────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ To cancel, type anything else and press Enter. │");
            UpdateChecker.DisplayCenteredMessage("╰────────────────────────────────────────────────╯");
            Console.ForegroundColor = Variables.MenuColor;
            Console.Write(">>");
            string confirmation = Console.ReadLine();
            Console.ForegroundColor = Variables.SecondaryColor;

            if (confirmation != "RESET")
            {
                return;
            }
            Console.Clear();
            UpdateChecker.DisplayCenteredMessage("╭───────────────────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ Reset in progress. Do not close reShut CLI... │");
            UpdateChecker.DisplayCenteredMessage("╰───────────────────────────────────────────────╯");
            Thread.Sleep(3000);
            RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryPopulated", "STRING", "0");
            RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SetupComplete", "STRING", "0");
            AutoRestart.Init();
            Console.Clear();
        }
    }
}
