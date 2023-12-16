using System;
using System.Diagnostics;
using System.Threading;

namespace reShutLegacy
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
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("╭───────────────────────────────╮");
            Console.WriteLine("│ What do you want to schedule? │");
            Console.WriteLine("│ Select an option:             │");
            Console.WriteLine("│ 1) Shutdown                   │");
            Console.WriteLine("│ 2) Reboot                     │");
            Console.WriteLine("├───────────────────────────────┤");
            Console.WriteLine("│ 9) Cancel schedule            │");
            Console.WriteLine("│ 0) Back                       │");
            Console.WriteLine("╰───────────────────────────────╯");
            var keyInfo = Console.ReadKey();
            var key = keyInfo.KeyChar.ToString();
            switch (key)
            {
                case "9":
                    Process.Start(@"cmd.exe", "/c shutdown -a");
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("╭────────────────────────────╮");
                    Console.WriteLine("│ Action has been cancelled. │");
                    Console.WriteLine("╰────────────────────────────╯");
                    Thread.Sleep(500); // Fixes the 'No Shutdown in progress' message in the middle of the main menu.
                    return false;
                case "1":
                    type = "shutdown";
                    break;
                case "2":
                    type = "reboot";
                    break;
                case "0":
                    Console.Clear();
                    return false;
                default:
                {
                    if (key != "1" | key != "2" | key != "0") 
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("╭────────────────────╮");
                        Console.WriteLine("│ An error occurred. │");
                        Console.WriteLine("╰────────────────────╯");
                        Console.ForegroundColor = ConsoleColor.White;
                        goto Retry;
                    }

                    break;
                }
            }

            // Phase 2
            Console.Clear();
            seconds:
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            if (DateTime.Now.ToString("tt") == "")
            {
                Console.WriteLine("╭────────────────────────╮");
                Console.WriteLine("│ Current time: " + Hardware.GetTime(true) + " │");
                Console.WriteLine("╰────────────────────────╯");
            }
            else 
            {
                if (DateTime.Now.ToString("tt") == "PM" | DateTime.Now.ToString("tt") == "AM")
                {
                    Console.WriteLine("╭───────────────────────────╮");
                    Console.WriteLine("│ Current time: " + Hardware.GetTime(false) + " │");
                    Console.WriteLine("╰───────────────────────────╯");
                }
            }

            Console.WriteLine("╭─────────────────────────────────────╮");
            Console.WriteLine("│ When should the action be executed? │");
            Console.WriteLine("│ Enter the time in seconds.          │");
            Console.WriteLine("╰─────────────────────────────────────╯");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Input: ");
            var inputStr = Console.ReadLine();

            if (!int.TryParse(inputStr, out var input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("╭──────────────────────────────────────╮");
                Console.WriteLine("│ Error: Input must be a valid number. │");
                Console.WriteLine("╰──────────────────────────────────────╯");
                goto seconds;
            }
            // Phase 3
            Console.Clear();
            phase3retry:
            var minutes = input / 60;
            var hours = minutes / 60;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            var header = "You want to schedule a " + type + " in " + input + " seconds. Is that correct?";
            var info = "(" + minutes + "min / " + hours + "hrs)";
            var option1 = "1) Yes, schedule " + type;
            const string option2 = "2) No, go back and retry";
            const string option0 = "0) Back to main menu";
            var maxLength = Math.Max(header.Length, Math.Max(option1.Length, Math.Max(option2.Length, option0.Length)));
            var borderLength = maxLength + 4;

            Console.WriteLine("╭" + new string('─', borderLength) + "╮");
            Console.WriteLine("│ " + header.PadRight(maxLength) + "   │");
            Console.WriteLine("│ " + info.PadRight(maxLength) + "   │");
            Console.WriteLine("├" + new string('─', borderLength) + "┤");
            Console.WriteLine("│ " + option1.PadRight(maxLength) + "   │");
            Console.WriteLine("│ " + option2.PadRight(maxLength) + "   │");
            Console.WriteLine("│ " + option0.PadRight(maxLength) + "   │");
            Console.WriteLine("╰" + new string('─', borderLength) + "╯");
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
                        Process.Start("cmd.exe", "/c shutdown /"+ character + " /f /t " + input);
                        Console.ForegroundColor= ConsoleColor.Green;
                        var toUpperChar = char.ToUpper(type[0]);
                        Console.WriteLine("╭────────────────────────────╮");
                        Console.WriteLine("│ Action has been scheduled. │");
                        Console.WriteLine("╰────────────────────────────╯");
                        Console.ForegroundColor = ConsoleColor.White;
                        return true;
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("╭────────────────────╮");
                        Console.WriteLine("│ An error occurred. │");
                        Console.WriteLine("╰────────────────────╯");
                        Console.ForegroundColor = ConsoleColor.White;
                        return false;
                    }
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("╭────────────────────╮");
            Console.WriteLine("│ An error occurred. │");
            Console.WriteLine("╰────────────────────╯");
            Console.ForegroundColor = ConsoleColor.White;
            goto phase3retry;
        }
    }
}
