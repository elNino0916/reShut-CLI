using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reShutCLI
{
    internal class ThemeLoader
    {
        // Available Themes: 
        // Default 
        // Cyberpunk: Yellow | Blue
        public static void loadTheme()
        {
            if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme") == "default")
            {
                setDefaultTheme();
            }
            if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme") == "cyberpunk")
            {
                setCyberpunkTheme();
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
        }

        public static void setDefaultTheme()
        {
            Variables.MenuColor = ConsoleColor.DarkYellow;
            Variables.LogoColor = ConsoleColor.Yellow;
            Variables.SecondaryColor = ConsoleColor.DarkRed;
        }
        public static void setCyberpunkTheme()
        {
            Variables.MenuColor = ConsoleColor.Blue;
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
    }
}
