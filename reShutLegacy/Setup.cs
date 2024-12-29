using System;
using System.Text;
using System.Threading;

namespace reShutCLI
{
    internal class Setup
    {
        // Updated for 2.0.0.0
        public static void FirstStartup()
        {
            // Check if setup is complete from registry
            if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SetupComplete") != "1")
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.CursorVisible = false;
                ThemeLoader.setDefaultTheme();
                Console.ForegroundColor = Variables.LogoColor;
                Console.Title = "Welcome to reShut CLI!";
                Program.CenterText();
                Console.ForegroundColor = ConsoleColor.Gray;
                Thread.Sleep(1500);
                Console.Clear();
                Console.ForegroundColor = Variables.MenuColor;
                Console.Title = "reShut CLI Setup";
                UpdateChecker.DisplayCenteredMessage("╭──────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Welcome to reShut CLI! (1/5) │");
                UpdateChecker.DisplayCenteredMessage("╰──────────────────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Hello! Bonjour! Hallo! │");
                UpdateChecker.DisplayCenteredMessage("╰────────────────────────╯");
                RegInit.Populate(false);
                Thread.Sleep(2000);
                Console.Clear();
                step2:
                Console.Clear();
                UpdateChecker.DisplayCenteredMessage("╭────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Language (2/5) │");
                UpdateChecker.DisplayCenteredMessage("╰────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭──────────────────────────────────────────────────────────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Help translate reShut CLI to your language! https://github.com/elnino0916/reshut-cli │");
                UpdateChecker.DisplayCenteredMessage("╰──────────────────────────────────────────────────────────────────────────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭────────────────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Select the language you would like to use. │");
                UpdateChecker.DisplayCenteredMessage("╰────────────────────────────────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭──────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ 1) English (US) [100%]           │");
                UpdateChecker.DisplayCenteredMessage("├──────────────────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 2) German (Deutsch) [80%]        │");
                UpdateChecker.DisplayCenteredMessage("├──────────────────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 3) French (Français) [50%]       │");
                UpdateChecker.DisplayCenteredMessage("├──────────────────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 4) Spanish (Español) [50%]       │");
                UpdateChecker.DisplayCenteredMessage("├──────────────────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 5) Portuguese (Português) [50%]  │");
                UpdateChecker.DisplayCenteredMessage("╰──────────────────────────────────╯");
                var keyInfoL = Console.ReadKey();
                var key = keyInfoL.KeyChar.ToString();
                switch (key)
                {
                    case "1":
                        RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "Language", "STRING", "en-US");
                        break;

                    case "2":
                        RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "Language", "STRING", "de-DE");
                        break;

                    case "3":
                        RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "Language", "STRING", "fr-FR");
                        break;

                    case "4":
                        RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "Language", "STRING", "es-ES");
                        break;

                    case "5":
                        RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "Language", "STRING", "pt-PT");
                        break;
                    default:
                        goto step2;
                }

                Console.Clear();
                UpdateChecker.DisplayCenteredMessage("╭────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ EULA (2/5) │");
                UpdateChecker.DisplayCenteredMessage("╰────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                Thread.Sleep(250);
                ShowEULA.Start();
                Console.ForegroundColor = Variables.MenuColor;

            step3:
                Console.Clear();
                UpdateChecker.DisplayCenteredMessage("╭───────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Updates (3/5) │");
                UpdateChecker.DisplayCenteredMessage("╰───────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭───────────────────────────────────────────────────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Do you want to enable update checking? This uses GitHub to check for updates. │");
                UpdateChecker.DisplayCenteredMessage("╰───────────────────────────────────────────────────────────────────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭───────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ 1) Yes, enable        │");
                UpdateChecker.DisplayCenteredMessage("├───────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 2) No, disable        │");
                UpdateChecker.DisplayCenteredMessage("╰───────────────────────╯");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.D1 || keyInfo.Key == ConsoleKey.NumPad1)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableUpdateSearch", "STRING", "1");
                }
                else if (keyInfo.Key == ConsoleKey.D2 || keyInfo.Key == ConsoleKey.NumPad2)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableUpdateSearch", "STRING", "0");
                }
                else
                {
                    goto step3;
                }
            step4:
                Console.Clear();
                UpdateChecker.DisplayCenteredMessage("╭───────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Updates (3/5) │");
                UpdateChecker.DisplayCenteredMessage("╰───────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭──────────────────────────────────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Do you want to enable Auto Updates on startup? (recommended) │");
                UpdateChecker.DisplayCenteredMessage("╰──────────────────────────────────────────────────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭───────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ 1) Yes, enable        │");
                UpdateChecker.DisplayCenteredMessage("├───────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 2) No, disable        │");
                UpdateChecker.DisplayCenteredMessage("╰───────────────────────╯");
                ConsoleKeyInfo keyInfo3 = Console.ReadKey();
                if (keyInfo3.Key == ConsoleKey.D1 || keyInfo3.Key == ConsoleKey.NumPad1)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "AutoUpdateOnStart", "STRING", "yes");
                }
                else if (keyInfo3.Key == ConsoleKey.D2 || keyInfo3.Key == ConsoleKey.NumPad2)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "AutoUpdateOnStart", "STRING", "no");
                }
                else
                {
                    goto step4;
                }
            step5:
                Console.Clear();
                UpdateChecker.DisplayCenteredMessage("╭─────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Theme (4/5) │");
                UpdateChecker.DisplayCenteredMessage("╰─────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭───────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ What theme would you like to use? │");
                UpdateChecker.DisplayCenteredMessage("╰───────────────────────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭──────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ 1) Default               │");
                UpdateChecker.DisplayCenteredMessage("├──────────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 2) Red                   │");
                UpdateChecker.DisplayCenteredMessage("├──────────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 3) Blue                  │");
                UpdateChecker.DisplayCenteredMessage("├──────────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 4) Green                 │");
                UpdateChecker.DisplayCenteredMessage("╰──────────────────────────╯");
                ConsoleKeyInfo keyInfo4 = Console.ReadKey();
                if (keyInfo4.Key == ConsoleKey.D1 || keyInfo4.Key == ConsoleKey.NumPad1)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme", "STRING", "default");
                }
                else if (keyInfo4.Key == ConsoleKey.D2 || keyInfo4.Key == ConsoleKey.NumPad2)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme", "STRING", "red");
                }
                else if (keyInfo4.Key == ConsoleKey.D3 || keyInfo4.Key == ConsoleKey.NumPad3)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme", "STRING", "blue");
                }
                else if (keyInfo4.Key == ConsoleKey.D4 || keyInfo4.Key == ConsoleKey.NumPad4)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme", "STRING", "green");
                }
                else
                {
                    goto step5;
                }
                Console.Clear();
                UpdateChecker.DisplayCenteredMessage("╭─────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Applying changes... │");
                UpdateChecker.DisplayCenteredMessage("╰─────────────────────╯");
                ThemeLoader.loadTheme();
                Thread.Sleep(1000);

            step6:
                Console.Clear();
                UpdateChecker.DisplayCenteredMessage("╭────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Confirmation (5/5) │");
                UpdateChecker.DisplayCenteredMessage("╰────────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭──────────────────────────────────────────────────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Would you like to be asked twice before shutting down / rebooting your PC?   │");
                UpdateChecker.DisplayCenteredMessage("╰──────────────────────────────────────────────────────────────────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭───────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ 1) Yes, ask twice     │");
                UpdateChecker.DisplayCenteredMessage("├───────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 2) No, ask once       │");
                UpdateChecker.DisplayCenteredMessage("╰───────────────────────╯");
                ConsoleKeyInfo keyInfo5 = Console.ReadKey();
                if (keyInfo5.Key == ConsoleKey.D1 || keyInfo5.Key == ConsoleKey.NumPad1)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SkipConfirmation", "STRING", "0");
                }
                else if (keyInfo5.Key == ConsoleKey.D2 || keyInfo5.Key == ConsoleKey.NumPad2)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SkipConfirmation", "STRING", "1");
                }
                else
                {
                    goto step6;
                }

                Console.Clear();
                UpdateChecker.DisplayCenteredMessage("╭─────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Setup complete! │");
                UpdateChecker.DisplayCenteredMessage("╰─────────────────╯");
                RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SetupComplete", "STRING", "1");
                UpdateChecker.DisplayCenteredMessage("╭────────────────────────────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ reShut CLI will now restart to save your preferences.  │");
                UpdateChecker.DisplayCenteredMessage("╰────────────────────────────────────────────────────────╯");
                Thread.Sleep(3000);
                AutoRestart.Init();
                return;
            }

        }
    }
}
