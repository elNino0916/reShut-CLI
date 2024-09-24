using System;
using System.Globalization;
using System.Management;
using System.Resources;

namespace reShutCLI
{
    internal class Variables
    {
        // TODO:
        // Implement SkipConfirmation
        // Settings Rework

        // Set true if this is a pre-release
        public static bool prerelease = true;
        public static bool DevelopmentBuild = true;

        // Language
        public static string lang = "de-DE";

        // The version
        public static string version = "2.0.0.0";

        // Changes the registry version.
        public static string registryVersion = "5";
        public static string fullversion = $"v.{version}";

        // DO NOT CHANGE
        public static bool isUpToDate = true;

        // Will be changed by ThemeLoader, change default colors in ThemeLoader.cs
        public static ConsoleColor LogoColor { get; set; } = ConsoleColor.Green;
        public static ConsoleColor MenuColor { get; set; } = ConsoleColor.DarkCyan;
        public static ConsoleColor SecondaryColor { get; set; } = ConsoleColor.Red;


        // The motd
        public static string Motd()
        {
            CultureInfo culture = new CultureInfo(Variables.lang);
            ResourceManager rm = new ResourceManager("reShutCLI.Resources.Strings", typeof(Program).Assembly);
            return prerelease ? $"{UserGreeter()} ({rm.GetString("UpToDate", culture)})" : $"{UserGreeter()} ({rm.GetString("UpToDate", culture)})";
        }
        public static string UserGreeter()
        {
            CultureInfo culture = new CultureInfo(Variables.lang);
            ResourceManager rm = new ResourceManager("reShutCLI.Resources.Strings", typeof(Program).Assembly);
            if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme") == "cyberpunk")
            {
                DateTime now1 = DateTime.Now;
                string greeting1;

                if (now1.Hour < 12)
                {
                    greeting1 = $"{rm.GetString("GoodMorning", culture)}, Choom!";
                }
                else if (now1.Hour < 18)
                {
                    greeting1 = $"{rm.GetString("GoodMorning", culture)}, Choom!";
                }
                else
                {
                    greeting1 = $"{rm.GetString("GoodMorning", culture)}, Choom!";
                }

                return greeting1;
            }

            string fullName = GetFullName();
            DateTime now = DateTime.Now;
            string greeting;

            if (now.Hour < 12)
            {
                greeting = $"{rm.GetString("GoodMorning", culture)}, {fullName}!";
            }
            else if (now.Hour < 18)
            {
                greeting = $"{rm.GetString("GoodDay", culture)}, {fullName}!";
            }
            else
            {
                greeting = $"{rm.GetString("GoodEvening", culture)}, {fullName}!";
            }

            return greeting;
        }

        private static string GetFullName()
        {
            string fullName = Environment.UserName;
            string firstName = fullName;

            try
            {
                string query = "SELECT FullName FROM Win32_UserAccount WHERE Name = '" + Environment.UserName + "'";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                foreach (ManagementObject obj in searcher.Get())
                {
                    fullName = obj["FullName"].ToString();
                    break;
                }
            }
            catch
            {

            }

            if (!string.IsNullOrEmpty(fullName))
            {
                string[] nameParts = fullName.Split(' ');
                if (nameParts.Length > 0)
                {
                    firstName = nameParts[0];
                }
            }

            return firstName;
        }
    }
}
