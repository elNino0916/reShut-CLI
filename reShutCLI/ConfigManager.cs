using System;
using System.Globalization;
using System.Resources;
using System.Runtime.Versioning;
using System.Threading;

namespace reShutCLI
{
    internal class ConfigManager
    {

        public static void Reset()
        {
        ResetMenu:
            Console.Clear();
            CultureInfo culture = new CultureInfo(Variables.lang);
            ResourceManager rm = new ResourceManager("reShutCLI.Resources.Strings", typeof(Program).Assembly);
            Console.ForegroundColor = Variables.SecondaryColor;
            UIDraw.DisplayBoxedMessage(rm.GetString("ResetWarning", culture));
            UpdateChecker.DisplayCenteredMessage("");
            UpdateChecker.DisplayCenteredMessage("╭──────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ 1) Reset         │");
            UpdateChecker.DisplayCenteredMessage("├──────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 2) Cancel        │");
            UpdateChecker.DisplayCenteredMessage("╰──────────────────╯");
            Console.ForegroundColor = Variables.MenuColor;

            var setInfo = Console.ReadKey();
            var confirmation = setInfo.KeyChar.ToString();
            Console.ForegroundColor = Variables.SecondaryColor;

            if (confirmation == "1")
            {
                Console.Clear();
                UIDraw.DisplayBoxedMessage(rm.GetString("ResetProg", culture));
                RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryPopulated", "STRING", "0");
                RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SetupComplete", "STRING", "0");
                Thread.Sleep(500);
                AutoRestart.Init();
                Console.Clear();
                return;
            }
            if (confirmation == "2")
            {
                return;
            }
            goto ResetMenu;
        }
    }
}
