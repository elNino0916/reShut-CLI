using System;
using System.Diagnostics.Eventing.Reader;

namespace reShutCLI
{
    internal class Variables
    {


        // Set true if this is a pre-release
        public static bool prerelease = true;


        // The version
        public static string version = "1.0.1.0";
        public static string fullversion = $"v.{version} Beta 2";
        public static bool isUpToDate = true;

        // The motd
        public static string Motd()
        {
            return prerelease ? "This version is currently under development and may experience occasional instability. (reShut is up to date!)" : "reShut is up to date!";
        }
        // The color
        public static ConsoleColor LogoColor
        {
            get { return ConsoleColor.DarkYellow; }
        }
        public static ConsoleColor MenuColor
        {
            get { return ConsoleColor.Yellow; }
        }
    }
}
