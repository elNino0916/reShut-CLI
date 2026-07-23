using System.Runtime.InteropServices;
using System.Text;
using reShutCLI.Helpers;
using reShutCLI.Services;

namespace reShutCLI;

/// <summary>
/// Main class for the reShut CLI application.
/// Handles application initialization, main loop, UI rendering, and input processing.
/// </summary>
internal static partial class Program
{
    private const int STD_OUTPUT_HANDLE = -11;
    private const int ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

    [LibraryImport("kernel32.dll")]
    private static partial IntPtr GetStdHandle(int nStdHandle);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetConsoleMode(IntPtr hConsoleHandle, out int mode);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

    public static void Main(string[] args)
    {
        Console.Clear();
        InitializeApp(args);

        Console.Title = Localization.Get("ConsoleTitle") + " " + Variables.FullVersion;

        // Checks if EULA is accepted
        if (RegistryWorker.ReadFromRegistry(Constants.RegistryPathConfig, Constants.RegistryValueEulaAccepted) == Constants.EulaNotAcceptedValue
            && !ShowEULA.Start())
        {
            Environment.Exit(Constants.ExitCodeSuccess);
        }

        while (true)
        {
            Console.Title = Localization.Get("ConsoleTitle") + " " + Variables.FullVersion;
            PrintLogo();
            CheckInput(MainMenu());
        }
    }

    /// <summary>
    /// Performs initial setup: console configuration, registry defaults, first-run
    /// setup, version bookkeeping, command-line parsing and theme loading.
    /// </summary>
    private static void InitializeApp(string[] args)
    {
        // Set console output to UTF8 to support box drawing characters.
        Console.OutputEncoding = Encoding.UTF8;
        EnableVirtualTerminal();

        // Initialize or update registry entries, false means not forced.
        RegInit.Populate(false);

        // Perform first startup routines (e.g., EULA display).
        Setup.FirstStartup();

        // Update the stored application version in the registry.
        RegistryWorker.WriteToRegistry(Constants.RegistryPathBase, Constants.RegistryValueReShutVersion, Constants.RegistryValueTypeString, Variables.Version);

        // Process any command-line arguments.
        _ = new CmdLine(args);

        // Configure console appearance.
        Console.CursorVisible = false;
        UIDraw.BackgroundColor = ConsoleColor.Black;
        Console.Clear();

        // Load user-selected theme.
        ThemeLoader.LoadTheme();
        if (string.IsNullOrEmpty(RegistryWorker.ReadFromRegistry(Constants.RegistryPathBase, Constants.RegistryValueDefaultThemeRecentName)))
        {
            RegistryWorker.WriteToRegistry(Constants.RegistryPathBase, Constants.RegistryValueDefaultThemeRecentName, Constants.RegistryValueTypeString, Variables.UpdatedDefaultThemeName);
        }
    }

    /// <summary>Enables ANSI escape sequence processing for 24-bit theme colors.</summary>
    private static void EnableVirtualTerminal()
    {
        var handle = GetStdHandle(STD_OUTPUT_HANDLE);
        if (GetConsoleMode(handle, out var mode))
        {
            SetConsoleMode(handle, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
        }
    }

    /// <summary>
    /// Centers and displays the ASCII art logo, application version, and copyright information.
    /// </summary>
    public static void CenterText()
    {
        if (System.Diagnostics.Debugger.IsAttached)
        {
            UIDraw.TextColor = Variables.SecondaryColor;
            UIDraw.DrawCenteredLine("Debugger attached!");
        }

        UIDraw.TextColor = Variables.LogoColor;
        string[] lines =
        [
            @"          __ _           _        ___   __   _____ ",
            @" _ __ ___/ _\ |__  _   _| |_     / __\ / /   \_   \",
            @"| '__/ _ \ \| '_ \| | | | __|   / /   / /     / /\/",
            @"| | |  __/\ \ | | | |_| | |_   / /___/ /___/\/ /_  ",
            @"|_|  \___\__/_| |_|\__,_|\__|  \____/\____/\____/  ",
            @"                                                   "
        ];

        foreach (var line in lines)
        {
            UIDraw.DrawCenteredLine(line);
        }

        var versionText = Variables.IsPreRelease
            ? Localization.Get("PreRelease") + Variables.FullVersion
            : Variables.FullVersion;

        UIDraw.DrawCenteredLine(versionText);
        UIDraw.DrawCenteredLine(Localization.Get("CopyrightText"));
    }

    /// <summary>
    /// Prints the application logo and checks for updates if enabled.
    /// </summary>
    private static void PrintLogo()
    {
        if (RegistryWorker.ReadFromRegistry(Constants.RegistryPathBase, Constants.RegistryValueDefaultThemeRecentName) != Variables.UpdatedDefaultThemeName)
        {
            UIDraw.TextColor = Variables.SecondaryColor;
            UIDraw.DrawCenteredLine("New default theme installed: " + Variables.UpdatedDefaultThemeName);
            RegistryWorker.WriteToRegistry(Constants.RegistryPathBase, Constants.RegistryValueDefaultThemeRecentName, Constants.RegistryValueTypeString, Variables.UpdatedDefaultThemeName);
        }

        UIDraw.TextColor = Variables.LogoColor;
        CenterText();

        if (RegistryWorker.ReadFromRegistry(Constants.RegistryPathConfig, Constants.RegistryValueEnableUpdateSearch) == Constants.UpdateSearchEnabledValue)
        {
            try
            {
                UpdateChecker.MainCheck().GetAwaiter().GetResult();
            }
            catch // Handle potential errors during update check (e.g., network issues).
            {
                UIDraw.TextColor = Variables.SecondaryColor;
                UIDraw.DrawCenteredLine(Localization.Get("UpdateCheckFailed"));
                UIDraw.TextColor = ConsoleColor.Gray;
            }
        }
        else
        {
            UIDraw.TextColor = Variables.SecondaryColor;
            UIDraw.DrawCenteredLine(Localization.Get("UpdateSearchDisabled"));
            UIDraw.TextColor = ConsoleColor.Gray;
        }
    }

    /// <summary>
    /// Displays the main menu and waits for user input.
    /// </summary>
    private static char MainMenu()
    {
        UIDraw.TextColor = Variables.MenuColor;

        UIDraw.DrawMenu(Localization.Get("MainMenu"),
            [
                "1) " + Localization.Get("Shutdown"),
                "2) " + Localization.Get("Reboot"),
                "3) " + Localization.Get("Logoff"),
                "4) " + Localization.Get("Schedule"),
            ],
            [
                "9) " + Localization.Get("Settings"),
                "0) " + Localization.Get("Quit"),
            ]);

        return Console.ReadKey().KeyChar;
    }

    /// <summary>
    /// Processes the user's input key from the main menu.
    /// </summary>
    private static void CheckInput(char key)
    {
        switch (char.ToLowerInvariant(key))
        {
            case 'l':
                HandleLicense();
                break;
            case '1':
                ConfirmAndRun(PowerManager.Shutdown);
                break;
            case '2':
                ConfirmAndRun(PowerManager.Reboot);
                break;
            case '3':
                ConfirmAndRun(PowerManager.Logoff);
                break;
            case '4':
                Schedule.Plan();
                break;
            case '9':
                Settings.Show();
                break;
            case '0':
                Environment.Exit(Constants.ExitCodeSuccess);
                break;
            case 'u':
                HandleUpdate();
                break;
            default:
                HandleInvalidInput();
                break;
        }
    }

    /// <summary>
    /// Asks for confirmation (unless disabled in the settings) and runs the action.
    /// </summary>
    private static void ConfirmAndRun(Action action)
    {
        if (RegistryWorker.ReadFromRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSkipConfirmation) != Constants.SkipConfirmationEnabledValue)
        {
            UIDraw.TextColor = Variables.SecondaryColor;
            UIDraw.DrawCenteredLine("\n");
            UIDraw.DrawBoxedMessage(Localization.Get("ConfirmationText"));
            Console.ReadKey();
        }

        action();
    }

    private static void HandleLicense()
    {
        Console.Clear();
        UIDraw.DrawLine(Localization.Get("LicenseText"));
        UIDraw.DrawLine(Environment.NewLine + Localization.Get("PressAnyKeyToGoBack"));
        Console.ReadKey();
        Console.Clear();
    }

    private static void HandleUpdate()
    {
        if (Variables.IsUpToDate)
        {
            Console.Clear();
            UIDraw.TextColor = Variables.SecondaryColor;
            UIDraw.DrawBoxedMessage(Localization.Get("UpToDate"));
            return;
        }

        Console.Clear();
        Console.Title = Localization.Get("UpdaterTitle");
        UIDraw.TextColor = Variables.MenuColor;
        UIDraw.DrawCenteredLine(Localization.Get("UpdateDLStarted"));
        Thread.Sleep(Constants.UpdateDownloadMessageDelayMs);
        AutoUpdater.PerformUpdate().GetAwaiter().GetResult();
        Environment.Exit(Constants.ExitCodeSuccess);
    }

    private static void HandleInvalidInput()
    {
        Console.Clear();
        UIDraw.TextColor = Variables.SecondaryColor;
        UIDraw.DrawBoxedMessage(Localization.Get("InvalidInput"));
        UIDraw.TextColor = ConsoleColor.White;
    }
}
