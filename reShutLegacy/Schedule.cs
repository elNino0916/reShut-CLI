using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reShutLegacy
{
    internal class Schedule
    {
        public static bool Plan()
        {
            Retry:
            // set strings
            // type:
            // shutdown
            // reboot
            string type = "";
            int input = 0;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"           ____  _           _     _                                ");
            Console.WriteLine(@"  _ __ ___/ ___|| |__  _   _| |_  | |    ___  __ _  __ _  ___ _   _ ");
            Console.WriteLine(@" | '__/ _ \___ \| '_ \| | | | __| | |   / _ \/ _` |/ _` |/ __| | | |");
            Console.WriteLine(@" | | |  __/___) | | | | |_| | |_  | |__|  __/ (_| | (_| | (__| |_| |");
            Console.WriteLine(@" |_|  \___|____/|_| |_|\__,_|\__| |_____\___|\__, |\__,_|\___|\__, |");
            Console.WriteLine(@"                                             |___/            |___/ ");
            Console.WriteLine(@"                                                            Schedule");
            Console.ForegroundColor= ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("---");
            Console.WriteLine("What do you want to schedule?");
            Console.WriteLine("Select an option:");
            Console.WriteLine("1) Shutdown");
            Console.WriteLine("2) Reboot");
            Console.WriteLine("-");
            Console.WriteLine("9) Cancel schedule");
            Console.WriteLine("0) Back");
            Console.WriteLine("---");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Input: ");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            string key = keyInfo.KeyChar.ToString();
            if (key == "9") 
            {
                type = "cancel";
                Process.Start(@"cmd.exe", "/c shutdown -a");
                Console.Clear();
                return false;
            }
            else

            if (key == "1")
            {
                type = "shutdown";
            }else if (key == "2")
            {
                type = "reboot";
            }else if (key == "0") 
            {
                type = "quit";
                Console.Clear();
                return false;
            }else if (key != "1" | key != "2" | key != "0") 
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("---");
                Console.WriteLine("An error occurred!");
                Console.WriteLine("---");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(8000);
                goto Retry;
            }
            // Phase 2
            Console.Clear();
            seconds:
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"           ____  _           _     _                                ");
            Console.WriteLine(@"  _ __ ___/ ___|| |__  _   _| |_  | |    ___  __ _  __ _  ___ _   _ ");
            Console.WriteLine(@" | '__/ _ \___ \| '_ \| | | | __| | |   / _ \/ _` |/ _` |/ __| | | |");
            Console.WriteLine(@" | | |  __/___) | | | | |_| | |_  | |__|  __/ (_| | (_| | (__| |_| |");
            Console.WriteLine(@" |_|  \___|____/|_| |_|\__,_|\__| |_____\___|\__, |\__,_|\___|\__, |");
            Console.WriteLine(@"                                             |___/            |___/ ");
            Console.WriteLine(@"                                                            Schedule");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" ");
            Console.WriteLine(" ");

            if (DateTime.Now.ToString("tt") == "")
            {
                Console.WriteLine("Current time: " + api.GetTime(true));
            }
            else 
            {
                if (DateTime.Now.ToString("tt") == "PM" | DateTime.Now.ToString("tt") == "AM")
                {
                    Console.WriteLine("Current time: " + api.GetTime(false));
                }
            }

            Console.WriteLine("---");
            Console.WriteLine("When do you want to run the scheduled " + type + "?");
            Console.WriteLine("Enter the time until the " + type + " in seconds.");
            Console.WriteLine("---");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Input: ");
            string inputStr = Console.ReadLine();

            if (!int.TryParse(inputStr, out input))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("---");
                Console.WriteLine("Error: Input must be a valid number.");
                Console.WriteLine("---");
                Thread.Sleep(8000);
                Console.Clear();
                goto seconds;
            }
            // Phase 3
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"           ____  _           _     _                                ");
            Console.WriteLine(@"  _ __ ___/ ___|| |__  _   _| |_  | |    ___  __ _  __ _  ___ _   _ ");
            Console.WriteLine(@" | '__/ _ \___ \| '_ \| | | | __| | |   / _ \/ _` |/ _` |/ __| | | |");
            Console.WriteLine(@" | | |  __/___) | | | | |_| | |_  | |__|  __/ (_| | (_| | (__| |_| |");
            Console.WriteLine(@" |_|  \___|____/|_| |_|\__,_|\__| |_____\___|\__, |\__,_|\___|\__, |");
            Console.WriteLine(@"                                             |___/            |___/ ");
            Console.WriteLine(@"                                                            Schedule");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("---");
            Console.WriteLine("You want to schedule a " + type + " in " + input + " seconds. Is that correct?");
            int minutes = input / 60;
            int hours = minutes / 60;
            Console.WriteLine("(" + minutes + "min / " + hours + "hrs)");
            Console.WriteLine("Select an option:");
            Console.WriteLine("1) Yes, schedule " + type);
            Console.WriteLine("2) No, go back and retry");
            Console.WriteLine("0) Back to main menu");
            Console.WriteLine("---");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Input: ");
            ConsoleKeyInfo keyInfo3 = Console.ReadKey();
            string ke3y = keyInfo3.KeyChar.ToString();
            if (ke3y == "0")
            {
                Console.Clear();
                return false;
            }
            else if (ke3y == "2")
            {
                Console.Clear();
                goto Retry;
            }
            else if (ke3y == "1")
            {
                Console.Clear();
                try
                {
                    string character = "";
                    if (type == "shutdown")
                    {
                        character = "s";
                    }
                    else
                    {
                        character = "r";
                    }
                    Process.Start(@"cmd.exe", "/c shutdown /"+ character + " /f /t " + input);
                    Console.ForegroundColor= ConsoleColor.Green;
                    Console.WriteLine(@"---");
                    Console.WriteLine("Successfully scheduled " + type + "!");
                    Console.WriteLine(@"---");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(8000);
                    return true;
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(@"---");
                    Console.WriteLine("An error occurred.");
                    Console.WriteLine(@"---");
                    Thread.Sleep(8000);
                    Console.ForegroundColor = ConsoleColor.White;
                    return false;
                }
                return false;
            }
            return false;

        }
    }
}
