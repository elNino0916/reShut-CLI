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
        static void question()
        {
            // This is the confirmation prompt.
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("╭──────────────────────────────────────────╮");
            Console.WriteLine("│ Are you sure? Press any key to continue. │");
            Console.WriteLine("╰──────────────────────────────────────────╯");
        }
        static void CenterText()
        {
            // This is the ascii art that is printed at the top.
            string[] lines = new string[]
            {
        @"           ____  _           _     _                                ",
        @"  _ __ ___/ ___|| |__  _   _| |_  | |    ___  __ _  __ _  ___ _   _ ",
        @" | '__/ _ \___ \| '_ \| | | | __| | |   / _ \/ _` |/ _` |/ __| | | |",
        @" | | |  __/___) | | | | |_| | |_  | |__|  __/ (_| | (_| | (__| |_| |",
        @" |_|  \___|____/|_| |_|\__,_|\__| |_____\___|\__, |\__,_|\___|\__, |",
        @"                                             |___/            |___/ ",
            };

            int maxLength = lines.Max(line => line.Length);
            int padding = (Console.WindowWidth - maxLength) / 2;

            foreach (string line in lines)
            {
                string centeredLine = new string(' ', padding) + line;
                Console.WriteLine(centeredLine);
            }

            string centeredText;

            if (variables.prerelease == true)
            {
                centeredText = new string(' ', (Console.WindowWidth - "Pre-Release ".Length - variables.version.Length) / 2) + "Pre-Release " + variables.version;
            }
            else
            {
                centeredText = new string(' ', (Console.WindowWidth - variables.version.Length) / 2) + variables.version;
            }

            Console.WriteLine(centeredText);
        }
        static void Main(string[] args)
        {
            // Initializes cmdLine-args
            var cmdLine = new CmdLine(args);


        // Main UI starts here:
        start:
            // Sets UTF8 encoding for new design.
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Sets Title
            Console.Title = "reShut Legacy " + variables.version;

            // Prints ascii art
            Console.ForegroundColor = ConsoleColor.Blue;
            CenterText();

            // Prints motd
            Console.ForegroundColor = ConsoleColor.Cyan; 
            string motdcenter = new string(' ', (Console.WindowWidth - variables.motd.Length) / 2) + variables.motd;
            Console.WriteLine("\n" + motdcenter + "\n");

            // Prints main menu
            Console.ForegroundColor = ConsoleColor.Magenta;
            string[] menuItems = { "Shutdown", "Reboot", "Logoff", "Schedule...", "Settings", "Quit" };
             
            Console.WriteLine("╭────────────────────────╮");
            Console.WriteLine("│       Main Menu        │");
            Console.WriteLine("├────────────────────────┤");

            for (int i = 1; i < menuItems.Length - 1; i++)
            {
                Console.WriteLine("│ " + i + ") " + menuItems[i - 1].PadRight(20) + "│");
            }
            Console.WriteLine("├────────────────────────┤");
            Console.WriteLine("│ 9) " + menuItems[4].PadRight(20) + "│");
            Console.WriteLine("│ 0) " + menuItems[5].PadRight(20) + "│");

            Console.WriteLine("╰────────────────────────╯");

            // Gets the pressed key
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            string key = keyInfo.KeyChar.ToString();

            // Checks and runs the requested action
            if (key == "1")
            {
                // Shutdown
                question();
                Console.ReadKey();
                Handler.Shutdown();
            }else if (key.ToUpper() == "E")
            {
                // Emergency Shutdown
                Handler.Shutdown();
            }
            else if (key == "2")
            {
                // Reboot
                question();
                Console.ReadKey();
                Handler.Reboot();
            } else if (key == "3")
            {
                // Logoff
                question();
                Console.ReadKey();
                Handler.Logoff();
            }else if (key == "9")
            {
            settings:
                // Settings
                Console.Clear();

                // Prints the settings menu
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("╭───────────────────╮");
                Console.WriteLine("│     Settings:     │");
                Console.WriteLine("├───────────────────┤");
                Console.WriteLine("│ 1) About...       │");
                Console.WriteLine("│ 2) Whats new      │");
                Console.WriteLine("│ 9) Back           │");
                Console.WriteLine("╰───────────────────╯");
                Console.ForegroundColor = ConsoleColor.White;
                // Key detect
                ConsoleKeyInfo setInfo = Console.ReadKey();
                string set = setInfo.KeyChar.ToString();

                // Go back
                if (set == "9")
                {

                    Console.Clear();
                    goto start;
                
                }

                // About screen
                else if (set == "1")
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    string header = "reShut-Legacy " + variables.version;
                    string releaseStatus = "Pre-Release: " + variables.prerelease.ToString();
                    string systemInfoHeader = "System Information:";
                    string cpuInfo = "CPU: " + Hardware.GetCPU() + " (" + Hardware.GetCPUID() + ")";
                    string gpuInfo = "GPU: " + Hardware.GetGPU();
                    string ramInfo = "RAM Size (bytes): " + Hardware.GetRAM().ToString();
                    string identifierInfo = "Identifier: " + HWID.GetHWID();

                    int maxLength = Math.Max(header.Length, Math.Max(releaseStatus.Length, Math.Max(cpuInfo.Length, Math.Max(gpuInfo.Length, Math.Max(ramInfo.Length, identifierInfo.Length)))));
                    int borderLength = maxLength + 4;

                    Console.WriteLine("╭" + new string('─', borderLength) + "╮");
                    Console.WriteLine("│ " + header.PadRight(maxLength) + "   │");
                    Console.WriteLine("│ (c) 2023 elNino0916".PadLeft(19).PadRight(maxLength) + "     │");
                    Console.WriteLine("│ " + releaseStatus.PadRight(maxLength) + "   │");
                    Console.WriteLine("├" + new string('─', borderLength) + "┤");
                    Console.WriteLine("│" + systemInfoHeader.PadLeft(20).PadRight(maxLength) + "    │");
                    Console.WriteLine("│ " + cpuInfo.PadLeft(18).PadRight(maxLength) + "   │");
                    Console.WriteLine("│ " + gpuInfo.PadLeft(18).PadRight(maxLength) + "   │");
                    Console.WriteLine("│ " + ramInfo.PadLeft(10).PadRight(maxLength) + "   │");
                    Console.WriteLine("├" + new string('─', borderLength) + "┤");
                    Console.WriteLine("│ " + identifierInfo.PadLeft(15).PadRight(maxLength) + "   │");
                    Console.WriteLine("╰" + new string('─', borderLength) + "╯");

                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                    goto settings;
                }

                // Whats new screen
                else if (set == "2")
                {
                    Console.Clear();
                    Console.WriteLine("What´s new:");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                     Console.WriteLine("cmd-line arguments");
                     Console.WriteLine("More information: 'reShutLegacy.exe -help'");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("---");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Emergency shutdown: Press 'E' in the main menu to start.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("\nPress any key to go back.");

                    Console.ReadKey();
                    goto settings;
                }

                // Go back when invalid key has been pressed
                else
                {
                    goto settings;
                }
            }
            // End of settings

            // Close app
            else if (key == "0")
            {
                Environment.Exit(0);
            } 
            
            // Open Schedule Manager
            else if (key == "4")
            {
                Schedule.Plan();
                goto start;
            }
            
            // Invalid key pressed
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("╭───────────────────╮");
                Console.WriteLine("│ " + key + ") was not found. │");
                Console.WriteLine("╰───────────────────╯");
                Console.ForegroundColor = ConsoleColor.White;
                goto start;
            }

        }
    }
}
