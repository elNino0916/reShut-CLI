using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

internal class Handler
{
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

    // Flags for ExitWindowsEx
    private const uint EWX_LOGOFF = 0x00000000;
    private const uint EWX_SHUTDOWN = 0x00000001;
    private const uint EWX_REBOOT = 0x00000002;
    private const uint EWX_FORCE = 0x00000004;

    public static void Reboot()
    {
        if (!ExitWindowsEx(EWX_REBOOT | EWX_FORCE, 0))
        {
           Process.Start("shutdown", "/r /f /t 0");
        }
    }

    public static void Shutdown()
    {
        // Shutdown the computer
        if (!ExitWindowsEx(EWX_SHUTDOWN | EWX_FORCE, 0))
        {
           Process.Start("shutdown", "/s /f /t 0");
        }
    }

    public static void Logoff()
    {
        // Log off the current user
        if (!ExitWindowsEx(EWX_LOGOFF, 0))
        {
           Process.Start("shutdown", "/l");
        }
    }
}
