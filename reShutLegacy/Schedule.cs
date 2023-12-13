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
            string type = "";
            int input = 0;
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
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            string key = keyInfo.KeyChar.ToString();
            if (key == "9") 
            {
                type = "cancel";
                Process.Start(@"cmd.exe", "/c shutdown -a");
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("╭────────────────────────────╮");
                Console.WriteLine("│ Action has been cancelled. │");
                Console.WriteLine("╰────────────────────────────╯");
                Thread.Sleep(500); // Fixes the 'No Shutdown in progress' message in the middle of the main menu.
                return false;
            }
            else

            if (key == "1")
            {
                type = "shutdown";
            }else if (key == "2")
            {
                type = "reboot";
            }else if (key == "0") 
            {
                type = "quit";
                Console.Clear();
                return false;
            }else if (key != "1" | key != "2" | key != "0") 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("╭────────────────────╮");
                Console.WriteLine("│ An error occurred. │");
                Console.WriteLine("╰────────────────────╯");
                Console.ForegroundColor = ConsoleColor.White;
                goto Retry;
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
            string inputStr = Console.ReadLine();

            if (!int.TryParse(inputStr, out input))
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
            int minutes = input / 60;
            int hours = minutes / 60;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string header = "You want to schedule a " + type + " in " + input + " seconds. Is that correct?";
            string info = "(" + minutes + "min / " + hours + "hrs)";
            string option1 = "1) Yes, schedule " + type;
            string option2 = "2) No, go back and retry";
            string option0 = "0) Back to main menu";
            int maxLength = Math.Max(header.Length, Math.Max(option1.Length, Math.Max(option2.Length, option0.Length)));
            int borderLength = maxLength + 4;

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
            ConsoleKeyInfo keyInfo3 = Console.ReadKey();
            string ke3y = keyInfo3.KeyChar.ToString();
            if (ke3y == "0")
            {
                Console.Clear();
                return false;
            }
            else if (ke3y == "2")
            {
                Console.Clear();
                goto Retry;
            }
            else if (ke3y == "1")
            {
                Console.Clear();
                try
                {
                    string character = "";
                    if (type == "shutdown")
                    {
                        character = "s";
                    }
                    else
                    {
                        character = "r";
                    }
                    Process.Start(@"cmd.exe", "/c shutdown /"+ character + " /f /t " + input);
                    Console.ForegroundColor= ConsoleColor.Green;
                    char toUpperChar = char.ToUpper(type[0]);
                    string toUpperOut = toUpperChar.ToString() + type.Substring(1);
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
            else
            {
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
}
