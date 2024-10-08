﻿using System;
using System.Linq;
using System.Management;

namespace reShutCLI
{
    internal class Hardware
    {
#pragma warning disable CA1416
        public static string GetTime(bool use24HoursFormat)
        {
            return DateTime.Now.ToString(!use24HoursFormat ? "hh:mm:ss tt" : "HH:mm:ss");
        }
        public static string GetCPU() // Obsolete, will be removed in future update.
        {
            ManagementObjectSearcher searcher = new("select * from Win32_Processor");
            return searcher.Get().Cast<ManagementObject>().Select(obj => obj["Name"].ToString()).FirstOrDefault();
        }
        public static string GetGPU() // Obsolete, will be removed in future update.
        {
            ManagementObjectSearcher searcher = new("select * from Win32_VideoController ");
            foreach (var obj in searcher.Get().Cast<ManagementObject>())
            {
                var VC = obj["Description"].ToString();
                return VC;

            }

            return "Not detected! / Unknown error.";
        }
        public static string GetRAM() // Obsolete, will be removed in future update.
        {
            ManagementObjectSearcher searcher = new("select Capacity from Win32_PhysicalMemory");

            var ramSize = searcher.Get().Cast<ManagementObject>().Aggregate<ManagementObject, ulong>(0, (current, obj) => current + Convert.ToUInt64(obj["Capacity"]));

            var ramInGB = (double)ramSize / (1024 * 1024 * 1024);
            return ramInGB.ToString("0.##");
        }
#pragma warning restore CA1416
    }
}
