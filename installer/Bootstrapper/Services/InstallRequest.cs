namespace reShutCLI.Bootstrapper.Services;

public sealed record InstallRequest(string InstallDir, bool CreateShortcut, bool LaunchWhenFinished);
