using System;
using System.IO;

namespace reShutLegacy
{
    internal class Preload
    {
        private static string[] preloadedStrings;

        public static void Startup(bool atStartup)
        {
            // Check if the strings are already preloaded
            if (preloadedStrings != null)
            {
                return;
            }
            // Check for FastStartup
            if (atStartup)
            {
                if (File.ReadAllText(@"config\startup.cfg") == "fast=enabled" ) 
                {
                    return; 
                }
            }
            // Preload the strings here
            preloadedStrings = new string[]
            {
                Environment.OSVersion.Version.ToString(), // 0
                Hardware.GetCPU(), // 1
                /*HWID.GetHWID()*/ "Currently disabled due to performance issues.", // 2
                Hardware.GetRAM(), // 3
                Hardware.GetGPU(), // 4
                "1" // 5 (This marks that the strings have been preloaded)
            };
        }
        public static string GetString(int index)
        {
            // Check if the array has been preloaded
            if (preloadedStrings == null)
            {
                // Not preloaded, preload now
                Startup(false);
            }

            if (index >= 0 && index < preloadedStrings.Length)
            {
                return preloadedStrings[index];
            }
            else
            {
                return "Invalid index";
            }
        }
        public static void Reset()
        {
            // Reset the preloaded strings by setting the array to null
            preloadedStrings = null;
        }
    }
}