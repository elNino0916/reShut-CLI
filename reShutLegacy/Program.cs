using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace reShutLegacy
{
    internal class Program
    {

        static void Main(string[] args)
        {
            // Setting parameters
            string version = "v.11.1.0";
            string startup = api.GetTime(true);

            // Main UI
            Console.Title = "reShut Legacy " + version;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"           ____  _           _     _                                ");
            Console.WriteLine(@"  _ __ ___/ ___|| |__  _   _| |_  | |    ___  __ _  __ _  ___ _   _ ");
            Console.WriteLine(@" | '__/ _ \___ \| '_ \| | | | __| | |   / _ \/ _` |/ _` |/ __| | | |");
            Console.WriteLine(@" | | |  __/___) | | | | |_| | |_  | |__|  __/ (_| | (_| | (__| |_| |");
            Console.WriteLine(@" |_|  \___|____/|_| |_|\__,_|\__| |_____\___|\__, |\__,_|\___|\__, |");
            Console.WriteLine(@"                                             |___/            |___/ ");
            Console.WriteLine(@"                                                     reShut " + version);
            Console.ForegroundColor = ConsoleColor.White;
            File.AppendAllText(@"reshut.log", "---" + Environment.NewLine);
            File.AppendAllText(@"reshut.log", "reShut " + version + Environment.NewLine); 
            File.AppendAllText(@"reshut.log", "---" + Environment.NewLine);
            File.AppendAllText(@"reshut.log", "System Information:" + Environment.NewLine);
            File.AppendAllText(@"reshut.log", api.GetCPU() + Environment.NewLine);
            File.AppendAllText(@"reshut.log", api.GetGPU() + Environment.NewLine);
            File.AppendAllText(@"reshut.log", api.GetMainboard() + Environment.NewLine);
            File.AppendAllText(@"reshut.log", api.GetCPUID() + Environment.NewLine);
            File.AppendAllText(@"reshut.log", "---" + Environment.NewLine);
            Console.WriteLine("\nWelcome, " + Environment.UserName + "!");
            Thread.Sleep(1000);
            start:
            Console.WriteLine("");
            Console.WriteLine("");
            File.AppendAllText(@"reshut.log", "Awaiting user input..." + Environment.NewLine);
            Console.ForegroundColor= ConsoleColor.Cyan;
            Console.WriteLine("---");
            Console.WriteLine("Select an option:");
            Console.WriteLine("1) Shutdown");
            Console.WriteLine("2) Reboot");
            Console.WriteLine("3) Logoff");
            Console.WriteLine("9) Settings");
            Console.WriteLine("0) Quit");
            Console.WriteLine("---");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Input: ");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            string key = keyInfo.KeyChar.ToString();
            File.AppendAllText(@"reshut.log", "Got input: " + key + Environment.NewLine);
            // This looks like shit. Fix later
            if (key == "1")
            {
                File.AppendAllText(@"reshut.log", "Shutting down..." + Environment.NewLine);
                Process.Start("shutdown", "/s /f /t 0");
            }
            else if (key == "2")
            {
                File.AppendAllText(@"reshut.log", "Restarting..." + Environment.NewLine);
                Process.Start("shutdown", "/r /f /t 0");
            } else if (key == "3")
            {
                File.AppendAllText(@"reshut.log", "Logging out..." + Environment.NewLine);
                Process.Start("shutdown", "/l");
            }else if (key == "9")
            {
                File.AppendAllText(@"reshut.log", "Opening settings menu..." + Environment.NewLine);
            settings:
                // Settings
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                File.AppendAllText(@"reshut.log", "Awaiting user input..." + Environment.NewLine);
                Console.WriteLine("---");
                Console.WriteLine("Settings:");
                Console.WriteLine("1) Clear log file");
                Console.WriteLine("2) About...");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("3) [Preview | Not working] Enable UI");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("9) Back");
                Console.WriteLine("---");
                Console.Write("Input: ");
                Console.ForegroundColor = ConsoleColor.White;
                ConsoleKeyInfo setInfo = Console.ReadKey();
                string set = setInfo.KeyChar.ToString();
                if (set == "9")
                {
                    File.AppendAllText(@"reshut.log", "Returning to main menu..." + Environment.NewLine);
                    Console.Clear();
                    goto start;
                } else if (set == "1")
                {
                    try
                    {
                        File.Delete(@"reshut.log");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nLog file has been cleared. Closing in 3 seconds");
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(3000);
                        Environment.Exit(0);
                    }
                    catch
                    {
                        File.AppendAllText(@"reshut.log", "Failed to delete log file." + Environment.NewLine);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nFailed to delete log file.");
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(3000);
                        goto settings;
                    }
                } else if (set == "2")
                {
                    // About
                    Console.Clear();
                    Console.WriteLine("--");
                    Console.WriteLine("reShut-Legacy " + version + " (c) 2023 elNino0916");
                    Console.WriteLine("Application startup time: " + startup);
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
                    Console.Clear();
                    Console.WriteLine("[!] This feature is a preview. It does not work yet.");
                    Thread.Sleep(5000);
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
            }else if (key == "0")
            {
                File.AppendAllText(@"reshut.log", "Exitting..." + Environment.NewLine);
            }
            else
            {
                File.AppendAllText(@"reshut.log", "Wrong input." + Environment.NewLine);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine(key + ") was not found. Please use 1, 2 or 3.");
                Console.ForegroundColor = ConsoleColor.White;
                goto start;
            }

        }
    }
}
