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
            Console.WriteLine("╭────────────────╮");
            Console.WriteLine("│ Please wait... │");
            Console.WriteLine("╰────────────────╯");
            Console.ForegroundColor = Variables.MenuColor;
            var header = "reShut CLI " + Variables.fullversion;
            var releaseStatus = "Pre-Release: " + Variables.prerelease;
            const string systemInfoHeader = "System Information:";
            var cpuInfo = "CPU: " + Preload.GetString(1);
            var gpuInfo = "Video Controller: " + Preload.GetString(3);
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

            Console.WriteLine("╭" + new string('─', borderLength) + "╮");
            Console.WriteLine("│ " + header.PadRight(borderLength - 2) + " │");
            Console.WriteLine("│ " + "Copyright (c) 2024 elNino0916".PadRight(borderLength - 2) + " │");
            Console.WriteLine("│ " +
                              "The license can be viewed by pressing L in the main menu.".PadRight(
                                  borderLength - 2) + " │");
            Console.WriteLine("│ " + releaseStatus.PadRight(borderLength - 2) + " │");
            Console.WriteLine("├" + new string('─', borderLength) + "┤");
            Console.WriteLine("│ " + systemInfoHeader.PadRight(borderLength - 2) + " │");
            Console.WriteLine("│ " + cpuInfo.PadRight(borderLength - 2) + " │");
            Console.WriteLine("│ " + gpuInfo.PadRight(borderLength - 2) + " │");
            Console.WriteLine("│ " + ramInfo.PadRight(borderLength - 2) + " │");
            Console.WriteLine("│ " + winVer.PadRight(borderLength - 2) + " │");
            Console.WriteLine("╰" + new string('─', borderLength) + "╯");
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();
            Console.Clear();
            return;
        }
    }
}
