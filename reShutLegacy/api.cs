using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace reShutLegacy
{
    internal class api
    {
        public static string GetDate()
        {
            return DateTime.Now.ToString("dd.MM.yyyy");
        }
        public static string GetTime(bool use24HoursFormat)
        {
            if (!use24HoursFormat)
            {
                return DateTime.Now.ToString("hh:mm:ss");
            }
            else
            {
                return DateTime.Now.ToString("HH:mm:ss");
            }
        }
        public static string GetCPU()
        {
            string cpuName = null;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                cpuName = obj["Name"].ToString();
                break;
            }
            return cpuName;
        }
        public static string GetGPU()
        {
            string gpu = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_VideoController");
            foreach (ManagementObject obj in searcher.Get())
            {
                gpu = obj["Name"].ToString();
                break;
            }
            return gpu;
        }
        public static string GetMainboard()
        {
            string mb = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_BaseBoard");
            foreach (ManagementObject obj in searcher.Get())
            {
                mb = obj["Product"].ToString();
                break;
            }
            return mb;
        }
        public static string GetHardDrive()
        {
            string hd = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_DiskDrive WHERE InterfaceType='IDE' OR InterfaceType='SCSI' OR InterfaceType='SAS' OR InterfaceType='SATA'");
            foreach (ManagementObject obj in searcher.Get())
            {
                hd = obj["SerialNumber"].ToString();
                break;
            }
            return hd;
        }

        public static string GetBIOSSerial()
        {
            string hd = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_BIOS");
            foreach (ManagementObject obj in searcher.Get())
            {
                hd = obj["SerialNumber"].ToString();
                break;
            }
            return hd;
        }

        public static string GetRAM()
        {
            ulong ramSize = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select Capacity from Win32_PhysicalMemory");

            foreach (ManagementObject obj in searcher.Get())
            {
                ramSize += Convert.ToUInt64(obj["Capacity"]);
            }

            return ramSize.ToString();
        }
        public static string GetCPUID()
        {
            string hd = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                hd = obj["ProcessorID"].ToString();
                break;
            }
            return hd;
        }
        public static string GetProductKey()
        {
            try
            {
                string hd = "";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from SoftwareLicensingService");
                foreach (ManagementObject obj in searcher.Get())
                {
                    hd = obj["ProductKey"].ToString();
                    break;
                }
                return hd;
            }
            catch
            {
                return "Not found";
            }
        }
        public static string GetMacAdress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            string macAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (macAddress == string.Empty)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    macAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return macAddress;
        }
    }
}
