using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace reShutCLI;
internal class Program
{
    private static void License()
    {
        Console.WriteLine("reShut CLI (version 1.0.5.0+) © 2023-2024 is now licensed under CC BY-NC-SA 4.0\n\nhttps://creativecommons.org/licenses/by-nc-sa/4.0");
    }

    private static void ConfirmationPrompt()
    {
        if (RegistryWorker.ReadFromRegistry("HKEY_CURRENT_USER\\Software\\elNino0916\\reShutCLI\\config", "SkipConfirmation") != "1")
        {

            Console.ForegroundColor = Variables.SecondaryColor;
            UpdateChecker.DisplayCenteredMessage("");
            UpdateChecker.DisplayCenteredMessage("╭──────────────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ Are you sure? Press any key to continue. │");
            UpdateChecker.DisplayCenteredMessage("╰──────────────────────────────────────────╯");
        }
    }

    public static void CenterText()
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
        var copyright = new string(' ', (Console.WindowWidth - "Copyright (c) 2023-2024 elNino0916".Length) / 2) +
                        "Copyright (c) 2023-2024 elNino0916";
        Console.WriteLine(centeredText);
        Console.WriteLine(copyright);
        Preload.Startup(true);
    }

    private static void Main(string[] args)
    {

        // Check for update (registry)
        RegInit.Populate(false);

        // Welcome Screen
        FirstTimeSetup.FirstStartup();

        // Update reShut Version in case of update:
        RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "reShutVersion", "STRING", Variables.version);

        // Initializes cmdLine-args
        var cmdLine = new CmdLine(args);

        Console.CursorVisible = false;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();

        // Loads the Users Theme
        ThemeLoader.loadTheme();

    // Main UI starts here:
    start:
        // Sets UTF8 encoding for new design.
        Console.OutputEncoding = Encoding.UTF8;
        // Sets Title
        Console.Title = "reShut CLI " + Variables.fullversion;

        // Checks if EULA is accepted
        if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EULAAccepted") == "0")
        {
            // Prompt EULA
            if (!EULAHost.Start() == true)
            {
                // Error handler needed
                Environment.Exit(0);
            }
        }


        // Prints ascii art
        Console.ForegroundColor = Variables.LogoColor;
        CenterText();
        // Checks for updates
        if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableUpdateSearch") == "1")
        {
            try
            {
                UpdateChecker.MainCheck().Wait();
            }
            catch
            {
                Console.ForegroundColor = Variables.SecondaryColor;
                UpdateChecker.DisplayCenteredMessage("Failed to check for updates. Check your network connection and try again.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        else
        {
            // Updates are disabled
            Console.ForegroundColor = Variables.SecondaryColor;
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
        UpdateChecker.DisplayCenteredMessage("╭────────────────────────╮");
        UpdateChecker.DisplayCenteredMessage("│       Main Menu        │");
        UpdateChecker.DisplayCenteredMessage("├────────────────────────┤");

        for (var i = 1; i < menuItems.Length - 1; i++)
            UpdateChecker.DisplayCenteredMessage("│ " + i + ") " + menuItems[i - 1].PadRight(20) + "│");
        UpdateChecker.DisplayCenteredMessage("├────────────────────────┤");
        UpdateChecker.DisplayCenteredMessage("│ 9) " + menuItems[4].PadRight(20) + "│");
        UpdateChecker.DisplayCenteredMessage("│ 0) " + menuItems[5].PadRight(20) + "│");

        UpdateChecker.DisplayCenteredMessage("╰────────────────────────╯");

        // Gets the pressed key
        var keyInfo = Console.ReadKey();
        var key = keyInfo.KeyChar.ToString();

        // Checks and runs the requested action
        if (key.Equals("L", StringComparison.CurrentCultureIgnoreCase))
        {
            Console.Clear();
            License();
            Console.WriteLine("\nPress any key to go back.");
            Console.ReadKey();
            Console.Clear();
            goto start;
        }

        if (key == "1")
        {
            // Shutdown
            ConfirmationPrompt();
            Console.ReadKey();
            Handler.Shutdown();
        }
        else
        {
            switch (key.ToLower())
            {
                case "2":
                    // Reboot
                    ConfirmationPrompt();
                    Console.ReadKey();
                    Handler.Reboot();
                    break;

                case "3":
                    // Logoff
                    ConfirmationPrompt();
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
                case "u":
                    if (Variables.isUpToDate) { Console.Clear(); goto start; }
                    Console.Clear();
                    Console.Title = "reShut CLI Updater";
                    Console.ForegroundColor = Variables.MenuColor;
                    UpdateChecker.DisplayCenteredMessage("Download started, Installer will open in a few seconds...");
                    Thread.Sleep(2000);
                    AutoUpdater.PerformUpdate();
                    Console.ReadLine();
                    Environment.Exit(0);
                    break;
                // Invalid key pressed
                default:
                    Console.Clear();
                    Console.ForegroundColor = Variables.SecondaryColor;
                    UpdateChecker.DisplayCenteredMessage("╭────────────────╮");
                    UpdateChecker.DisplayCenteredMessage("│ Invalid input. │");
                    UpdateChecker.DisplayCenteredMessage("╰────────────────╯");
                    Console.ForegroundColor = ConsoleColor.White;
                    goto start;
            }
        }
    }
}