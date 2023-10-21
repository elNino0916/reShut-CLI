using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reShutLegacy
{
    internal class Handler
    {
        public static void Reboot()
        {
            Process.Start("shutdown", "/r /f /t 0");
        }
        public static void Shutdown()
        { 
            Process.Start("shutdown", "/s /f /t 0");
        }
        public static void Logoff()
        {
            Process.Start("shutdown", "/l");
        }
    }
}
