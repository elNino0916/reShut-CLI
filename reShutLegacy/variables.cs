using System;

namespace reShutCLI
{
    internal class Variables
    {
        // Set true if this is a pre-release
        public static bool prerelease = true;


        // The version
        public static string version = "";
        public static string fullversion = $"{version}"; // old: $"v.{version}

        // The motd
        public static string Motd()
        {
            return "Welcome to the .NET 9 Preview!";
        }
        // The color
        public static ConsoleColor LogoColor
        {
            get { return ConsoleColor.Magenta; }
        }
        public static ConsoleColor MenuColor
        {
            get { return ConsoleColor.DarkGreen; }
        }
    }
}
