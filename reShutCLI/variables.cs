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
        public static string version = "2.0.3";

        // Changes the registry version.
        public static string registryVersion = "10";
        public static string fullversion = $"v{version}";

        // DO NOT CHANGE, NEEDS TO BE SET TO TRUE BY DEFAULT
        public static bool isUpToDate = true;

        // Will be changed by ThemeLoader, change default colors in ThemeLoader.cs
        public static ConsoleColor LogoColor { get; set; } = ConsoleColor.Yellow;   
        public static ConsoleColor MenuColor { get; set; } = ConsoleColor.Cyan;  
        public static ConsoleColor SecondaryColor { get; set; } = ConsoleColor.Magenta; 


        // The motd

        public static string Motd()
        {
            CultureInfo culture = new CultureInfo(Variables.lang);
            ResourceManager rm = new ResourceManager("reShutCLI.Resources.Strings", typeof(Program).Assembly);
            return prerelease ? $"{rm.GetString("UpToDate", culture)}" : $"{rm.GetString("UpToDate", culture)}";
        }
    }
}
