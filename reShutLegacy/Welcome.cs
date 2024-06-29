using System;
using System.Text;
using System.Threading;

namespace reShutCLI
{
    internal class Welcome
    {
        public static void FirstStartup()
        {
            // Check if setup is complete from registry
            if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SetupComplete") != "1")
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.CursorVisible = false;
                Console.ForegroundColor = Variables.LogoColor;
                Console.Title = "Welcome to reShut CLI!";
                UpdateChecker.DisplayCenteredMessage("Welcome to:");
                Program.CenterText();
                Console.ForegroundColor = ConsoleColor.Gray;
                Thread.Sleep(7000);
                Console.Clear();
                Console.ForegroundColor = Variables.MenuColor;
                Console.Title = "reShut CLI Setup";
                UpdateChecker.DisplayCenteredMessage("╭─────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Initial Setup (1/4) │");
                UpdateChecker.DisplayCenteredMessage("╰─────────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭───────────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Preparing reShut CLI for first use... │");
                UpdateChecker.DisplayCenteredMessage("╰───────────────────────────────────────╯");
                RegInit.Populate();
                Thread.Sleep(3000);
                Console.Clear();
                UpdateChecker.DisplayCenteredMessage("╭──────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ License Agreement and EULA (2/4) │");
                UpdateChecker.DisplayCenteredMessage("╰──────────────────────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                Thread.Sleep(2000);
                EULAHost.Start();
                Console.Clear();
                UpdateChecker.DisplayCenteredMessage("╭───────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Update Checking (3/4) │");
                UpdateChecker.DisplayCenteredMessage("╰───────────────────────╯");
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
                    UpdateChecker.MainCheck().Wait();
                    Console.ForegroundColor = Variables.MenuColor;
                    Thread.Sleep(5000);
                }
                else if (keyInfo.Key == ConsoleKey.D2 || keyInfo.Key == ConsoleKey.NumPad2)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableUpdateSearch", "STRING", "0");
                }
                Console.Clear();
                UpdateChecker.DisplayCenteredMessage("╭────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Fast Startup (4/4) │");
                UpdateChecker.DisplayCenteredMessage("╰────────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭───────────────────────────────────────────────────────────────────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Do you want to enable Fast Startup? This will skip some things on startup. (not recommended!) │");
                UpdateChecker.DisplayCenteredMessage("╰───────────────────────────────────────────────────────────────────────────────────────────────╯");
                UpdateChecker.DisplayCenteredMessage("");
                UpdateChecker.DisplayCenteredMessage("╭───────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ 1) Yes, enable        │");
                UpdateChecker.DisplayCenteredMessage("├───────────────────────┤");
                UpdateChecker.DisplayCenteredMessage("│ 2) No, disable        │");
                UpdateChecker.DisplayCenteredMessage("╰───────────────────────╯");
                ConsoleKeyInfo keyInfo2 = Console.ReadKey();
                if (keyInfo2.Key == ConsoleKey.D1 || keyInfo2.Key == ConsoleKey.NumPad1)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableFastStartup", "STRING", "1");
                }
                else if (keyInfo2.Key == ConsoleKey.D2 || keyInfo2.Key == ConsoleKey.NumPad2)
                {
                    RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableFastStartup", "STRING", "0");
                }
                Console.Clear();
                UpdateChecker.DisplayCenteredMessage("╭─────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Setup complete! │");
                UpdateChecker.DisplayCenteredMessage("╰─────────────────╯");
                RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SetupComplete", "STRING", "1");
                UpdateChecker.DisplayCenteredMessage("╭────────────────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Thank you! reShut CLI is now ready to use. │");
                UpdateChecker.DisplayCenteredMessage("╰────────────────────────────────────────────╯");
                Thread.Sleep(5000);
                return;
            }
        }
    }
}
