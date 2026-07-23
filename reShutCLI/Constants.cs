namespace reShutCLI;

internal static class Constants
{
    // Exit Codes
    public const int ExitCodeSuccess = 0;
    public const int ExitCodeError = 1;

    // Registry Values
    public const string EulaNotAcceptedValue = "0";
    public const string SkipConfirmationEnabledValue = "1";
    public const string UpdateSearchEnabledValue = "1";
    public const string EnabledValue = "1";
    public const string DisabledValue = "0";

    // Registry Keys
    public const string RegistryValueEulaAccepted = "EULAAccepted";
    public const string RegistryValueSkipConfirmation = "SkipConfirmation";
    public const string RegistryValueEnableUpdateSearch = "EnableUpdateSearch";
    public const string RegistryValueAutoUpdateOnStart = "AutoUpdateOnStart";
    public const string RegistryValueReShutVersion = "reShutVersion";
    public const string RegistryValueRegistryVersion = "RegistryVersion";
    public const string RegistryValueRegistryPopulated = "RegistryPopulated";
    public const string RegistryValueSetupComplete = "SetupComplete";
    public const string RegistryValueSelectedTheme = "SelectedTheme";
    public const string RegistryValueLanguage = "Language";
    public const string RegistryValueDefaultThemeRecentName = "DefaultThemeRecentName";
    public const string RegistryValueFirstStartupTime = "FirstStartupTime";

    // Registry Value Types
    public const string RegistryValueTypeString = "STRING";

    // Registry Paths
    public const string RegistryPathBase = @"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI";
    public const string RegistryPathConfig = RegistryPathBase + @"\config";
    public const string RegistryPathPolicies = @"HKEY_CURRENT_USER\Software\elNino0916\Policies\reShutCLI";

    // Languages
    public const string LanguageEnglish = "en-US";
    public const string LanguageGerman = "de-DE";

    // External endpoints
    public const string GitHubRepoUrl = "https://github.com/elnino0916/reshut-cli";
    public const string GitHubLatestReleaseApiUrl = "https://api.github.com/repos/elnino0916/reshut-cli/releases/latest";

    // UI Magic Numbers
    public const int BoxPaddingWidth = 2;
    public const int MinimumBoxWidth = 44;
    public const int MenuItemPaddingWidth = 20;
    public const int UpdateDownloadMessageDelayMs = 2000;

    // Resource Names
    public const string ResourceAssemblyName = "reShutCLI.Resources.Strings";
}
