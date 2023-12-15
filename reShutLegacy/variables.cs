namespace reShutLegacy
{
    internal class Variables
    {
        // Set true if this is a pre-release
        public static bool prerelease = true;

        // The latest GitHub Commit checksum / or other important thing
        public static string buildid = "alpha-5";

        // The version
        public static string version = "12.0.0";
        public static string fullversion = $"v.{version} (" + buildid + ")";

        // The motd
        public static string Motd()
        {
            return prerelease ? "This version is currently under development and may experience occasional instability. (reShut is up to date!)" : "Welcome to v.12.0.0! (reShut is up to date!)";
        }
    }
}
