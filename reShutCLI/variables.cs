using System;
using System.Globalization;
using System.Management;
using System.Resources;
using System.Runtime.Versioning;

namespace reShutCLI
{
    internal class Variables
    {

        // Set true if this is a pre-release
        public static bool prerelease = false;
        public static bool DevelopmentBuild = false;

        // Language
        public static string lang = RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config","Language");

        // The version
        public static string version = "2.0.4";

        // Changes the registry version.
        public static string registryVersion = "10";
        public static string fullversion = $"v{version}";

        // DO NOT CHANGE, NEEDS TO BE SET TO TRUE BY DEFAULT
        public static bool isUpToDate = true;

        // Will be changed by ThemeLoader, change default colors in ThemeLoader.cs
        public static CliColor LogoColor { get; set; } = ConsoleColor.Gray;
        public static CliColor MenuColor { get; set; } = ConsoleColor.DarkGray;
        public static CliColor SecondaryColor { get; set; } = ConsoleColor.Red;


        // The motd

        public static string Motd()
        {
            CultureInfo culture = new CultureInfo(Variables.lang);
            ResourceManager rm = new ResourceManager("reShutCLI.Resources.Strings", typeof(Program).Assembly);
            return prerelease ? $"{rm.GetString("UpToDate", culture)}" : $"{rm.GetString("UpToDate", culture)}";
        }
    }
}
