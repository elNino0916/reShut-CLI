namespace reShutCLI.Bootstrapper.Services;

/// <summary>A step in the install/uninstall pipeline: overall percentage plus a status line.</summary>
internal readonly record struct InstallProgress(double Percent, string Status);
