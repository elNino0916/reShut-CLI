using System.Diagnostics;

namespace reShutCLI.Helpers;

internal static class AutoRestart
{
    /// <summary>Starts a fresh instance of the application and exits the current one.</summary>
    public static void Init()
    {
        var exePath = Environment.ProcessPath
                      ?? Path.Combine(AppContext.BaseDirectory, AppDomain.CurrentDomain.FriendlyName);

        Process.Start(new ProcessStartInfo
        {
            FileName = exePath,
            UseShellExecute = false,
        });
        Thread.Sleep(1000);
        Environment.Exit(Constants.ExitCodeSuccess);
    }
}
