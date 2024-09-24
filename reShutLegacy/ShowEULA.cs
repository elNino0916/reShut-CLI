using System;
using System.Runtime.Versioning;
using System.Threading;

namespace reShutCLI
{
    internal class ShowEULA
    {
        [SupportedOSPlatform("windows")]
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
            Console.WriteLine("1. Updates");
            Console.WriteLine("   ---------");
            Console.WriteLine("   This software checks for updates on GitHub, which can be disabled in the settings.");
            Console.WriteLine("   Updates are provided via GitHub to ensure you have the latest features and security patches.");
            Console.WriteLine();
            Console.WriteLine("2. Disclaimer of Liability");
            Console.WriteLine("   -------------------------");
            Console.WriteLine("   The authors are not responsible for any data loss or damages that may occur while using this software.");
            Console.WriteLine();
            Console.WriteLine("3. Acceptance");
            Console.WriteLine("   ------------");
            Console.WriteLine("   By accepting this agreement, you agree to the terms and conditions stated above.");
            Console.WriteLine("=================================");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Press (1) to accept these terms or (2) to decline.");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.D1 || keyInfo.Key == ConsoleKey.NumPad1)
            {
                EULAAccepted = true;
            }
            else if (keyInfo.Key == ConsoleKey.D2 || keyInfo.Key == ConsoleKey.NumPad2)
            {
                EULAAccepted = false;
            }
            else
            {
                goto Retry;
            }
            if (EULAAccepted)
            {
                Console.Clear();
                RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EULAAccepted", "STRING", "1");
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
                return false;
            }
        }
    }
}
