using System;
using System.Diagnostics.Eventing.Reader;

namespace reShutCLI
{
    internal class Variables
    {
        // 1.0.3.0 feature toggles
        public static bool EnableWelcomeUI = false; // Not implemented yet.
        public static bool EnableSounds = false; // Not implemented yet.
        public static bool ConfigureReShutCLIAfterWelcomeUI = false; // Not implemented yet.
        public static bool ResetFunctionEnabled = false; // Not implemented yet.
        public static bool reShutUIIntegration = false; // Not implemented yet.
        public static bool RunInBackground = false; // Not implemented yet.
        public static bool AcceptLicenseInWelcomeUI = false; // Not implemented yet.
        public static bool AcceptEULAInWelcomeUI = false; // Not implemented yet.


        // Set true if this is a pre-release
        public static bool prerelease = false;


        // The version
        public static string version = "1.0.2.0";
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
            get { return ConsoleColor.Yellow; }
        }
    }
}
