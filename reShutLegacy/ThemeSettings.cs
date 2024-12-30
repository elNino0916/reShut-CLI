using System;
using System.Threading;

namespace reShutCLI
{
    internal class ThemeSettings
    {
        public static void OpenSettings()
        {
        step6:
            Console.Clear();
            UpdateChecker.DisplayCenteredMessage("╭───────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ What theme would you like to use? │");
            UpdateChecker.DisplayCenteredMessage("╰───────────────────────────────────╯");
            UpdateChecker.DisplayCenteredMessage("");
            UpdateChecker.DisplayCenteredMessage("╭──────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ 1) Default               │"); 
            UpdateChecker.DisplayCenteredMessage("│ 2) Red                   │");
            UpdateChecker.DisplayCenteredMessage("│ 3) Blue                  │");
            UpdateChecker.DisplayCenteredMessage("│ 4) Green                 │");
            UpdateChecker.DisplayCenteredMessage("├──────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 9) Back                  │");
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
            else if (keyInfo4.Key == ConsoleKey.D9 || keyInfo4.Key == ConsoleKey.NumPad9)
            {
                return;
            }
            else
            {
                goto step6;
            }
            ThemeLoader.loadTheme();
            Console.Clear();
            AutoRestart.Init();
            return;
        }
    }
}
