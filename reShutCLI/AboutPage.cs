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
            UIDraw.TextColor = Variables.MenuColor;

            var prereleasetag = Variables.prerelease ? rm.GetString("PreRelease", culture) : "";
            var header = "reShut CLI " + prereleasetag + Variables.version;
            var releaseStatus = $"{rm.GetString("PreRelease", culture)}: {(Variables.prerelease ? rm.GetString("Yes", culture) : rm.GetString("No", culture))}";
            var registryVersion = $"{rm.GetString("RegistryVersion", culture)}: v{Variables.registryVersion}";

            Console.Clear();
            var maxLength = Math.Max(header.Length, Math.Max(releaseStatus.Length, registryVersion.Length));
            var borderLength = maxLength + 4;
            UIDraw.TextColor = ConsoleColor.DarkYellow;

            Console.Write("\u001b[38;2;255;154;2m");
            UIDraw.DrawCenteredLine("╔═══════════╗");
            UIDraw.DrawCenteredLine("║ ╭───────╮ ║");
            UIDraw.DrawCenteredLine("║ │ > _   │ ║");
            UIDraw.DrawCenteredLine("║ │       │ ║");
            UIDraw.DrawCenteredLine("║ ╰───────╯ ║");
            UIDraw.DrawCenteredLine("╚═══════════╝");
            UIDraw.DrawCenteredLine("reShut CLI v2");
            Console.Write("\u001b[0m");
            UIDraw.TextColor = Variables.MenuColor;
            UIDraw.DrawCenteredLine("");
            UIDraw.DrawCenteredLine("╭" + new string('─', borderLength) + "╮");
            UIDraw.DrawCenteredLine("│ " + header.PadRight(borderLength - 2) + " │");
            UIDraw.DrawCenteredLine("│ " + releaseStatus.PadRight(borderLength - 2) + " │");
            UIDraw.DrawCenteredLine("│ " + registryVersion.PadRight(borderLength - 2) + " │");
            UIDraw.DrawCenteredLine("│ " + "© 2023-2025 elNino0916".PadRight(borderLength - 2) + " │");
            UIDraw.DrawCenteredLine("│ " + "https://elNino0916.de".PadRight(borderLength - 2) + " │");
            UIDraw.DrawCenteredLine("╰" + new string('─', borderLength) + "╯");
            UIDraw.DrawBoxedMessage(rm.GetString("PressAnyKeyToGoBack", culture));
            
            Console.ReadKey();
            Console.Clear();
            return;
        }
    }
}
