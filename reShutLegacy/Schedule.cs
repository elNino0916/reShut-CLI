using System;
using System.Diagnostics;
using System.Threading;

namespace reShutCLI
{
    internal class Schedule
    {
        public static bool Plan()
        {
        Retry:
            // set strings
            // type:
            // shutdown
            // reboot
            var type = "";
            Console.Clear();
            Console.ForegroundColor = Variables.MenuColor;
            UpdateChecker.DisplayCenteredMessage("╭───────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ What do you want to schedule? │");
            UpdateChecker.DisplayCenteredMessage("│ Select an option:             │");
            UpdateChecker.DisplayCenteredMessage("│ 1) Shutdown                   │");
            UpdateChecker.DisplayCenteredMessage("│ 2) Reboot                     │");
            UpdateChecker.DisplayCenteredMessage("├───────────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage("│ 0) Cancel schedule            │");
            UpdateChecker.DisplayCenteredMessage("│ 9) Back                       │");
            UpdateChecker.DisplayCenteredMessage("╰───────────────────────────────╯");
            var keyInfo = Console.ReadKey();
            var key = keyInfo.KeyChar.ToString();
            switch (key)
            {
                case "0":
                    Process.Start(@"cmd.exe", "/c shutdown -a");
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    UpdateChecker.DisplayCenteredMessage("╭────────────────────────────╮");
                    UpdateChecker.DisplayCenteredMessage("│ Action has been cancelled. │");
                    UpdateChecker.DisplayCenteredMessage("╰────────────────────────────╯");
                    Thread.Sleep(500); // Fixes the 'No Shutdown in progress' message in the middle of the main menu.
                    return false;
                case "1":
                    type = "shutdown";
                    break;
                case "2":
                    type = "reboot";
                    break;
                case "9":
                    Console.Clear();
                    return false;
                default:
                    {
                        if (key != "1" | key != "2" | key != "0")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            UpdateChecker.DisplayCenteredMessage("╭────────────────────╮");
                            UpdateChecker.DisplayCenteredMessage("│ An error occurred. │");
                            UpdateChecker.DisplayCenteredMessage("╰────────────────────╯");
                            Console.ForegroundColor = ConsoleColor.White;
                            goto Retry;
                        }

                        break;
                    }
            }

            // Phase 2
            Console.Clear();
        seconds:
            Console.ForegroundColor = Variables.MenuColor;

            if (DateTime.Now.ToString("tt") == "")
            {
                UpdateChecker.DisplayCenteredMessage("╭────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Current time: " + Hardware.GetTime(true) + " │");
                UpdateChecker.DisplayCenteredMessage("╰────────────────────────╯");
            }
            else
            {
                if (DateTime.Now.ToString("tt") == "PM" | DateTime.Now.ToString("tt") == "AM")
                {
                    UpdateChecker.DisplayCenteredMessage("╭───────────────────────────╮");
                    UpdateChecker.DisplayCenteredMessage("│ Current time: " + Hardware.GetTime(false) + " │");
                    UpdateChecker.DisplayCenteredMessage("╰───────────────────────────╯");
                }
            }

            UpdateChecker.DisplayCenteredMessage("╭─────────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage("│ When should the action be executed? │");
            UpdateChecker.DisplayCenteredMessage("│ Enter the time in seconds.          │");
            UpdateChecker.DisplayCenteredMessage("╰─────────────────────────────────────╯");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Input: ");
            var inputStr = Console.ReadLine();

            if (!int.TryParse(inputStr, out var input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                UpdateChecker.DisplayCenteredMessage("╭──────────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage("│ Error: Input must be a valid number. │");
                UpdateChecker.DisplayCenteredMessage("╰──────────────────────────────────────╯");
                goto seconds;
            }
            // Phase 3
            Console.Clear();
        phase3retry:
            var minutes = input / 60;
            var hours = minutes / 60;
            Console.ForegroundColor = Variables.MenuColor;
            var header = "You want to schedule a " + type + " in " + input + " seconds. Is that correct?";
            var info = "(~" + minutes + "min / ~" + hours + "hrs)";
            var option1 = "1) Yes, schedule " + type;
            const string option2 = "2) No, go back.";
            const string option0 = "0) Back to main menu";
            var maxLength = Math.Max(header.Length, Math.Max(option1.Length, Math.Max(option2.Length, option0.Length)));
            var borderLength = maxLength + 4;

            UpdateChecker.DisplayCenteredMessage("╭" + new string('─', borderLength) + "╮");
            UpdateChecker.DisplayCenteredMessage("│ " + header.PadRight(maxLength) + "   │");
            UpdateChecker.DisplayCenteredMessage("│ " + info.PadRight(maxLength) + "   │");
            UpdateChecker.DisplayCenteredMessage("├" + new string('─', borderLength) + "┤");
            UpdateChecker.DisplayCenteredMessage("│ " + option1.PadRight(maxLength) + "   │");
            UpdateChecker.DisplayCenteredMessage("│ " + option2.PadRight(maxLength) + "   │");
            UpdateChecker.DisplayCenteredMessage("│ " + option0.PadRight(maxLength) + "   │");
            UpdateChecker.DisplayCenteredMessage("╰" + new string('─', borderLength) + "╯");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Input: ");
            var keyInfo3 = Console.ReadKey();
            var ke3y = keyInfo3.KeyChar.ToString();
            switch (ke3y)
            {
                case "0":
                    Console.Clear();
                    return false;
                case "2":
                    Console.Clear();
                    goto Retry;
                case "1":
                    Console.Clear();
                    try
                    {
                        var character = type == "shutdown" ? "s" : "r";
                        Process.Start("cmd.exe", "/c shutdown /" + character + " /f /t " + input);
                        Console.ForegroundColor = ConsoleColor.Green;
                        var toUpperChar = char.ToUpper(type[0]);
                        UpdateChecker.DisplayCenteredMessage("╭────────────────────────────╮");
                        UpdateChecker.DisplayCenteredMessage("│ Action has been scheduled. │");
                        UpdateChecker.DisplayCenteredMessage("╰────────────────────────────╯");
                        Console.ForegroundColor = ConsoleColor.White;
                        return true;
                    }
                    catch
                    {
                        ErrorHandler.ShowError(@$"Failed to schedule an action.", false);
                        return false;
                    }
                default:
                    Console.Clear();
                    goto phase3retry;
            }
        }
    }
}
