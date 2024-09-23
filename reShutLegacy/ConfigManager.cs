using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace reShutCLI
{
    internal class ConfigManager
    {
        public static void Reset()
        {
            Console.Clear();
        Retry:
            Console.ForegroundColor = Variables.SecondaryColor;
            UpdateChecker.DisplayCenteredMessage("╭─────────────────────────────────────────────────────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ Do you really want to reset reShut CLI? Enter RESET and press Enter to confirm. │");
            UpdateChecker.DisplayCenteredMessage("╰─────────────────────────────────────────────────────────────────────────────────╯");
            UpdateChecker.DisplayCenteredMessage("╭──────────────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ To cancel, type nothing and press Enter. │");
            UpdateChecker.DisplayCenteredMessage("╰──────────────────────────────────────────╯");
            Console.ForegroundColor = Variables.MenuColor;
            Console.Write(">>");
            string confirmation = Console.ReadLine();
            Console.ForegroundColor = Variables.SecondaryColor;
            if (confirmation == "")
            {
                return;
            }
            if (confirmation == "nothing") // heh
            {
                return;
            }
            if (confirmation != "RESET")
            {
                Console.Clear();
                UpdateChecker.DisplayCenteredMessage("╭─────────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ You have entered an invalid string. │");
                UpdateChecker.DisplayCenteredMessage("╰─────────────────────────────────────╯");
                goto Retry;

            }
            Console.Clear();
            UpdateChecker.DisplayCenteredMessage("╭─────────────────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ Reset initiated. Do not close reShut CLI... │");
            UpdateChecker.DisplayCenteredMessage("╰─────────────────────────────────────────────╯");
            Thread.Sleep(3000);
            RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryPopulated", "STRING", "0");
            RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SetupComplete", "STRING", "0");
            AutoRestart.Init();
            Console.Clear();
        }
    }
}
