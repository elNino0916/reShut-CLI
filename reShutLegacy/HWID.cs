using System;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace reShutLegacy
{
#pragma warning disable CA1416
    // This class is no longer required and will be removed in the future.
    internal class HWID
    {
        public static string GetHWID()
        {
            var cpuName = Hardware.GetCPU();
            var gpuName = Hardware.GetGPU();
            var mainboardName = string.Empty;
            var hardDriveSerial = string.Empty;
            var biosSerial = string.Empty;
            var ramSize = string.Empty;
            var processorId = string.Empty;
            var windowsProductKey = string.Empty;

            var query = string.Empty;
            _ = new ManagementObjectSearcher(query);
            query = "SELECT Product FROM Win32_BaseBoard";
            ManagementObjectSearcher searcher = new(query);

            foreach (var obj in searcher.Get().Cast<ManagementObject>())
            {
                mainboardName = obj["Product"].ToString();
                break;
            }

            query = "SELECT SerialNumber FROM Win32_DiskDrive WHERE InterfaceType='IDE' OR InterfaceType='SCSI' OR InterfaceType='SAS' OR InterfaceType='SATA'";
            searcher = new ManagementObjectSearcher(query);

            foreach (var obj in searcher.Get().Cast<ManagementObject>())
            {
                hardDriveSerial = obj["SerialNumber"].ToString();
                break;
            }

            query = "SELECT SerialNumber FROM Win32_BIOS";
            searcher = new ManagementObjectSearcher(query);

            foreach (var obj in searcher.Get().Cast<ManagementObject>())
            {
                biosSerial += obj["SerialNumber"].ToString();
                break;
            }

            query = "SELECT Capacity FROM Win32_PhysicalMemory";
            searcher = new ManagementObjectSearcher(query);

            ramSize = searcher.Get().Cast<ManagementObject>().Select(obj => Convert.ToUInt64(obj["Capacity"])).Aggregate(ramSize, (current, capacity) => current + (capacity.ToString() + ";"));

            query = "SELECT ProcessorId FROM Win32_Processor";
            searcher = new ManagementObjectSearcher(query);

            foreach (var obj in searcher.Get().Cast<ManagementObject>())
            {
                processorId = obj["ProcessorId"].ToString();
                break;
            }

            try
            {
                query = "SELECT ProductKey FROM SoftwareLicensingService";
                searcher = new ManagementObjectSearcher(query);

                foreach (var obj in searcher.Get().Cast<ManagementObject>())
                {
                    windowsProductKey = obj["ProductKey"].ToString();
                    break;
                }
            }
            catch
            {
                windowsProductKey = null;
            }

            var nics = NetworkInterface.GetAllNetworkInterfaces();
            var macAddress = string.Empty;

            foreach (var adapter in nics)
            {
                if (macAddress != string.Empty) continue;
                var properties = adapter.GetIPProperties();
                macAddress = adapter.GetPhysicalAddress().ToString();
            }

            var hwid = cpuName + gpuName + mainboardName + hardDriveSerial + biosSerial + ramSize + processorId + windowsProductKey + macAddress;
            var salt = "reShut-";
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(salt + hwid));
            StringBuilder sb = new();
            foreach (var t in hashBytes)
            {
                sb.Append(t.ToString("x2"));
            }
            return sb.ToString();
        }
    }
#pragma warning restore CA1416
}

