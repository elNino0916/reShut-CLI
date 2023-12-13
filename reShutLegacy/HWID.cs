using System;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace reShutLegacy
{
#pragma warning disable CA1416
    internal class HWID
    {
        public static string GetHWID()
        {
            string cpuName = Hardware.GetCPU();
            string gpuName = Hardware.GetGPU();
            string mainboardName = string.Empty;
            string hardDriveSerial = string.Empty;
            string biosSerial = string.Empty;
            string ramSize = string.Empty;
            string processorId = string.Empty;
            string windowsProductKey = string.Empty;

            string query = String.Empty;
            _ = new ManagementObjectSearcher(query);
            query = "SELECT Product FROM Win32_BaseBoard";
            ManagementObjectSearcher searcher = new(query);

            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                mainboardName = obj["Product"].ToString();
                break;
            }

            query = "SELECT SerialNumber FROM Win32_DiskDrive WHERE InterfaceType='IDE' OR InterfaceType='SCSI' OR InterfaceType='SAS' OR InterfaceType='SATA'";
            searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                hardDriveSerial = obj["SerialNumber"].ToString();
                break;
            }

            query = "SELECT SerialNumber FROM Win32_BIOS";
            searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                biosSerial += obj["SerialNumber"].ToString();
                break;
            }

            query = "SELECT Capacity FROM Win32_PhysicalMemory";
            searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                ulong capacity = Convert.ToUInt64(obj["Capacity"]);
                ramSize += capacity.ToString() + ";";
            }

            query = "SELECT ProcessorId FROM Win32_Processor";
            searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                processorId = obj["ProcessorId"].ToString();
                break;
            }

            try
            {
                query = "SELECT ProductKey FROM SoftwareLicensingService";
                searcher = new ManagementObjectSearcher(query);

                foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
                {
                    windowsProductKey = obj["ProductKey"].ToString();
                    break;
                }
            }
            catch
            {
                windowsProductKey = null;
            }

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

            string hwid = cpuName + gpuName + mainboardName + hardDriveSerial + biosSerial + ramSize + processorId + windowsProductKey + macAddress;
            string salt = "reShut-";
            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(salt + hwid));
            StringBuilder sb = new();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));

            }
            return sb.ToString();
        }
    }
#pragma warning restore CA1416
}

