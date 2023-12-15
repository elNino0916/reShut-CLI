using System;
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
        private static void MITLicense()
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
            string[] lines =
            [
        @"           ____  _           _     _                                ",
        @"  _ __ ___/ ___|| |__  _   _| |_  | |    ___  __ _  __ _  ___ _   _ ",
        @" | '__/ _ \___ \| '_ \| | | | __| | |   / _ \/ _` |/ _` |/ __| | | |",
        @" | | |  __/___) | | | | |_| | |_  | |__|  __/ (_| | (_| | (__| |_| |",
        @" |_|  \___|____/|_| |_|\__,_|\__| |_____\___|\__, |\__,_|\___|\__, |",
        @"                                             |___/            |___/ ",
            ];

            var maxLength = lines.Max(line => line.Length);
            var padding = (Console.WindowWidth - maxLength) / 2;

            foreach (var line in lines)
            {
                var centeredLine = new string(' ', padding) + line;
                Console.WriteLine(centeredLine);
            }

            string centeredText;

            if (Variables.prerelease)
            {
                centeredText = new string(' ', (Console.WindowWidth - "Pre-Release ".Length - Variables.fullversion.Length) / 2) + "Pre-Release " + Variables.fullversion;
            }
            else
            {
                centeredText = new string(' ', (Console.WindowWidth - Variables.fullversion.Length) / 2) + Variables.fullversion;
            }
            var copyright = new string(' ', (Console.WindowWidth - "Copyright (c) 2023 elNino0916".Length) / 2) + "Copyright (c) 2023 elNino0916";
            Console.WriteLine(centeredText);
            Console.WriteLine(copyright);
            Preload.Startup(true);
        }

        private static void Main(string[] args)
        {
            // Initializes config
            if (!Directory.Exists(@"config"))
            {
                Directory.CreateDirectory(@"config");
                File.WriteAllText(@"config\startup.cfg", "fast=disabled");
                File.WriteAllText(@"config\update.cfg", "search=enabled");

            }

            // Initializes cmdLine-args
            var cmdLine = new CmdLine(args);

        // Main UI starts here:
        start:
            // Sets UTF8 encoding for new design.
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Sets Title
            Console.Title = "reShut Legacy " + Variables.fullversion;

            // Prints ascii art
            Console.ForegroundColor = ConsoleColor.Red;
            CenterText();
            // Checks for updates
            if (File.ReadAllText(@"config\update.cfg") == "search=enabled")
            {
                try
                {
                    // Call the MainCheck method and display the result
                    UpdateChecker.MainCheck().Wait();
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine();
                    UpdateChecker.DisplayCenteredMessage($"Failed to check for updates.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
            else
            {
                // Updates are disabled
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine();
                UpdateChecker.DisplayCenteredMessage($"Update Search is disabled.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            // Prints main menu
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string[] menuItems = ["Shutdown", "Reboot", "Logoff", "Schedule...", "Settings", "Quit"];

            Console.WriteLine("╭────────────────────────╮");
            Console.WriteLine("│       Main Menu        │");
            Console.WriteLine("├────────────────────────┤");

            for (var i = 1; i < menuItems.Length - 1; i++)
            {
                Console.WriteLine("│ " + i + ") " + menuItems[i - 1].PadRight(20) + "│");
            }
            Console.WriteLine("├────────────────────────┤");
            Console.WriteLine("│ 9) " + menuItems[4].PadRight(20) + "│");
            Console.WriteLine("│ 0) " + menuItems[5].PadRight(20) + "│");

            Console.WriteLine("╰────────────────────────╯");

            // Gets the pressed key
            var keyInfo = Console.ReadKey();
            var key = keyInfo.KeyChar.ToString();

            // Checks and runs the requested action
            if (key.Equals("L", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.Clear();
                MITLicense();
                Console.WriteLine("\nPress any key to go back.");
                Console.ReadKey();
                Console.Clear();
                goto start;
            }

            if (key == "1")
            {
                // Shutdown
                Question();
                Console.ReadKey();
                Handler.Shutdown();
            }
            else if (key.Equals("E", StringComparison.CurrentCultureIgnoreCase))
            {
                // Emergency Shutdown
                Handler.Shutdown();
            }
            else switch (key)
            {
                case "2":
                    // Reboot
                    Question();
                    Console.ReadKey();
                    Handler.Reboot();
                    break;
                case "3":
                    // Logoff
                    Question();
                    Console.ReadKey();
                    Handler.Logoff();
                    break;
                case "9":
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
                    var setInfo = Console.ReadKey();
                    var set = setInfo.KeyChar.ToString();

                    switch (set)
                    {
                        // Go back
                        // About screen
                        case "9":
                            Console.Clear();
                            goto start;
                        // What's new screen
                        case "1":
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Clear();
                            Console.WriteLine("╭────────────────╮");
                            Console.WriteLine("│ Please wait... │");
                            Console.WriteLine("╰────────────────╯");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            var header = "reShut-Legacy " + Variables.fullversion;
                            var releaseStatus = "Pre-Release: " + Variables.prerelease;
                            const string systemInfoHeader = "System Information:";
                            var cpuInfo = "CPU: " + Preload.GetString(1);
                            var gpuInfo = "Video Controller: " + Preload.GetString(3);
                            var ramInfo = "RAM: " + Preload.GetString(2) + " GB";
                            var winVer = "Windows Version: " + Preload.GetString(0);
                          

                            Console.Clear();
                            var maxLength = Math.Max(header.Length, Math.Max(systemInfoHeader.Length, Math.Max(cpuInfo.Length, Math.Max(gpuInfo.Length, Math.Max(ramInfo.Length, Math.Max("The license can be viewed by pressing L in the main menu.".Length, winVer.Length))))));
                            var borderLength = maxLength + 4;

                            Console.WriteLine("╭" + new string('─', borderLength) + "╮");
                            Console.WriteLine("│ " + header.PadRight(borderLength - 2) + " │");
                            Console.WriteLine("│ " + "Copyright (c) 2023 elNino0916".PadRight(borderLength - 2) + " │");
                            Console.WriteLine("│ " + "The license can be viewed by pressing L in the main menu.".PadRight(borderLength - 2) + " │");
                            Console.WriteLine("│ " + releaseStatus.PadRight(borderLength - 2) + " │");
                            Console.WriteLine("├" + new string('─', borderLength) + "┤");
                            Console.WriteLine("│ " + systemInfoHeader.PadRight(borderLength - 2) + " │");
                            Console.WriteLine("│ " + cpuInfo.PadRight(borderLength - 2) + " │");
                            Console.WriteLine("│ " + gpuInfo.PadRight(borderLength - 2) + " │");
                            Console.WriteLine("│ " + ramInfo.PadRight(borderLength - 2) + " │"); 
                            Console.WriteLine("│ " + winVer.PadRight(borderLength - 2) + " │"); 
                            Console.WriteLine("╰" + new string('─', borderLength) + "╯");
                            Console.WriteLine("Press any key to go back.");
                            Console.ReadKey();
                            Console.Clear();
                            goto settings;
                        }
                        case "2":
                            Console.Clear();
                            Console.WriteLine("What´s new in v1:\n");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("New UI");
                            Console.WriteLine("The UI is now better then ever!");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("---");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("Startup Settings");
                            Console.WriteLine("\nMost of the things will now be loaded at startup! That means that you don't need to wait after the app launched.\nYou can change this in the settings by enabling Fast Startup.");
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
                        // Go back when invalid key has been pressed
                        case "3":
                        {
                            // NEW: Startup Settings
                            invalidstartup:
                            Console.Clear();

                            Console.Clear();
                            if (!File.Exists(@"config\startup.cfg"))
                            {
                                Directory.CreateDirectory("config");
                                File.WriteAllText(@"config\startup.cfg", String.Empty);
                                Console.ForegroundColor = ConsoleColor.Yellow;
                            }

                            if (!File.Exists(@"config\update.cfg")) File.WriteAllText(@"config\update.cfg", "search=enabled");
                            if (File.ReadAllText(@"config\startup.cfg") == "fast=disabled")
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("╭───────────────────────────╮");
                                Console.WriteLine("│ Fast Startup is disabled. │");
                                Console.WriteLine("╰───────────────────────────╯");
                            }
                            else
                            {
                                if (File.ReadAllText(@"config\startup.cfg") == "fast=enabled")
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("╭──────────────────────────╮");
                                    Console.WriteLine("│ Fast Startup is enabled. │");
                                    Console.WriteLine("╰──────────────────────────╯");
                                }
                            }
                            if (File.ReadAllText(@"config\update.cfg") == "search=disabled")
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("╭────────────────────────────╮");
                                Console.WriteLine("│ Update Search is disabled. │");
                                Console.WriteLine("╰────────────────────────────╯");
                            }
                            else
                            {
                                if (File.ReadAllText(@"config\update.cfg") == "search=enabled")
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("╭───────────────────────────╮");
                                    Console.WriteLine("│ Update Search is enabled. │");
                                    Console.WriteLine("╰───────────────────────────╯");
                                }
                            }
                            Console.ForegroundColor = ConsoleColor.DarkYellow; 
                            Console.WriteLine("╭───────────────────────────────────────╮");
                            Console.WriteLine("│                Startup:               │");
                            Console.WriteLine("├───────────────────────────────────────┤");
                            Console.WriteLine("│ 1) Enable Fast Startup                │");
                            Console.WriteLine("│ 2) Disable Fast Startup (recommended) │");
                            Console.WriteLine("├───────────────────────────────────────┤");
                            Console.WriteLine("│ 4) Enable Update Search (recommended) │");
                            Console.WriteLine("│ 5) Disable Update Search              │");
                            Console.WriteLine("├───────────────────────────────────────┤");
                            Console.WriteLine("│ 9) Back                               │");
                            Console.WriteLine("╰───────────────────────────────────────╯");
                            var setInfoS = Console.ReadKey();
                            var setS = setInfoS.KeyChar.ToString();
                            switch (setS)
                            {
                                case "1":
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("╭──────────────────────────────╮");
                                    Console.WriteLine("│ Successfully saved settings! │");
                                    Console.WriteLine("╰──────────────────────────────╯");
                                    File.WriteAllText(@"config\startup.cfg", "fast=enabled");
                                    goto invalidstartup;
                                case "9":
                                    Console.Clear();
                                    goto settings;
                                case "2":
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("╭──────────────────────────────╮");
                                    Console.WriteLine("│ Successfully saved settings! │");
                                    Console.WriteLine("╰──────────────────────────────╯");
                                    File.WriteAllText(@"config\startup.cfg", "fast=disabled");
                                    goto invalidstartup; 
                                case "4":
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("╭──────────────────────────────╮");
                                    Console.WriteLine("│ Successfully saved settings! │");
                                    Console.WriteLine("╰──────────────────────────────╯");
                                    File.WriteAllText(@"config\update.cfg", "search=enabled");
                                    goto invalidstartup;
                                case "5":
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("╭──────────────────────────────╮");
                                    Console.WriteLine("│ Successfully saved settings! │");
                                    Console.WriteLine("╰──────────────────────────────╯");
                                    File.WriteAllText(@"config\update.cfg", "search=disabled");
                                    goto invalidstartup;
                            }

                            goto invalidstartup;
                        }
                    }

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("╭───────────────────╮");
                    Console.WriteLine("│   Invalid input.  │");
                    Console.WriteLine("╰───────────────────╯");
                    Console.ForegroundColor = ConsoleColor.White;
                    goto settings;
                }
                // End of settings
                // Close app
                case "0":
                    Environment.Exit(0);
                    break;
                // Open Schedule Manager
                case "4":
                    Schedule.Plan();
                    goto start;
                // Invalid key pressed
                default:
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
