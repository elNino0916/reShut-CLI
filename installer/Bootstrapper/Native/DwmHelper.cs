using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace reShutCLI.Bootstrapper.Native;

/// <summary>Applies Windows 11 dark title bar + rounded window corners via DWM.</summary>
internal static class DwmHelper
{
    [DllImport("dwmapi.dll", PreserveSig = true)]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int size);

    private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
    private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
    private const int DWMWCP_ROUND = 2;

    /// <summary>
    /// Enables the dark title bar and rounded corners on Windows 11. Both
    /// attributes are silently ignored by DwmSetWindowAttribute on Windows 10,
    /// so this is safe to call unconditionally.
    /// </summary>
    public static void ApplyDarkRoundedChrome(Window window)
    {
        var hwnd = new WindowInteropHelper(window).EnsureHandle();
        var enabled = 1;
        DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref enabled, sizeof(int));
        var round = DWMWCP_ROUND;
        DwmSetWindowAttribute(hwnd, DWMWA_WINDOW_CORNER_PREFERENCE, ref round, sizeof(int));
    }
}
