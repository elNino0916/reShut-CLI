﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace reShutCLI
{
    internal class ThemeSettings
    {
        public static void OpenSettings()
        {
        step6:
            UpdateChecker.DisplayCenteredMessage("Opening setup, please wait...");
            Thread.Sleep(3000);
            Console.Clear();
            UpdateChecker.DisplayCenteredMessage("╭─────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ Theme (6/6) │");
            UpdateChecker.DisplayCenteredMessage("╰─────────────╯");
            UpdateChecker.DisplayCenteredMessage("");
            UpdateChecker.DisplayCenteredMessage("╭─────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ What theme do you want? │");
            UpdateChecker.DisplayCenteredMessage("╰─────────────────────────╯");
            UpdateChecker.DisplayCenteredMessage("");
            UpdateChecker.DisplayCenteredMessage("╭────────────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ 1) Default (Changes after some time)   │");
            UpdateChecker.DisplayCenteredMessage("├────────────────────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 2) Cyberpunk (Yellow,Blue)             │");
            UpdateChecker.DisplayCenteredMessage("├────────────────────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 3) Red                                 │");
            UpdateChecker.DisplayCenteredMessage("├────────────────────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 4) Blue                                │");
            UpdateChecker.DisplayCenteredMessage("├────────────────────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 5) Green                               │");
            UpdateChecker.DisplayCenteredMessage("╰────────────────────────────────────────╯");
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
                goto step6;
            }
            Console.Clear();
            UpdateChecker.DisplayCenteredMessage("╭─────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ Applying changes... │");
            UpdateChecker.DisplayCenteredMessage("╰─────────────────────╯");
            ThemeLoader.loadTheme();
            Thread.Sleep(3000);

            return;
        }
    }
}
