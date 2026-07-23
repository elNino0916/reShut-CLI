using System.Text;
using reShutCLI.Helpers;
using reShutCLI.Services;

namespace reShutCLI;

/// <summary>First-run setup wizard.</summary>
internal static class Setup
{
    public static void FirstStartup()
    {
        if (RegistryWorker.ReadFromRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSetupComplete) == "1")
            return;

        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;
        ThemeLoader.SetDefaultTheme();
        UIDraw.TextColor = Variables.LogoColor;
        Console.Title = "Welcome to reShut CLI!";
        Program.CenterText();
        UIDraw.TextColor = ConsoleColor.Gray;
        Thread.Sleep(1500);
        Console.Clear();
        UIDraw.TextColor = Variables.MenuColor;
        Console.Title = "reShut CLI Setup";
        UIDraw.DrawBoxedMessage("Welcome to reShut CLI!");
        RegInit.Populate(false);
        Thread.Sleep(2000);

        SelectLanguage();
        AcceptEula();
        ConfigureUpdateSearch();
        ConfigureAutoUpdate();
        SelectTheme();
        ConfigureConfirmation();

        Console.Clear();
        UIDraw.DrawBoxedMessage("Setup complete!");
        RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSetupComplete, Constants.RegistryValueTypeString, "1");
        Thread.Sleep(3000);
        AutoRestart.Init();
    }

    private static void SelectLanguage()
    {
        while (true)
        {
            Console.Clear();
            UIDraw.DrawBoxedMessage("Language (1/6)");
            UIDraw.DrawCenteredLine("");
            UIDraw.DrawBoxedMessage("Help translate reShut CLI to your language! " + Constants.GitHubRepoUrl);
            UIDraw.DrawCenteredLine("");
            UIDraw.DrawBoxedMessage("Select the language you would like to use.");
            UIDraw.DrawCenteredLine("");
            UIDraw.DrawMenu(null,
                ["1) English (US) [100%]"],
                ["2) German (Deutsch) [100%]"]);

            switch (Console.ReadKey().KeyChar)
            {
                case '1':
                    RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueLanguage, Constants.RegistryValueTypeString, Constants.LanguageEnglish);
                    return;
                case '2':
                    RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueLanguage, Constants.RegistryValueTypeString, Constants.LanguageGerman);
                    return;
            }
        }
    }

    private static void AcceptEula()
    {
        Console.Clear();
        UIDraw.DrawBoxedMessage("EULA (2/6)");
        UIDraw.DrawCenteredLine("");
        ShowEULA.Start();
        UIDraw.TextColor = Variables.MenuColor;
    }

    private static void ConfigureUpdateSearch() =>
        AskYesNo("Updates (3/6)",
            "Do you want to enable update checking? This uses GitHub to check for updates.",
            Constants.RegistryValueEnableUpdateSearch);

    private static void ConfigureAutoUpdate() =>
        AskYesNo("Updates (4/6)",
            "Do you want to enable Auto Updates on startup? (recommended)",
            Constants.RegistryValueAutoUpdateOnStart);

    private static void AskYesNo(string stepTitle, string question, string registryValueName)
    {
        while (true)
        {
            Console.Clear();
            UIDraw.DrawBoxedMessage(stepTitle);
            UIDraw.DrawCenteredLine("");
            UIDraw.DrawBoxedMessage(question);
            UIDraw.DrawCenteredLine("");
            UIDraw.DrawMenu(null,
                ["1) Yes, enable"],
                ["2) No, disable"]);

            var keyInfo = Console.ReadKey();
            if (keyInfo.Key is ConsoleKey.D1 or ConsoleKey.NumPad1)
            {
                RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, registryValueName, Constants.RegistryValueTypeString, Constants.EnabledValue);
                return;
            }
            if (keyInfo.Key is ConsoleKey.D2 or ConsoleKey.NumPad2)
            {
                RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, registryValueName, Constants.RegistryValueTypeString, Constants.DisabledValue);
                return;
            }
        }
    }

    private static void SelectTheme()
    {
        while (true)
        {
            Console.Clear();
            UIDraw.DrawBoxedMessage("Theme (5/6)");
            UIDraw.DrawCenteredLine("");
            UIDraw.DrawBoxedMessage("Select a theme to personalize your experience.");
            UIDraw.DrawCenteredLine("");
            UIDraw.TextColor = Variables.SecondaryColor;
            UIDraw.DrawBoxedMessage("Current default theme name: " + Variables.UpdatedDefaultThemeName);
            UIDraw.TextColor = Variables.MenuColor;
            UIDraw.DrawCenteredLine("");
            UIDraw.DrawMenu(null,
                ["1) Default (Network required)"],
                ["2) Red", "3) Blue", "4) Green", "5) Nord"]);

            var theme = Console.ReadKey().Key switch
            {
                ConsoleKey.D1 or ConsoleKey.NumPad1 => "default",
                ConsoleKey.D2 or ConsoleKey.NumPad2 => "red",
                ConsoleKey.D3 or ConsoleKey.NumPad3 => "blue",
                ConsoleKey.D4 or ConsoleKey.NumPad4 => "green",
                ConsoleKey.D5 or ConsoleKey.NumPad5 => "nord",
                _ => null,
            };
            if (theme is null) continue;

            RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSelectedTheme, Constants.RegistryValueTypeString, theme);
            Console.Clear();
            UIDraw.DrawBoxedMessage("Applying changes...");
            ThemeLoader.LoadTheme();
            UIDraw.TextColor = Variables.MenuColor;
            return;
        }
    }

    private static void ConfigureConfirmation()
    {
        while (true)
        {
            Console.Clear();
            UIDraw.DrawBoxedMessage("Confirmation (6/6)");
            UIDraw.DrawCenteredLine("");
            UIDraw.DrawBoxedMessage("Would you like to be asked twice before shutting down / rebooting your PC?");
            UIDraw.DrawCenteredLine("");
            UIDraw.DrawMenu(null,
                ["1) Yes, ask twice"],
                ["2) No, ask once"]);

            var keyInfo = Console.ReadKey();
            if (keyInfo.Key is ConsoleKey.D1 or ConsoleKey.NumPad1)
            {
                RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSkipConfirmation, Constants.RegistryValueTypeString, "0");
                return;
            }
            if (keyInfo.Key is ConsoleKey.D2 or ConsoleKey.NumPad2)
            {
                RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSkipConfirmation, Constants.RegistryValueTypeString, "1");
                return;
            }
        }
    }
}
