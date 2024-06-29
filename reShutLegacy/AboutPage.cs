using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reShutCLI
{
    internal class AboutPage
    {
        public static void Show()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            UpdateChecker.DisplayCenteredMessage("╭────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ Please wait... │");
            UpdateChecker.DisplayCenteredMessage("╰────────────────╯");
            Console.ForegroundColor = Variables.MenuColor;
            var header = "reShut CLI " + Variables.fullversion;
            var releaseStatus = "Pre-Release: " + Variables.prerelease;
            const string systemInfoHeader = "System Information:";
            var cpuInfo = "CPU: " + Preload.GetString(1);
            var gpuInfo = "Primary Video Controller: " + Preload.GetString(3);
            var ramInfo = "RAM: " + Preload.GetString(2) + " GB";
            var winVer = "Windows Version: " + Preload.GetString(0);


            Console.Clear();
            var maxLength = Math.Max(header.Length,
                Math.Max(systemInfoHeader.Length,
                    Math.Max(cpuInfo.Length,
                        Math.Max(gpuInfo.Length,
                            Math.Max(ramInfo.Length,
                                Math.Max(
                                    "The license can be viewed by pressing L in the main menu.".Length,
                                    winVer.Length))))));
            var borderLength = maxLength + 4;

            UpdateChecker.DisplayCenteredMessage("╭" + new string('─', borderLength) + "╮");
            UpdateChecker.DisplayCenteredMessage("│ " + header.PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("│ " + "Copyright (c) 2024 elNino0916".PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("│ " +
                              "The license can be viewed by pressing L in the main menu.".PadRight(
                                  borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("│ " + releaseStatus.PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("├" + new string('─', borderLength) + "┤");
            UpdateChecker.DisplayCenteredMessage("│ " + systemInfoHeader.PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("│ " + cpuInfo.PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("│ " + gpuInfo.PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("│ " + ramInfo.PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("│ " + winVer.PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("╰" + new string('─', borderLength) + "╯");
            UpdateChecker.DisplayCenteredMessage("Press any key to go back.");
            Console.ReadKey();
            Console.Clear();
            return;
        }
    }
}
