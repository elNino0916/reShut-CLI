using System.Management;
using reShutCLI.Helpers;

namespace reShutCLI.Services;

/// <summary>Triggers shutdown or reboot on a remote host via WMI.</summary>
internal static class RemoteManager
{
    public static bool Trigger(string host, string? username, string? password, bool reboot)
    {
        try
        {
            var options = new ConnectionOptions();
            if (!string.IsNullOrWhiteSpace(username))
            {
                options.Username = username;
                options.Password = password ?? string.Empty;
            }

            var scope = new ManagementScope($@"\\{host}\root\cimv2", options);
            scope.Connect();

            var query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            using var searcher = new ManagementObjectSearcher(scope, query);
            foreach (var os in searcher.Get().Cast<ManagementObject>())
            {
                // 5 = forced shutdown, 6 = forced reboot
                os.InvokeMethod("Win32Shutdown", [reboot ? 6 : 5, 0]);
            }

            UIDraw.TextColor = ConsoleColor.Green;
            UIDraw.DrawLine($"Remote {(reboot ? "reboot" : "shutdown")} triggered on {host}.");
            return true;
        }
        catch (Exception ex)
        {
            UIDraw.TextColor = ConsoleColor.Red;
            UIDraw.DrawLine($"Remote operation failed: {ex.Message}");
            return false;
        }
        finally
        {
            UIDraw.TextColor = ConsoleColor.White;
        }
    }
}
