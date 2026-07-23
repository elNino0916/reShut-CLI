using reShutCLI.Helpers;
using reShutCLI.Services;

namespace reShutCLI;

/// <summary>
/// Creates the registry layout on first start and migrates it between versions.
/// </summary>
internal static class RegInit
{
    public static void Populate(bool skipCreation)
    {
        if (!skipCreation
            && RegistryWorker.ReadFromRegistry(Constants.RegistryPathBase, Constants.RegistryValueRegistryPopulated) != "1")
        {
            InitializeRegistry();
            return;
        }

        var currentRegistryVersion = RegistryWorker.ReadFromRegistry(Constants.RegistryPathBase, Constants.RegistryValueRegistryVersion);
        if (currentRegistryVersion == Variables.RegistryVersion) return;

        UIDraw.TextColor = Variables.MenuColor;
        UIDraw.DrawBoxedMessage(Localization.Get("RegUpdate"));
        Thread.Sleep(4000);
        Console.Clear();

        // Reset the registry if the app is downgraded
        if (string.CompareOrdinal(currentRegistryVersion, Variables.RegistryVersion) > 0)
        {
            RegistryWorker.WriteToRegistry(Constants.RegistryPathBase, Constants.RegistryValueRegistryPopulated, Constants.RegistryValueTypeString, "0");
            InitializeRegistry();
            return;
        }

        // Delete no longer required keys.
        RegistryWorker.DeleteFromRegistry(Constants.RegistryPathConfig, "EnableFastStartup"); // Removed in 2.0.0.0
        RegistryWorker.DeleteFromRegistry(Constants.RegistryPathConfig, "EnableSounds"); // Removed in 1.0.4.0

        // Unsupported languages were removed in a 2.0 pre-release.
        var language = RegistryWorker.ReadFromRegistry(Constants.RegistryPathConfig, Constants.RegistryValueLanguage);
        if (language is "fr-FR" or "pt-PT" or "es-ES")
        {
            RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueLanguage, Constants.RegistryValueTypeString, Constants.LanguageEnglish);
        }

        // Migrate legacy yes/no AutoUpdateOnStart values to 1/0.
        var autoUpdate = RegistryWorker.ReadFromRegistry(Constants.RegistryPathConfig, Constants.RegistryValueAutoUpdateOnStart);
        if (string.Equals(autoUpdate, "yes", StringComparison.OrdinalIgnoreCase))
            RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueAutoUpdateOnStart, Constants.RegistryValueTypeString, Constants.EnabledValue);
        else if (string.Equals(autoUpdate, "no", StringComparison.OrdinalIgnoreCase))
            RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueAutoUpdateOnStart, Constants.RegistryValueTypeString, Constants.DisabledValue);

        EnsurePoliciesKey();

        // Update the registry version
        RegistryWorker.WriteToRegistry(Constants.RegistryPathBase, Constants.RegistryValueRegistryVersion, Constants.RegistryValueTypeString, Variables.RegistryVersion);
    }

    private static void InitializeRegistry()
    {
        RegistryWorker.WriteToRegistry(Constants.RegistryPathBase, Constants.RegistryValueRegistryPopulated, Constants.RegistryValueTypeString, "1");
        RegistryWorker.WriteToRegistry(Constants.RegistryPathBase, Constants.RegistryValueFirstStartupTime, Constants.RegistryValueTypeString, DateTime.Now.ToString("HH:mm:ss (dd.MM.yyyy)"));
        EnsurePoliciesKey();

        // Settings
        RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueEnableUpdateSearch, Constants.RegistryValueTypeString, Constants.EnabledValue);
        RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueAutoUpdateOnStart, Constants.RegistryValueTypeString, Constants.DisabledValue);
        RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueEulaAccepted, Constants.RegistryValueTypeString, Constants.DisabledValue);
        RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSetupComplete, Constants.RegistryValueTypeString, Constants.DisabledValue);
        RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSelectedTheme, Constants.RegistryValueTypeString, "default");
        RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSkipConfirmation, Constants.RegistryValueTypeString, Constants.DisabledValue);
        RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueLanguage, Constants.RegistryValueTypeString, Constants.LanguageEnglish);
        RegistryWorker.WriteToRegistry(Constants.RegistryPathBase, Constants.RegistryValueRegistryVersion, Constants.RegistryValueTypeString, Variables.RegistryVersion);
        RegistryWorker.WriteToRegistry(Constants.RegistryPathBase, Constants.RegistryValueReShutVersion, Constants.RegistryValueTypeString, Variables.Version);
    }

    /// <summary>Creates the (currently empty) policies key reserved for future use.</summary>
    private static void EnsurePoliciesKey()
    {
        RegistryWorker.WriteToRegistry(Constants.RegistryPathPolicies, "Temp", Constants.RegistryValueTypeString, Variables.RegistryVersion);
        RegistryWorker.DeleteFromRegistry(Constants.RegistryPathPolicies, "Temp");
    }
}
