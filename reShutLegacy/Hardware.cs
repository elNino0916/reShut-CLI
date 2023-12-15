using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace reShutLegacy
{
    internal class Hardware
    {
        #pragma warning disable CA1416
        public static string GetTime(bool use24HoursFormat)
        {
            return DateTime.Now.ToString(!use24HoursFormat ? "hh:mm:ss tt" : "HH:mm:ss");
        }
        public static string GetCPU()
        {
            ManagementObjectSearcher searcher = new("select * from Win32_Processor");
            return searcher.Get().Cast<ManagementObject>().Select(obj => obj["Name"].ToString()).FirstOrDefault();
        }
        public static string GetGPU()
        {
            ManagementObjectSearcher searcher = new("select * from Win32_VideoController ");
            foreach (var obj in searcher.Get().Cast<ManagementObject>())
            {
                var VC = obj["Description"].ToString();
                return VC;
                    
            }
                
            return "Not detected! / Unknown error.";
        }
        public static string GetRAM()
        {
            ManagementObjectSearcher searcher = new("select Capacity from Win32_PhysicalMemory");

            var ramSize = searcher.Get().Cast<ManagementObject>().Aggregate<ManagementObject, ulong>(0, (current, obj) => current + Convert.ToUInt64(obj["Capacity"]));

            var ramInGB = (double)ramSize / (1024 * 1024 * 1024);
            return ramInGB.ToString("0.##");
        }
        #pragma warning restore CA1416
    }
}
