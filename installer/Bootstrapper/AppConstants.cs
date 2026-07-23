namespace reShutCLI.Bootstrapper;

/// <summary>
/// Mirrors the constants the main application uses for its registry layout.
/// Duplicated deliberately: the bootstrapper is a standalone single-file exe
/// with no project reference to the main app.
/// </summary>
internal static class AppConstants
{
    public const string AppName = "reShut CLI";
    public const string AppExeName = "reShutCLI.exe";

    /// <summary>
    /// The installer copies itself into the install directory under this name; running
    /// as it is what puts the bootstrapper into uninstall mode.
    /// </summary>
    public const string UninstallExeName = "uninstall.exe";

    /// <summary>
    /// Task Scheduler folder the application registers recurring shutdowns into.
    /// Must match TaskSchedulerService.TaskFolder in the main app.
    /// </summary>
    public const string TaskFolder = @"\reShut CLI";

    public const string Version = "2.2.0";
    public const string Publisher = "elNino0916";
    public const string ProjectUrl = "https://github.com/elnino0916/reshut-cli";

    public const string UninstallKey = @"Software\Microsoft\Windows\CurrentVersion\Uninstall\reShutCLI";
    public const string OldInnoUninstallKeyName = @"Software\Microsoft\Windows\CurrentVersion\Uninstall\{89CA6EED-11D7-40FD-A2C7-5E234D978BDE}_is1";

    public const string UserSettingsKey = @"Software\elNino0916\reShutCLI";
    public const string UserPoliciesKey = @"Software\elNino0916\Policies\reShutCLI";

    public const int DotNetMajorVersion = 10;
    public const string DotNetSharedFxRegKey = @"SOFTWARE\dotnet\Setup\InstalledVersions\x64\sharedfx\Microsoft.NETCore.App";
    public const string DotNetRuntimeDownloadUrl = "https://aka.ms/dotnet/10.0/dotnet-runtime-win-x64.exe";
}
