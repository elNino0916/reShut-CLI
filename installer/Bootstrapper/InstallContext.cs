namespace reShutCLI.Bootstrapper;

/// <summary>Carries the parsed launch options into MainWindow for a fresh install.</summary>
public sealed class InstallContext(LaunchOptions options)
{
    public LaunchOptions Options { get; } = options;
}

/// <summary>Marker telling MainWindow to show the uninstall flow instead of install.</summary>
public sealed class UninstallContext;
