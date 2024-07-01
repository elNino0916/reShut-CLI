using System;
using System.Diagnostics.Eventing.Reader;
using System.Management;

namespace reShutCLI
{
    internal class Variables
    {


        // Set true if this is a pre-release
        public static bool prerelease = true;


        // The version
        public static string version = "1.0.4.0";

        // Changes the registry version.
        public static string registryVersion = "1.0";
        public static string fullversion = $"v.{version}";

        // DO NOT CHANGE
        public static bool isUpToDate = true;







        // The motd
        public static string Motd()
        {
            return prerelease ? $"{UserGreeter()} (reShut is up to date!)" : $"{UserGreeter()}(reShut is up to date!)";
        }
        public static string UserGreeter()
        {
            if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme") == "cyberpunk"){
                DateTime now1 = DateTime.Now;
                string greeting1;

                if (now1.Hour < 12)
                {
                    greeting1 = $"Good Morning, Choom!";
                }
                else if (now1.Hour < 18)
                {
                    greeting1 = $"Good Day, Choom!";
                }
                else
                {
                    greeting1 = $"Good Evening, Choom!";
                }

                return greeting1;
            }

            string fullName = GetFullName();
            DateTime now = DateTime.Now;
            string greeting;

            if (now.Hour < 12)
            {
                greeting = $"Good Morning, {fullName}!";
            }
            else if (now.Hour < 18)
            {
                greeting = $"Good Day, {fullName}!";
            }
            else
            {
                greeting = $"Good Evening, {fullName}!";
            }

            return greeting;
        }

        // The color
        public static ConsoleColor LogoColor { get; set; } = ConsoleColor.DarkYellow;
        public static ConsoleColor MenuColor { get; set; } = ConsoleColor.Yellow;
        public static ConsoleColor SecondaryColor { get; set; } = ConsoleColor.Red;







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
