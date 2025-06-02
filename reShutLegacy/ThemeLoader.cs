using System;

namespace reShutCLI
{
    internal class ThemeLoader
    {
        public static void loadTheme()
        {
            if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme") == "default")
            {
                setDefaultTheme();
            }
            if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme") == "red")
            {
                setRedTheme();
            }
            if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme") == "blue")
            {
                setBlueTheme();
            }
            if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme") == "green")
            {
                setGreenTheme();
            }
            if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme") == "nord")
            {
                setNordTheme();
            }
        }

        public static void setDefaultTheme()
        {
            Variables.MenuColor = ConsoleColor.Cyan;
            Variables.LogoColor = ConsoleColor.Yellow;
            Variables.SecondaryColor = ConsoleColor.Red;
        }
        public static void setRedTheme()
        {
            Variables.MenuColor = ConsoleColor.Red;
            Variables.LogoColor = ConsoleColor.DarkRed;
            Variables.SecondaryColor = ConsoleColor.Magenta;
        }
        public static void setBlueTheme()
        {
            Variables.MenuColor = ConsoleColor.Blue;
            Variables.LogoColor = ConsoleColor.DarkBlue;
            Variables.SecondaryColor = ConsoleColor.DarkGreen;
        }
        public static void setGreenTheme()
        {
            Variables.MenuColor = ConsoleColor.Green;
            Variables.LogoColor = ConsoleColor.DarkGreen;
            Variables.SecondaryColor = ConsoleColor.Blue;
        }
        public static void setNordTheme()
        {
            Variables.MenuColor = ConsoleColor.DarkCyan;
            Variables.LogoColor = ConsoleColor.Cyan;
            Variables.SecondaryColor = ConsoleColor.DarkGray;
        }
    }
}
