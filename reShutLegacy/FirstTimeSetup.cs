using System;
using System.Text;
using System.Threading;

namespace reShutCLI
{
    internal class FirstTimeSetup
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
                Thread.Sleep(3000);
                Console.Clear();
                Console.ForegroundColor = Variables.MenuColor;
                Console.Title = "reShut CLI Setup";
                UpdateChecker.DisplayCenteredMessage("╭─────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Initial Setup (1/5) │");
                UpdateChecker.DisplayCenteredMessage("╰─────────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭───────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Creating initial configuration... │");
                UpdateChecker.DisplayCenteredMessage("╰───────────────────────────────────╯");
                RegInit.Populate(false);
                Thread.Sleep(1000);
                Console.Clear();
                UpdateChecker.DisplayCenteredMessage("╭────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ EULA (2/5) │");
                UpdateChecker.DisplayCenteredMessage("╰────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                Thread.Sleep(500);
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
                UpdateChecker.DisplayCenteredMessage("╭─────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ What theme do you want? │");
                UpdateChecker.DisplayCenteredMessage("╰─────────────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭──────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ 1) Default               │");
                UpdateChecker.DisplayCenteredMessage("├──────────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 2) Cyberpunk             │");
                UpdateChecker.DisplayCenteredMessage("├──────────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 3) Red                   │");
                UpdateChecker.DisplayCenteredMessage("├──────────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 4) Blue                  │");
                UpdateChecker.DisplayCenteredMessage("├──────────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 5) Green                 │");
                UpdateChecker.DisplayCenteredMessage("╰──────────────────────────╯");
                ConsoleKeyInfo keyInfo4 = Console.ReadKey();
                if (keyInfo4.Key == ConsoleKey.D1 || keyInfo4.Key == ConsoleKey.NumPad1)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme", "STRING", "default");
                }
                else if (keyInfo4.Key == ConsoleKey.D2 || keyInfo4.Key == ConsoleKey.NumPad2)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme", "STRING", "cyberpunk");
                }
                else if (keyInfo4.Key == ConsoleKey.D3 || keyInfo4.Key == ConsoleKey.NumPad3)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme", "STRING", "red");
                }
                else if (keyInfo4.Key == ConsoleKey.D4 || keyInfo4.Key == ConsoleKey.NumPad4)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SelectedTheme", "STRING", "blue");
                }
                else if (keyInfo4.Key == ConsoleKey.D5 || keyInfo4.Key == ConsoleKey.NumPad5)
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
                UpdateChecker.DisplayCenteredMessage("╭─────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ reShut CLI is now ready to use. │");
                UpdateChecker.DisplayCenteredMessage("╰─────────────────────────────────╯");
                Thread.Sleep(3000);
                return;
            }
        }
    }
}
