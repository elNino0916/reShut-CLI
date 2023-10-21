using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace reShutLegacy
{
    internal class Program
    { 
        static void Main(string[] args)
        {
            // Command-Line Args (Disabled for 11.7
            //var cmdLine = new CmdLine(args);


        // Main UI
        start:
            Console.Title = "reShut Legacy " + variables.version;

                Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"           ____  _           _     _                                ");
            Console.WriteLine(@"  _ __ ___/ ___|| |__  _   _| |_  | |    ___  __ _  __ _  ___ _   _ ");
            Console.WriteLine(@" | '__/ _ \___ \| '_ \| | | | __| | |   / _ \/ _` |/ _` |/ __| | | |");
            Console.WriteLine(@" | | |  __/___) | | | | |_| | |_  | |__|  __/ (_| | (_| | (__| |_| |");
            Console.WriteLine(@" |_|  \___|____/|_| |_|\__,_|\__| |_____\___|\__, |\__,_|\___|\__, |");
            Console.WriteLine(@"                                             |___/            |___/ ");
            if (variables.prerelease == true)
            {
            Console.WriteLine(@"                                                Pre-Release " + variables.version);
            }
            else
            {
                Console.WriteLine(@"https://github.com/elNino0916/reShut-Legacy          reShut " + variables.version);
            }
            Console.ForegroundColor = ConsoleColor.Cyan; 
            Console.WriteLine("\n" + variables.motd);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("---");
            Console.WriteLine("1) Shutdown");
            Console.WriteLine("2) Reboot");
            Console.WriteLine("3) Logoff");
            Console.WriteLine("4) Schedule...");
            Console.WriteLine("9) Settings");
            Console.WriteLine("0) Quit");
            Console.WriteLine("---");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Input: ");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            string key = keyInfo.KeyChar.ToString();
            // This looks like shit. Fix later
            if (key == "1")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nAre you sure? Press any key to continue.");
                Console.ReadKey();
                Handler.Shutdown();
            }else if (key.ToUpper() == "E")
            {
                Handler.Shutdown();
            }
            else if (key == "2")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nAre you sure? Press any key to continue.");
                Console.ReadKey();
                Handler.Reboot();
            } else if (key == "3")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nAre you sure? Press any key to continue.");
                Console.ReadKey();
                Handler.Logoff();
            }else if (key == "9")
            {
            settings:
                // Settings
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("---");
                Console.WriteLine("Settings:");
                Console.WriteLine("1) About...");
                Console.WriteLine("2) Whats new");
                Console.WriteLine("9) Back");
                Console.WriteLine("---");
                Console.Write("Input: ");
                Console.ForegroundColor = ConsoleColor.White;
                ConsoleKeyInfo setInfo = Console.ReadKey();
                string set = setInfo.KeyChar.ToString();
                if (set == "9")
                {
                    Console.Clear();
                    goto start;
                
                } else if (set == "1")
                {
                    // About
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("--");
                    Console.WriteLine("reShut-Legacy " + variables.version + " | (c) 2023 elNino0916");
                    Console.WriteLine("Pre-Release: " + variables.prerelease);
                    Console.WriteLine("----");
                    Console.WriteLine("System Information:");
                    Console.WriteLine("CPU: " + Hardware.GetCPU() + " (" + Hardware.GetCPUID() + ")");
                    Console.WriteLine("GPU: " + Hardware.GetGPU());
                    Console.WriteLine("RAM Size (bytes): " + Hardware.GetRAM());

                    Console.WriteLine("----");
                    Console.WriteLine("Identifier: " + HWID.GetHWID());
                    Console.WriteLine("--");
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                    goto settings;
                }else if (set == "2")
                {
                    Console.Clear();
                    Console.WriteLine("What´s new:");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Use 'reshutlegacy.exe /<arg>'");
                    Console.WriteLine("Available args: 's','r','l','schedule'");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("---");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Emergency shutdown: Press 'E' in the main menu to start.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("\nPress any key to go back.");

                    Console.ReadKey();
                    goto settings;
                }
                else
                {
                    goto settings;
                }
            }
            else if (key == "0")
            {
                Environment.Exit(0);
            } else if (key == "4")
            {
                Schedule.Plan();
                goto start;
            }
            
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine(key + ") was not found.");
                Console.ForegroundColor = ConsoleColor.White;
                goto start;
            }

        }
    }
}
