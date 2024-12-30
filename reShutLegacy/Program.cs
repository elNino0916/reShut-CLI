using System;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
namespace reShutCLI;
internal class Program
{
    
    private static void Main(string[] args)
    {
        InitializeApp(args);

        // Load languages
        CultureInfo culture = new CultureInfo(Variables.lang);
        ResourceManager rm = new ResourceManager("reShutCLI.Resources.Strings", typeof(Program).Assembly);

        Console.Title = "reShut CLI " + Variables.fullversion;

        // Checks if EULA is accepted
        if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EULAAccepted") == "0")
        {
            // Prompt EULA
            if (!ShowEULA.Start())
            {
                Environment.Exit(0);
            }
        }
        while (true)
        {

            // Prints ascii art
            PrintLogo();


            // Print the main menu
            string key = MainMenu();

            // Check the input
            CheckInput(key);
        }
    }

    
    private static void ConfirmationPrompt()
    {
        if (RegistryWorker.ReadFromRegistry("HKEY_CURRENT_USER\\Software\\elNino0916\\reShutCLI\\config", "SkipConfirmation") != "1")
        {
            CultureInfo culture = new CultureInfo(Variables.lang);
            ResourceManager rm = new ResourceManager("reShutCLI.Resources.Strings", typeof(Program).Assembly);

            // Get the translated string
            string confirmationText = rm.GetString("ConfirmationText", culture);

            // Calculate the maximum length (either the message or the box)
            int boxWidth = Math.Max(confirmationText.Length + 2, 44); // Ensure minimum width of 44
            string topBorder = "╭" + new string('─', boxWidth) + "╮";
            string bottomBorder = "╰" + new string('─', boxWidth) + "╯";

            // Center the message within the box
            int paddingLeft = (boxWidth - confirmationText.Length) / 2;
            string paddedMessage = "│" + new string(' ', paddingLeft) + confirmationText + new string(' ', boxWidth - confirmationText.Length - paddingLeft) + "│";

            // Center the entire box within the console window
            int windowWidth = Console.WindowWidth;

            // Print the confirmation message centered on the console
            Console.ForegroundColor = Variables.SecondaryColor;
            UpdateChecker.DisplayCenteredMessage("\n");
            UpdateChecker.DisplayCenteredMessage(topBorder);
            UpdateChecker.DisplayCenteredMessage(paddedMessage);
            UpdateChecker.DisplayCenteredMessage(bottomBorder);
        }
    }
    private static void License()
    {
        Console.WriteLine("The license is available at: https://creativecommons.org/licenses/by-nc-sa/4.0/legalcode.en");
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
        var copyright = new string(' ', (Console.WindowWidth - "Copyright © 2023-2025 elNino0916".Length) / 2) +
                        "Copyright © 2023-2025 elNino0916";
        Console.WriteLine(centeredText);
        Console.WriteLine(copyright);
    }
    private static string MainMenu()
    {
        CultureInfo culture = new CultureInfo(Variables.lang);
        ResourceManager rm = new ResourceManager("reShutCLI.Resources.Strings", typeof(Program).Assembly);
        Console.ForegroundColor = Variables.MenuColor;
        string[] menuItems = [rm.GetString("Shutdown", culture), rm.GetString("Reboot", culture), rm.GetString("Logoff", culture), rm.GetString("Schedule", culture), rm.GetString("Settings", culture), rm.GetString("Quit", culture)];
        UpdateChecker.DisplayCenteredMessage("╭────────────────────────╮");
        UpdateChecker.DisplayCenteredMessage($"│       {rm.GetString("MainMenu", culture)}        │"); // Has to be changed when more languages are added.
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
        return key;
    }
    
    private static void InitializeApp(string[] args)
    {
        // Required for UI
        Console.OutputEncoding = Encoding.UTF8;

        // Check for update (registry)
        RegInit.Populate(false);

        // Welcome Screen
        Setup.FirstStartup();

        // Update reShut Version in case of update:
        RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "reShutVersion", "STRING", Variables.version);

        // Initializes cmdLine-args
        var cmdLine = new CmdLine(args);

        Console.CursorVisible = false;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();

        // Loads the Users Theme
        ThemeLoader.loadTheme();
    }
    
    private static void PrintLogo()
    {
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
    }
    
    private static void CheckInput(string key)
    {
        CultureInfo culture = new CultureInfo(Variables.lang);
        ResourceManager rm = new ResourceManager("reShutCLI.Resources.Strings", typeof(Program).Assembly);

        switch (key.ToLower())
        {
            case "l":
                Console.Clear();
                License();
                string welcomeMessage = rm.GetString("PressAnyKeyToGoBack", culture);
                Console.WriteLine("\n" + welcomeMessage);
                Console.ReadKey();
                Console.Clear();
                break;

            case "1":
                // Shutdown
                ConfirmationPrompt();
                Console.ReadKey();
                Handler.Shutdown();
                break;
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
                break;
            // End of settings
            // Close app
            case "0":
                Environment.Exit(0);
                break;
            // Open Schedule Manager
            case "4":
                Schedule.Plan();
                break;
            case "u":
                if (Variables.isUpToDate) { Console.Clear(); return; }
                Console.Clear();
                Console.Title = "reShut CLI Updater";
                Console.ForegroundColor = Variables.MenuColor;
                UpdateChecker.DisplayCenteredMessage(rm.GetString("UpdateDLStarted", culture));
                Thread.Sleep(2000);
#pragma warning disable CS4014
                AutoUpdater.PerformUpdate();
#pragma warning restore CS4014
                Console.ReadLine();
                Environment.Exit(0);
                break;
            // Invalid key pressed
            default:
                Console.Clear();
                // Get the translated string
                string invalidText = rm.GetString("InvalidInput", culture);

                // Calculate the maximum length (either the message or the box)
                int boxWidth = Math.Max(invalidText.Length + 2, 44); // Ensure minimum width of 44
                string topBorder = "╭" + new string('─', boxWidth) + "╮";
                string bottomBorder = "╰" + new string('─', boxWidth) + "╯";

                // Center the message within the box
                int paddingLeft = (boxWidth - invalidText.Length) / 2;
                string paddedMessage = "│" + new string(' ', paddingLeft) + invalidText + new string(' ', boxWidth - invalidText.Length - paddingLeft) + "│";

                // Center the entire box within the console window
                int windowWidth = Console.WindowWidth;

                // Print the confirmation message centered on the console
                Console.ForegroundColor = Variables.SecondaryColor;
                UpdateChecker.DisplayCenteredMessage(topBorder);
                UpdateChecker.DisplayCenteredMessage(paddedMessage);
                UpdateChecker.DisplayCenteredMessage(bottomBorder);
                Console.ForegroundColor = ConsoleColor.White;
                break;

        }
    }
}