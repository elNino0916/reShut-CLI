namespace reShutLegacy
{
    internal class Variables
    {
        // Set true if this is a pre-release
        public static bool prerelease = false;

        // BuildID disabled for now
        //public static string buildid = "00001"; 

        // The version
        public static string version = "1.0.0.1";
        public static string fullversion = $"v.{version}";

        // The motd
        public static string Motd()
        {
            return prerelease ? "This version is currently under development and may experience occasional instability. (reShut is up to date!)" : "Welcome to v.1.0.0! (reShut is up to date!)";
        }
    }
}
