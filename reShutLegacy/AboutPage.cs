using System;
using System.Globalization;
using System.Resources;

namespace reShutCLI
{
    internal class AboutPage
    {
        public static void Show()
        {
            CultureInfo culture = new CultureInfo(Variables.lang);
            ResourceManager rm = new ResourceManager("reShutCLI.Resources.Strings", typeof(Program).Assembly);
            Console.ForegroundColor = Variables.MenuColor;

            var prereleasetag = "E";
            if (Variables.prerelease == true)
            {
                prereleasetag = "Pre-Release ";
            }
            else
            {
                prereleasetag = "Release ";
            }
            var header = "reShut CLI "+ prereleasetag + Variables.version;

            var releaseStatus = "Pre-Release: " + Variables.prerelease;

            Console.Clear();
            var maxLength = Math.Max(header.Length,
                                Math.Max(
                                    "The license can be viewed by pressing L in the main menu.".Length, header.Length));
            var borderLength = maxLength + 4;

            UpdateChecker.DisplayCenteredMessage("╭" + new string('─', borderLength) + "╮");
            UpdateChecker.DisplayCenteredMessage("│ " + header.PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("│ " + "Copyright © 2023-2025 elNino0916".PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("╰" + new string('─', borderLength) + "╯");
            UpdateChecker.DisplayCenteredMessage(rm.GetString("PressAnyKeyToGoBack", culture));
            Console.ReadKey();
            Console.Clear();
            return;
        }
    }
}
