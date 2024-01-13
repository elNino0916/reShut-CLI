using System;
using System.IO;
using System.Linq;
using System.Text;

namespace reShutCLI;
internal class Program
{
    private static void MITLicense()
    {
        Console.WriteLine("MIT License\n\nCopyright (c) 2024 elNino0916\n\nPermission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:\n\nThe above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.\n\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.");
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
            @"           ____  _           _      ____ _     ___ ",
            @"  _ __ ___/ ___|| |__  _   _| |_   / ___| |   |_ _|",
            @" | '__/ _ \___ \| '_ \| | | | __| | |   | |    | | ",
            @" | | |  __/___) | | | | |_| | |_  | |___| |___ | | ",
            @" |_|  \___|____/|_| |_|\__,_|\__|  \____|_____|___|",
            @"                                                   "
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
            centeredText =
                new string(' ', (Console.WindowWidth - "Pre-Release ".Length - Variables.fullversion.Length) / 2) +
                "Pre-Release " + Variables.fullversion;
        else
            centeredText = new string(' ', (Console.WindowWidth - Variables.fullversion.Length) / 2) +
                           Variables.fullversion;
        var copyright = new string(' ', (Console.WindowWidth - "Copyright (c) 2024 elNino0916".Length) / 2) +
                        "Copyright (c) 2024 elNino0916";
        Console.WriteLine(centeredText);
        Console.WriteLine(copyright);
        Preload.Startup(true);
    }

    private static void Main(string[] args)
    {
        // Check locks etc.
        Lock.Years();



        // Initializes config
        if (!Directory.Exists(@"config"))
        {
            Directory.CreateDirectory(@"config");
            File.WriteAllText(@"config\startup.cfg", "fast=disabled");
            File.WriteAllText(@"config\update.cfg", "search=enabled");
        }

        // Initializes cmdLine-args
        var cmdLine = new CmdLine(args);

        Console.CursorVisible = false;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();
    // Main UI starts here:
    start:
        // Sets UTF8 encoding for new design.
        Console.OutputEncoding = Encoding.UTF8;
        // Sets Title
        Console.Title = "reShut CLI " + Variables.fullversion;

        // Prints ascii art
        Console.ForegroundColor = Variables.LogoColor;
        CenterText();
        // Checks for updates
        if (File.ReadAllText(@"config\update.cfg") == "search=enabled")
        {
            try
            {
                UpdateChecker.MainCheck().Wait();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                UpdateChecker.DisplayCenteredMessage("Failed to check for updates.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        else
        {
            // Updates are disabled
            Console.ForegroundColor = ConsoleColor.Magenta;
            UpdateChecker.DisplayCenteredMessage("Update Search is disabled.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        // Prints main menu

        var consoleColors = ((ConsoleColor[])Enum.GetValues(typeof(ConsoleColor)))
            .Where(color => color != ConsoleColor.Black)
            .ToArray();
        Random random = new();
        var randomIndex = random.Next(consoleColors.Length);
        Console.ForegroundColor = Variables.MenuColor;
        string[] menuItems = ["Shutdown", "Reboot", "Logoff", "Schedule...", "Settings", "Quit"];

        Console.WriteLine("╭────────────────────────╮");
        Console.WriteLine("│       Main Menu        │");
        Console.WriteLine("├────────────────────────┤");

        for (var i = 1; i < menuItems.Length - 1; i++)
            Console.WriteLine("│ " + i + ") " + menuItems[i - 1].PadRight(20) + "│");
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
        else
        {
            switch (key)
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
                    Settings.Show();
                    goto start;
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