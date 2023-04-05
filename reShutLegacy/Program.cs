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
        static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debuggear.Check();
            }
        }

        static void Main(string[] args)
        {
            // check for debugger

            // setup timer
            // Create a timer with a 1 second interval
            var timer = new System.Timers.Timer(1);

            // Hook up the Elapsed event
            timer.Elapsed += OnTimerElapsed;

            // Start the timer
            timer.Start();

        // Main UI
        start:
            Console.Title = "reShut Legacy " + variables.version;
            if (variables.buildfromsource == true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
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
            if (variables.buildfromsource == true)
            {
                Console.WriteLine(@"You are using an version built using the source.");
            }
            else
            {
                Console.WriteLine("You are using an pre-compiled build, good!");
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow; 
            Console.WriteLine("\nThe 'Secure' update");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.ForegroundColor= ConsoleColor.Cyan;
            Console.WriteLine("---");
            Console.WriteLine("Select an option:");
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
                Process.Start("shutdown", "/s /f /t 0");
            }
            else if (key == "2")
            {
                Process.Start("shutdown", "/r /f /t 0");
            } else if (key == "3")
            {
                Process.Start("shutdown", "/l");
            }else if (key == "9")
            {
            settings:
                // Settings
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("---");
                Console.WriteLine("Settings:");
                Console.WriteLine("1) About...");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("2) [Preview ended | Disabled] Enable UI");
                Console.ForegroundColor = ConsoleColor.Cyan;
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
                
                } else if (set == "2")
                {
                    // About
                    Console.Clear();
                    Console.WriteLine("--");
                    Console.WriteLine("reShut-Legacy " + variables.version + " (c) 2023 elNino0916");
                    Console.WriteLine("Pre-Release: " + variables.prerelease);
                    Console.WriteLine("Current Time: " + api.GetTime(true));
                    Console.WriteLine("----");
                    Console.WriteLine("System Information:");
                    Console.WriteLine("CPU: " + api.GetCPU());
                    Console.WriteLine("CPU ID: " + api.GetCPUID());
                    Console.WriteLine("GPU: " + api.GetGPU());
                    Console.WriteLine("RAM Size (bytes): " + api.GetRAM());

                    Console.WriteLine("----");
                    Console.WriteLine("HWID (experimental): " + HWID.GetHWID());
                    Console.WriteLine("--");
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                    goto settings;
                }
            else if (set == "3")
                {
                    // UI
                    // INSTRUCTIONS HOW TO ENABLE THIS FEATURE
                    // Remove all lines below that end with //rm
                    // I do not recommend enabling this feature, its WIP
                    Console.Clear();
                    Console.WriteLine("[!] This feature is now disabled. Version 11.1.0 and 11.1.2 are the only versions with the preview for now.");
                    Console.WriteLine("[?] You need to modify the source code to open it anyways.");
                    Thread.Sleep(8000); //rm
                    goto settings; //rm
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("--");
                    Console.WriteLine("UI Mode is enabled. Close the UI to continue in the console.");
                    Console.WriteLine("--");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    var guiForm = new testui();

                    guiForm.ShowDialog();//This "opens" the GUI
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
            }else if (key == "4")
            {
                Schedule.Plan();
                goto start;
            }
            else
            {
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
