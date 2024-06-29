using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace reShutCLI
{
    internal class EULAHost
    {
        public static bool Start()
        {
            Retry:
            bool EULAAccepted = false;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.WriteLine("You have to accept this EULA to use reShut CLI. \n");
            Console.WriteLine("End User License Agreement (EULA)");
            Console.WriteLine("=================================");
            Console.WriteLine();
            Console.WriteLine("Please read the following terms and conditions carefully before using this software.");
            Console.WriteLine();
            Console.WriteLine("1. Data Processing");
            Console.WriteLine("   -----------------");
            Console.WriteLine("   System information is processed on device to ensure optimal performance and functionality.");
            Console.WriteLine();
            Console.WriteLine("2. Updates");
            Console.WriteLine("   ---------");
            Console.WriteLine("   This software checks for updates on GitHub, which can be disabled in the settings.");
            Console.WriteLine("   Updates are provided via GitHub to ensure you have the latest features and security patches.");
            Console.WriteLine();
            Console.WriteLine("3. Disclaimer of Liability");
            Console.WriteLine("   -------------------------");
            Console.WriteLine("   The authors are not responsible for any data loss or damages that may occur while using this software.");
            Console.WriteLine();
            Console.WriteLine("4. Acceptance");
            Console.WriteLine("   ------------");
            Console.WriteLine("   By accepting this agreement, you agree to the terms and conditions stated above.");
            Console.WriteLine("=================================");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Press (1) to accept these terms or (9) to decline.");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key.ToString() == "1")
            {
                EULAAccepted = true;
            }
            else if (keyInfo.Key.ToString() == "9")
            {
                EULAAccepted = false;
            }
            else
            {
                goto Retry;
            }
            if (EULAAccepted)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Clear();
                Console.WriteLine("Thank you for accepting these terms, reShut CLI will start.");
                RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EULAAccepted", "STRING", "1");
                Thread.Sleep(2000);
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Clear();
                Console.WriteLine("You have to accept this EULA in order to use reShut CLI.");
                RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EULAAccepted", "STRING", "0");
                Thread.Sleep(2000);
                Environment.Exit(0);
            }
            return false;
        }
    }
}
