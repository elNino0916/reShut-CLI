﻿using System;
using System.IO;
using System.Linq;

namespace reShutLegacy
{
    /*
        MIT License

        Copyright (c) 2023 elNino0916

        Permission is hereby granted, free of charge, to any person obtaining a copy
        of this software and associated documentation files (the "Software"), to deal
        in the Software without restriction, including without limitation the rights
        to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
        copies of the Software, and to permit persons to whom the Software is
        furnished to do so, subject to the following conditions:

        The above copyright notice and this permission notice shall be included in all
        copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
        IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
        FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
        AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
        LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
        OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
        SOFTWARE.
    */
    internal class Program
    {
        static void MITLicense()
        {
            Console.WriteLine("MIT License\n\nCopyright (c) 2023 elNino0916\n\nPermission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:\n\nThe above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.\n\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.");
        }
        private static void Question()
        {
            // This is the confirmation prompt.
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("╭──────────────────────────────────────────╮");
            Console.WriteLine("│ Are you sure? Press any key to continue. │");
            Console.WriteLine("╰──────────────────────────────────────────╯");
        }
        private static void CenterText()
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

            if (Variables.prerelease)
            {
                centeredText = new string(' ', (Console.WindowWidth - "Pre-Release ".Length - Variables.version.Length) / 2) + "Pre-Release " + Variables.version;
            }
            else
            {
                centeredText = new string(' ', (Console.WindowWidth - Variables.version.Length) / 2) + Variables.version;
            }
            string copyright;
            copyright = new string(' ', (Console.WindowWidth - "Copyright (c) 2023 elNino0916".Length) / 2) + "Copyright (c) 2023 elNino0916";
            Console.WriteLine(centeredText);
            Console.WriteLine(copyright);
            Preload.Startup(true);
        }
        static void Main(string[] args)
        {
            // Initializes config
            if (!Directory.Exists(@"config"))
            {
                Directory.CreateDirectory(@"config");
                File.WriteAllText(@"config\startup.cfg", "fast=disabled");

            }


            // Initializes cmdLine-args
            var cmdLine = new CmdLine(args);

        // Main UI starts here:
        start:
            // Sets UTF8 encoding for new design.
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Sets Title
            Console.Title = "reShut Legacy " + Variables.version;

            // Prints ascii art
            Console.ForegroundColor = ConsoleColor.Red;
            CenterText();

            // Prints motd
            Console.ForegroundColor = ConsoleColor.DarkRed;
            string motdcenter = new string(' ', (Console.WindowWidth - Variables.motd().Length) / 2) + Variables.motd();
            Console.WriteLine("\n" + motdcenter + "\n");

            // Prints main menu
            Console.ForegroundColor = ConsoleColor.DarkYellow;
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
            if (key.ToUpper() == "L")
            {
                Console.Clear();
                MITLicense();
                Console.WriteLine("\nPress any key to go back");
                Console.ReadKey();
                Console.Clear();
                goto start;
            }
            else
            if (key == "1")
            {
                // Shutdown
                Question();
                Console.ReadKey();
                Handler.Shutdown();
            }
            else if (key.ToUpper() == "E")
            {
                // Emergency Shutdown
                Handler.Shutdown();
            }
            else if (key == "2")
            {
                // Reboot
                Question();
                Console.ReadKey();
                Handler.Reboot();
            }
            else if (key == "3")
            {
                // Logoff
                Question();
                Console.ReadKey();
                Handler.Logoff();
            }
            else if (key == "9")
            {
                Console.Clear();
            settings:
                // Settings

                // Prints the settings menu
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("╭───────────────────╮");
                Console.WriteLine("│     Settings:     │");
                Console.WriteLine("├───────────────────┤");
                Console.WriteLine("│ 1) About...       │");
                Console.WriteLine("│ 2) What's new     │");
                Console.WriteLine("│ 3) Startup        │");
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Clear();
                    Console.WriteLine("╭────────────────╮");
                    Console.WriteLine("│ Please wait... │");
                    Console.WriteLine("╰────────────────╯");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    string header = "reShut-Legacy " + Variables.version;
                    string releaseStatus = "Pre-Release: " + Variables.prerelease.ToString();
                    string systemInfoHeader = "System Information:";
                    string cpuInfo = "CPU: " + Preload.GetString(1);
                    string gpuInfo = "Video Controller: " + Preload.GetString(4);
                    string ramInfo = "RAM: " + Preload.GetString(3) + " GB";
                    string winVer = "Windows Version: " + Preload.GetString(0);
                    string identifierInfo = "Identifier: " + Preload.GetString(2);

                    int maxLength = Math.Max(header.Length, Math.Max(releaseStatus.Length, Math.Max(cpuInfo.Length, Math.Max(gpuInfo.Length, Math.Max(ramInfo.Length, identifierInfo.Length)))));
                    int borderLength = maxLength + 4;
                    Console.Clear();

                    Console.WriteLine("╭" + new string('─', borderLength) + "╮");
                    Console.WriteLine("│ " + header.PadRight(maxLength) + "   │");
                    Console.WriteLine("│ Copyright (c) 2023 elNino0916".PadLeft(19).PadRight(maxLength) + "     │");
                    Console.WriteLine("│ The license can be viewed by pressing L in the main menu.".PadLeft(19).PadRight(maxLength) + "   │");
                    Console.WriteLine("│ " + releaseStatus.PadRight(maxLength) + "   │");
                    Console.WriteLine("├" + new string('─', borderLength) + "┤");
                    Console.WriteLine("│" + systemInfoHeader.PadLeft(20).PadRight(maxLength) + "    │");
                    Console.WriteLine("│ " + cpuInfo.PadLeft(18).PadRight(maxLength) + "   │");
                    Console.WriteLine("│ " + gpuInfo.PadLeft(18).PadRight(maxLength) + "   │");
                    Console.WriteLine("│ " + ramInfo.PadLeft(10).PadRight(maxLength) + "   │");
                    Console.WriteLine("│ " + winVer.PadLeft(10).PadRight(maxLength) + "   │");
                    Console.WriteLine("├" + new string('─', borderLength) + "┤");
                    Console.WriteLine("│ " + identifierInfo.PadLeft(15).PadRight(maxLength) + "   │");
                    Console.WriteLine("╰" + new string('─', borderLength) + "╯");


                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                    Console.Clear();
                    goto settings;
                }

                // Whats new screen
                else if (set == "2")
                {
                    Console.Clear();
                    Console.WriteLine("What´s new in v12:\n");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("New UI");
                    Console.WriteLine("The UI is now better then ever!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("---");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Startup Settings");
                    Console.WriteLine("\nMost of the things will now be loaded at startup! That means that you dont need to wait after the app launched.\nYou can change this in the settings by enabling Fast Startup.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("---");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Internal changes");
                    Console.WriteLine("reShut Legacy now uses .NET 8 instead of .NET Framework 4.8. Because of that, more files are needed to run reShut Legacy. The user experience will not change because of that.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("---");
                    Console.WriteLine("\nPress any key to go back.");
                    Console.ReadKey();
                    Console.Clear();
                    goto settings;
                }
                else if (set == "3")
                {
                // NEW: Startup Settings
                invalidstartup:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("╭────────────────────────────────────────────╮");
                    Console.WriteLine("│ Reading configuration file, please wait... │");
                    Console.WriteLine("╰────────────────────────────────────────────╯");
                    Console.Clear();
                    if (File.Exists(@"config\startup.cfg"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("╭───────────────────────────────────────╮");
                        Console.WriteLine("│ Successfully read configuration file. │");
                        Console.WriteLine("╰───────────────────────────────────────╯");
                    }
                    else
                    {
                        Directory.CreateDirectory("config");
                        File.WriteAllText(@"config\startup.cfg", String.Empty);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("╭──────────────────────────────────────────╮");
                        Console.WriteLine("│ Successfully created configuration file. │");
                        Console.WriteLine("╰──────────────────────────────────────────╯");
                    }
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("╭────────────────────────────────────────────────╮");
                    Console.WriteLine("│ reShut now loads most of the values at launch. │");
                    Console.WriteLine("╰────────────────────────────────────────────────╯");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    if (File.ReadAllText(@"config\startup.cfg") == "fast=disabled")
                    {
                        Console.WriteLine("╭───────────────────────────╮");
                        Console.WriteLine("│ Fast Startup is disabled. │");
                        Console.WriteLine("╰───────────────────────────╯");
                    }
                    else
                    {
                        if (File.ReadAllText(@"config\startup.cfg") == "fast=enabled")
                        {
                            Console.WriteLine("╭──────────────────────────╮");
                            Console.WriteLine("│ Fast Startup is enabled. │");
                            Console.WriteLine("╰──────────────────────────╯");
                        }
                    }
                    Console.WriteLine("╭───────────────────────────────────────╮");
                    Console.WriteLine("│                Startup:               │");
                    Console.WriteLine("├───────────────────────────────────────┤");
                    Console.WriteLine("│ 1) Enable Fast Startup                │");
                    Console.WriteLine("│ 2) Disable Fast Startup (recommended) │");
                    Console.WriteLine("│ 9) Back                               │");
                    Console.WriteLine("╰───────────────────────────────────────╯");
                    ConsoleKeyInfo setInfoS = Console.ReadKey();
                    string setS = setInfoS.KeyChar.ToString();
                    if (setS == "1")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("╭──────────────────────────────╮");
                        Console.WriteLine("│ Successfully saved settings! │");
                        Console.WriteLine("╰──────────────────────────────╯");
                        File.WriteAllText(@"config\startup.cfg", "fast=enabled");
                        goto invalidstartup;
                    }
                    if (setS == "9")
                    {
                        Console.Clear();
                        goto settings;
                    }
                    if (setS == "2")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("╭──────────────────────────────╮");
                        Console.WriteLine("│ Successfully saved settings! │");
                        Console.WriteLine("╰──────────────────────────────╯");
                        File.WriteAllText(@"config\startup.cfg", "fast=disabled");
                        goto invalidstartup;
                    }
                    goto invalidstartup;
                }
                // Go back when invalid key has been pressed
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("╭───────────────────╮");
                    Console.WriteLine("│   Invalid input.  │");
                    Console.WriteLine("╰───────────────────╯");
                    Console.ForegroundColor = ConsoleColor.White;
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
                Console.WriteLine("│   Invalid input.  │");
                Console.WriteLine("╰───────────────────╯");
                Console.ForegroundColor = ConsoleColor.White;
                goto start;
            }

        }
    }
}
