using System;
using System.Diagnostics.Eventing.Reader;

namespace reShutCLI
{
    internal class Variables
    {
        // Update 1.0.2.0 features
        public static bool UnlockExperimentalEULAAcceptingFeature = false;

        public static bool ForceEULAAcceptionOnEveryRestart = false;

        public static bool AllowNewErrorHandlingUserExperience = true;

        public static bool ForceNewErrorExperienceOnStart = false;

        // Alpha Features

        public static bool EnableRegistryIntegration = false;


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
