using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reShutLegacy
{
    internal class variables
    {
        // Set true if this is an pre-release
        public static bool prerelease = true;

        // The latest GitHub Commit checksum
        public static string buildid = "c3ee302";

        // The version
        public static string version = "v.12.0.0 (" + buildid + ")";

        // The motd
        public static string motd = "Please report any bugs in the new UI on GitHub.";
    }
}
