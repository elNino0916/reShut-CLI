using System;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace reShutCLI
{
    internal class Schedule
    {
        public static bool Plan()
        {
            CultureInfo culture = new CultureInfo(Variables.lang);
            ResourceManager rm = new ResourceManager("reShutCLI.Resources.Strings", typeof(Program).Assembly);
        Retry:
            // set strings
            // type:
            // shutdown
            // reboot
            Console.Clear();
            var type = "";
            Console.ForegroundColor = Variables.MenuColor;
            UpdateChecker.DisplayCenteredMessage("╭──────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage(string.Format("│ {0,-32} │", rm.GetString("Schedule_PromptAction", culture)));
            UpdateChecker.DisplayCenteredMessage(string.Format("│ {0,-32} │", rm.GetString("Schedule_SelectOption", culture)));
            UpdateChecker.DisplayCenteredMessage(string.Format("│ 1) {0,-29} │", rm.GetString("Schedule_ShutdownOption", culture)));
            UpdateChecker.DisplayCenteredMessage(string.Format("│ 2) {0,-29} │", rm.GetString("Schedule_RebootOption", culture)));
            UpdateChecker.DisplayCenteredMessage("├──────────────────────────────────┤");
            UpdateChecker.DisplayCenteredMessage(string.Format("│ 0) {0,-29} │", rm.GetString("Schedule_Cancel", culture)));
            UpdateChecker.DisplayCenteredMessage(string.Format("│ 9) {0,-29} │", rm.GetString("Back", culture)));
            UpdateChecker.DisplayCenteredMessage("╰──────────────────────────────────╯");
            var keyInfo = Console.ReadKey();
            var key = keyInfo.KeyChar.ToString();
            switch (key)
            {
                case "0":
                    Process.Start(@"cmd.exe", "/c shutdown -a");
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    UpdateChecker.DisplayCenteredMessage("╭────────────────────────────╮");
                    UpdateChecker.DisplayCenteredMessage(string.Format("│ {0,-26} │", rm.GetString("Schedule_ActionCancelled", culture)));
                    UpdateChecker.DisplayCenteredMessage("╰────────────────────────────╯");
                    Thread.Sleep(500); // Fixes the 'No Shutdown in progress' message in the middle of the main menu.
                    return false;
                case "1":
                    type = rm.GetString("Shutdown", culture).ToLower(); // Use translated term, ensure lowercase for logic
                    break;
                case "2":
                    type = rm.GetString("Reboot", culture).ToLower(); // Use translated term, ensure lowercase for logic
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
                            UpdateChecker.DisplayCenteredMessage(string.Format("│ {0,-18} │", rm.GetString("ErrorOccurred", culture)));
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
                UpdateChecker.DisplayCenteredMessage(string.Format("│ {0} {1} │", rm.GetString("Schedule_CurrentTime", culture), Time.GetTime(true).PadRight(13)));
                UpdateChecker.DisplayCenteredMessage("╰────────────────────────╯");
            }
            else
            {
                if (DateTime.Now.ToString("tt") == "PM" | DateTime.Now.ToString("tt") == "AM")
                {
                    UpdateChecker.DisplayCenteredMessage("╭───────────────────────────╮");
                    UpdateChecker.DisplayCenteredMessage(string.Format("│ {0} {1} │", rm.GetString("Schedule_CurrentTime", culture), Time.GetTime(false).PadRight(16)));
                    UpdateChecker.DisplayCenteredMessage("╰───────────────────────────╯");
                }
            }

            UpdateChecker.DisplayCenteredMessage("╭─────────────────────────────────────╮");
            UpdateChecker.DisplayCenteredMessage(string.Format("│ {0,-35} │", rm.GetString("Schedule_PromptWhen", culture)));
            UpdateChecker.DisplayCenteredMessage(string.Format("│ {0,-35} │", rm.GetString("Schedule_EnterSeconds", culture)));
            UpdateChecker.DisplayCenteredMessage("╰─────────────────────────────────────╯");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(rm.GetString("Schedule_InputPrompt", culture) + " ");
            var inputStr = Console.ReadLine();

            if (!int.TryParse(inputStr, out var input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                UpdateChecker.DisplayCenteredMessage("╭──────────────────────────────────────╮");
                UpdateChecker.DisplayCenteredMessage(string.Format("│ {0,-36} │", rm.GetString("Schedule_ErrorNumberInput", culture)));
                UpdateChecker.DisplayCenteredMessage("╰──────────────────────────────────────╯");
                goto seconds;
            }
            // Phase 3
            Console.Clear();
        phase3retry:
            var minutes = input / 60;
            var hours = minutes / 60;
            Console.ForegroundColor = Variables.MenuColor;

            string translatedType = type; // Default to 'type' which is already translated and lowercased
            if (type.Equals(rm.GetString("Shutdown", culture).ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                translatedType = rm.GetString("Shutdown", culture);
            }
            else if (type.Equals(rm.GetString("Reboot", culture).ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                translatedType = rm.GetString("Reboot", culture);
            }

            var header = string.Format(rm.GetString("Schedule_ConfirmActionSeconds", culture), translatedType, input);
            var info = string.Format(rm.GetString("Schedule_TimeBreakdown", culture), minutes, hours);
            var option1 = string.Format("1) " + rm.GetString("Schedule_ConfirmYes", culture), translatedType);
            string option2 = "2) " + rm.GetString("Schedule_ConfirmNoReenter", culture);
            string option0 = "0) " + rm.GetString("BackToMainMenu", culture);

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
            Console.Write(rm.GetString("Schedule_InputPrompt", culture) + " ");
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
                        var character = type.Equals(rm.GetString("Shutdown", culture).ToLower(), StringComparison.OrdinalIgnoreCase) ? "s" : "r";
                        Process.Start("cmd.exe", "/c shutdown /" + character + " /f /t " + input);
                        Console.ForegroundColor = ConsoleColor.Green;
                        UpdateChecker.DisplayCenteredMessage("╭────────────────────────────╮");
                        UpdateChecker.DisplayCenteredMessage(string.Format("│ {0,-26} │", rm.GetString("Schedule_ActionScheduled", culture)));
                        UpdateChecker.DisplayCenteredMessage("╰────────────────────────────╯");
                        Console.ForegroundColor = ConsoleColor.White;
                        return true;
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        UpdateChecker.DisplayCenteredMessage("╭────────────────────╮");
                        UpdateChecker.DisplayCenteredMessage(string.Format("│ {0,-18} │", rm.GetString("ErrorOccurred", culture)));
                        UpdateChecker.DisplayCenteredMessage("╰────────────────────╯");
                        Console.ForegroundColor = ConsoleColor.White;
                        return false;
                    }
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            UpdateChecker.DisplayCenteredMessage("╭────────────────────╮");
            UpdateChecker.DisplayCenteredMessage(string.Format("│ {0,-18} │", rm.GetString("ErrorOccurred", culture)));
            UpdateChecker.DisplayCenteredMessage("╰────────────────────╯");
            Console.ForegroundColor = ConsoleColor.White;
            goto phase3retry;
        }
    }
}
