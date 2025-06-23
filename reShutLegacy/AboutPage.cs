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

            var prereleasetag = Variables.prerelease ? rm.GetString("PreRelease", culture) : "";
            var header = "reShut CLI " + prereleasetag + Variables.version;
            var releaseStatus = $"{rm.GetString("PreRelease", culture)}: {(Variables.prerelease ? rm.GetString("Yes", culture) : rm.GetString("No", culture))}";
            var registryVersion = $"{rm.GetString("RegistryVersion", culture)}: {Variables.registryVersion}";

            Console.Clear();
            var maxLength = Math.Max(header.Length, Math.Max(releaseStatus.Length, registryVersion.Length));
            var borderLength = maxLength + 4;

            UpdateChecker.DisplayCenteredMessage("╭" + new string('─', borderLength) + "╮");
            UpdateChecker.DisplayCenteredMessage("│ " + header.PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("│ " + "Copyright © 2023-2025 elNino0916".PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("│ " + releaseStatus.PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("│ " + registryVersion.PadRight(borderLength - 2) + " │");
            UpdateChecker.DisplayCenteredMessage("╰" + new string('─', borderLength) + "╯");
            UIDraw.DisplayBoxedMessage(rm.GetString("PressAnyKeyToGoBack", culture));
            
            Console.ReadKey();
            Console.Clear();
            return;
        }
    }
}
