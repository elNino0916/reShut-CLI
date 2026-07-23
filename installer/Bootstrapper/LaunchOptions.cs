namespace reShutCLI.Bootstrapper;

/// <summary>Command-line options, compatible with the /S silent switch the app's AutoUpdater passes.</summary>
public sealed class LaunchOptions
{
    public bool Silent { get; private init; }
    public bool Uninstall { get; private init; }
    public string? InstallDir { get; private init; }

    public static LaunchOptions Parse(string[] args)
    {
        var silent = false;
        var uninstall = false;
        string? installDir = null;

        foreach (var arg in args)
        {
            if (arg.Equals("/S", StringComparison.OrdinalIgnoreCase))
            {
                silent = true;
            }
            else if (arg.Equals("/uninstall", StringComparison.OrdinalIgnoreCase))
            {
                uninstall = true;
            }
            else if (arg.StartsWith("/D=", StringComparison.OrdinalIgnoreCase))
            {
                installDir = arg.Substring(3);
            }
            else if (arg.StartsWith("/DIR=", StringComparison.OrdinalIgnoreCase))
            {
                installDir = arg.Substring(5);
            }
        }

        return new LaunchOptions { Silent = silent, Uninstall = uninstall, InstallDir = installDir };
    }
}
