using System;

namespace reShutLegacy
{
    internal class Variables
    {
        // Set true if this is a pre-release
        public static bool prerelease = false;


        // The version
        public static string version = "1.0.0.2";
        public static string fullversion = $"v.{version}";
        public static string copyrightyear = DateTime.Now.ToString("yyyy");

        // The motd
        public static string Motd()
        {
            return prerelease ? "This version is currently under development and may experience occasional instability. (reShut is up to date!)" : "Merry Christmas! (reShut is up to date!)";
        }
    }
}
