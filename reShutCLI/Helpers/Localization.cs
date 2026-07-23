using System.Globalization;
using System.Resources;

namespace reShutCLI.Helpers;

/// <summary>
/// Single cached access point for localized strings. Replaces the previous
/// pattern of creating a new ResourceManager and CultureInfo at every call site.
/// </summary>
internal static class Localization
{
    private static readonly ResourceManager Resources =
        new(Constants.ResourceAssemblyName, typeof(Localization).Assembly);

    private static CultureInfo? _culture;

    public static CultureInfo Culture => _culture ??= CreateCulture();

    private static CultureInfo CreateCulture()
    {
        try
        {
            return new CultureInfo(Variables.Language);
        }
        catch (CultureNotFoundException)
        {
            return new CultureInfo(Constants.LanguageEnglish);
        }
    }

    /// <summary>Returns the localized string for <paramref name="key"/>, or the key itself if missing.</summary>
    public static string Get(string key) => Resources.GetString(key, Culture) ?? key;

    /// <summary>Returns the localized string for <paramref name="key"/> formatted with <paramref name="args"/>.</summary>
    public static string Get(string key, params object[] args) => string.Format(Culture, Get(key), args);
}
