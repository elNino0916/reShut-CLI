using System;

namespace reShutLegacy
{
    internal class Variables
    {
        // Set true if this is a pre-release
        public static bool prerelease = false;


        // The version
        public static string version = "1.0.1.1";
        public static string fullversion = $"v.{version}";

        // The motd
        public static string Motd()
        {
            return prerelease ? "This version is currently under development and may experience occasional instability. (reShut is up to date!)" : "reShut is up to date!";
        }
        // The color
        public static ConsoleColor LogoColor
        {
            get { return ConsoleColor.Green; }
        }
        public static ConsoleColor MenuColor
        {
            get { return ConsoleColor.Cyan; }
        }
    }
}
