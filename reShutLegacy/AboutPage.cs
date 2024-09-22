using System;

namespace reShutCLI
{
    internal class AboutPage
    {
        public static void Show()
        {
            Console.ForegroundColor = Variables.MenuColor;
            Console.Clear();
            UpdateChecker.DisplayCenteredMessage("╭────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ Please wait... │");
            UpdateChecker.DisplayCenteredMessage("╰────────────────╯");
            Console.ForegroundColor = Variables.MenuColor;

            var header = "reShut CLI " + Variables.fullversion;
            var releaseStatus = "Pre-Release: " + Variables.prerelease;


            Console.Clear();
            var maxLength = Math.Max(header.Length,
                                Math.Max(
                                    "The license can be viewed by pressing L in the main menu.".Length,header.Length));
            var borderLength = maxLength + 4;

            UpdateChecker.DisplayCenteredMessage("╭" + new string('─', borderLength) + "╮");
            UpdateChecker.DisplayCenteredMessage("│ " + header.PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("│ " + "Copyright (c) 2023-2024 elNino0916".PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("│ " + releaseStatus.PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("╰" + new string('─', borderLength) + "╯");
            UpdateChecker.DisplayCenteredMessage("Press any key to go back.");
            Console.ReadKey();
            Console.Clear();
            return;
        }
    }
}
