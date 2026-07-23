using reShutCLI.Helpers;
using reShutCLI.Services;

namespace reShutCLI;

/// <summary>
/// Central application state and metadata. The version is sourced from the
/// assembly version, which is defined once in the project file.
/// </summary>
internal static class Variables
{
    // Set true if this is a pre-release
    public static bool IsPreRelease => false;

    // The application version, single-sourced from the csproj <Version> property.
    public static readonly string Version =
        typeof(Variables).Assembly.GetName().Version is { } v ? $"{v.Major}.{v.Minor}.{v.Build}" : "2.2.1";

    public static string FullVersion => $"v{Version}";

    // Theme API endpoint
    public const string ApiUrl = "http://api.elnino0916.de/api/v4/reshutcli/theme/default";

    // Bump when the registry layout changes so migrations in RegInit run once.
    public const string RegistryVersion = "14";

    // Language, cached after first read. Falls back to en-US before setup has run.
    private static string? _language;
    public static string Language =>
        _language ??= RegistryWorker.ReadFromRegistry(Constants.RegistryPathConfig, Constants.RegistryValueLanguage)
                      ?? Constants.LanguageEnglish;

    // Set to false by UpdateChecker when a newer release exists.
    public static bool IsUpToDate { get; set; } = true;

    // Populated by ThemeLoader; defaults are the fallback theme.
    public static CliColor LogoColor { get; set; } = ConsoleColor.Gray;
    public static CliColor MenuColor { get; set; } = ConsoleColor.DarkGray;
    public static CliColor SecondaryColor { get; set; } = ConsoleColor.Red;
    public static CliColor BackgroundColor { get; set; } = ConsoleColor.Red;
    public static string UpdatedDefaultThemeName { get; set; } = "";

    // The motd shown when the app is up to date.
    public static string Motd() => Localization.Get("UpToDate");
}
