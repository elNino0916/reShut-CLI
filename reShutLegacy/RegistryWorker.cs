using System;
using Microsoft.Win32;

namespace reShutCLI
{
    class RegistryWorker
    {
        public static void WriteToRegistry(string registryPath, string keyName, string type, string content)
        {
            try
            {
                // Determine the registry base key and subkey
                string[] pathParts = registryPath.Split('\\', 2);
                string baseKey = pathParts[0];
                string subKey = pathParts[1];

                // Get the appropriate base key
                RegistryKey registryKey = baseKey switch
                {
                    "HKEY_CLASSES_ROOT" => Registry.ClassesRoot,
                    "HKEY_CURRENT_USER" => Registry.CurrentUser,
                    "HKEY_LOCAL_MACHINE" => Registry.LocalMachine,
                    "HKEY_USERS" => Registry.Users,
                    "HKEY_CURRENT_CONFIG" => Registry.CurrentConfig,
                    _ => throw new ArgumentException("Invalid base registry key."),
                };

                // Open the subkey for writing
                using RegistryKey key = registryKey.OpenSubKey(subKey, writable: true) ?? registryKey.CreateSubKey(subKey);

                if (key == null)
                {
                    ErrorHandler.ShowError(@$"General Registry failure, cant open/create: {registryPath}. Try deleting the registry keys in HKCU\Software\elNino0916\reShutCLI", true);
                    return;
                }

                // Determine the value type and write to the registry
                switch (type.ToUpper())
                {
                    case "STRING":
                        key.SetValue(keyName, content, RegistryValueKind.String);
                        break;
                    case "DWORD":
                        if (int.TryParse(content, out int intValue))
                        {
                            key.SetValue(keyName, intValue, RegistryValueKind.DWord);
                        }
                        else
                        {
                            ErrorHandler.ShowError(@$"Internal registry failure [INVALID_DWORD_CONTENT]", true);
                        }
                        break;
                    case "QWORD":
                        if (long.TryParse(content, out long longValue))
                        {
                            key.SetValue(keyName, longValue, RegistryValueKind.QWord);
                        }
                        else
                        {
                            ErrorHandler.ShowError(@$"Internal registry failure [INVALID_QWORD_CONTENT]", true);
                        }
                        break;
                    case "BINARY":
                        try
                        {
                            byte[] binaryData = Convert.FromBase64String(content);
                            key.SetValue(keyName, binaryData, RegistryValueKind.Binary);
                        }
                        catch (FormatException)
                        {
                            ErrorHandler.ShowError(@$"Internal registry failure [INVALID_BINARY_CONTENT]", true);
                        }
                        break;
                    case "MULTI_STRING":
                        string[] multiStringData = content.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        key.SetValue(keyName, multiStringData, RegistryValueKind.MultiString);
                        break;
                    default:
                        ErrorHandler.ShowError(@$"Internal registry failure [UNSUPPORTED_REGISTRY_TYPE]", true);
                        break;
                }

            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError(@$"Internal registry failure: {ex.Message}", true);
            }
        }

        public static string ReadFromRegistry(string registryPath, string keyName)
        {
            try
            {
                // Determine the registry base key and subkey
                string[] pathParts = registryPath.Split('\\', 2);
                string baseKey = pathParts[0];
                string subKey = pathParts[1];

                // Get the appropriate base key
                RegistryKey registryKey = baseKey switch
                {
                    "HKEY_CLASSES_ROOT" => Registry.ClassesRoot,
                    "HKEY_CURRENT_USER" => Registry.CurrentUser,
                    "HKEY_LOCAL_MACHINE" => Registry.LocalMachine,
                    "HKEY_USERS" => Registry.Users,
                    "HKEY_CURRENT_CONFIG" => Registry.CurrentConfig,
                    _ => throw new ArgumentException("Invalid base registry key."),
                };

                // Open the subkey for reading
                using RegistryKey key = registryKey.OpenSubKey(subKey, writable: false);

                if (key == null)
                {
                    ErrorHandler.ShowError(@$"General Registry failure, cant open: {registryPath}. Try deleting the registry keys in HKCU\Software\elNino0916\reShutCLI", true);
                    return null;
                }

                // Read the value from the registry
                object value = key.GetValue(keyName);

                if (value == null)
                {
                    Console.WriteLine("Registry value not found.");
                    ErrorHandler.ShowError(@$"Could not find required registry {registryPath}. Try deleting the registry keys in HKCU\Software\elNino0916\reShutCLI", true);
                    return null;
                }

                return value.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from registry: {ex.Message}");
                ErrorHandler.ShowError(@$"Internal registry error: {ex.Message}. Try deleting the registry keys in HKCU\Software\elNino0916\reShutCLI", true);
                return null;
            }
        }
    }
}
