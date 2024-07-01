using System;
using System.Diagnostics.Eventing.Reader;

namespace reShutCLI
{
    internal class Variables
    {


        // Set true if this is a pre-release
        public static bool prerelease = true;


        // The version
        public static string version = "1.0.3.0";
        public static string fullversion = $"v.{version} Beta 3";

        // DO NOT CHANGE
        public static bool isUpToDate = true;

        // The motd
        public static string Motd()
        {
            return prerelease ? "You are using a beta version. (reShut is up to date!)" : "reShut is up to date!";
        }
        // The color
        public static ConsoleColor LogoColor { get; set; } = ConsoleColor.DarkYellow;
        public static ConsoleColor MenuColor { get; set; } = ConsoleColor.Yellow;
        public static ConsoleColor SecondaryColor { get; set; } = ConsoleColor.Red;
    }
}
