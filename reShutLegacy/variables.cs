using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reShutLegacy
{
    internal class Variables
    {
        // Set true if this is an pre-release
        public static bool prerelease = true;

        // The latest GitHub Commit checksum / or other important thing
        public static string buildid = "alpha-1";

        // The version
        public static string version = "v.12.0.0 (" + buildid + ")";

        // The motd
        public static string Motd()
        {
            if (prerelease) return "This version is currently under development and may experience occasional instability.";
            return "Welcome to v.12.0.0!";
        }
    }
}
