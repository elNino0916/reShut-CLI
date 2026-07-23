using Microsoft.Win32;

namespace reShutCLI.Services;

/// <summary>
/// Thin wrapper around the Windows registry used for all persisted settings.
/// All operations swallow errors by design: settings fall back to defaults.
/// </summary>
internal static class RegistryWorker
{
    private static (RegistryKey BaseKey, string SubKey) Resolve(string registryPath)
    {
        var pathParts = registryPath.Split('\\', 2);
        RegistryKey baseKey = pathParts[0] switch
        {
            "HKEY_CLASSES_ROOT" => Registry.ClassesRoot,
            "HKEY_CURRENT_USER" => Registry.CurrentUser,
            "HKEY_LOCAL_MACHINE" => Registry.LocalMachine,
            "HKEY_USERS" => Registry.Users,
            "HKEY_CURRENT_CONFIG" => Registry.CurrentConfig,
            _ => throw new ArgumentException("Invalid base registry key.", nameof(registryPath)),
        };
        return (baseKey, pathParts[1]);
    }

    public static void WriteToRegistry(string registryPath, string keyName, string type, string content)
    {
        try
        {
            var (baseKey, subKey) = Resolve(registryPath);
            using var key = baseKey.OpenSubKey(subKey, writable: true) ?? baseKey.CreateSubKey(subKey);

            switch (type.ToUpperInvariant())
            {
                case "STRING":
                    key.SetValue(keyName, content, RegistryValueKind.String);
                    break;
                case "DWORD":
                    if (int.TryParse(content, out var intValue))
                        key.SetValue(keyName, intValue, RegistryValueKind.DWord);
                    break;
                case "QWORD":
                    if (long.TryParse(content, out var longValue))
                        key.SetValue(keyName, longValue, RegistryValueKind.QWord);
                    break;
                case "BINARY":
                    try
                    {
                        key.SetValue(keyName, Convert.FromBase64String(content), RegistryValueKind.Binary);
                    }
                    catch (FormatException)
                    {
                    }
                    break;
                case "MULTI_STRING":
                    var multiStringData = content.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    key.SetValue(keyName, multiStringData, RegistryValueKind.MultiString);
                    break;
            }
        }
        catch
        {
        }
    }

    public static string? ReadFromRegistry(string registryPath, string keyName)
    {
        try
        {
            var (baseKey, subKey) = Resolve(registryPath);
            using var key = baseKey.OpenSubKey(subKey, writable: false);
            return key?.GetValue(keyName)?.ToString();
        }
        catch
        {
            return null;
        }
    }

    public static void DeleteFromRegistry(string registryPath, string keyName)
    {
        try
        {
            var (baseKey, subKey) = Resolve(registryPath);
            using var key = baseKey.OpenSubKey(subKey, writable: true);
            key?.DeleteValue(keyName, throwOnMissingValue: false);
        }
        catch
        {
        }
    }

    public static bool Exists(string registryPath, string keyName) =>
        ReadFromRegistry(registryPath, keyName) is not null;
}
