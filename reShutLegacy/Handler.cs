using System.Diagnostics;
namespace reShutCLI
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
