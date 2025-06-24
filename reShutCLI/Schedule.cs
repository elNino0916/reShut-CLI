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
            UIDraw.TextColor = Variables.MenuColor;
            UIDraw.DrawCenteredLine("╭──────────────────────────────────╮");
            UIDraw.DrawCenteredLine(string.Format("│ {0,-32} │", rm.GetString("Schedule_PromptAction", culture)));
            UIDraw.DrawCenteredLine(string.Format("│ {0,-32} │", rm.GetString("Schedule_SelectOption", culture)));
            UIDraw.DrawCenteredLine(string.Format("│ 1) {0,-29} │", rm.GetString("Schedule_ShutdownOption", culture)));
            UIDraw.DrawCenteredLine(string.Format("│ 2) {0,-29} │", rm.GetString("Schedule_RebootOption", culture)));
            UIDraw.DrawCenteredLine("├──────────────────────────────────┤");
            UIDraw.DrawCenteredLine(string.Format("│ 0) {0,-29} │", rm.GetString("Schedule_Cancel", culture)));
            UIDraw.DrawCenteredLine(string.Format("│ 9) {0,-29} │", rm.GetString("Back", culture)));
            UIDraw.DrawCenteredLine("╰──────────────────────────────────╯");
            var keyInfo = Console.ReadKey();
            var key = keyInfo.KeyChar.ToString();
            switch (key)
            {
                case "0":
                    Process.Start(@"cmd.exe", "/c shutdown -a");
                    Console.Clear();
                    UIDraw.TextColor = ConsoleColor.Green;
                    UIDraw.DrawBoxedMessage(rm.GetString("Schedule_ActionCancelled", culture));
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
                            UIDraw.TextColor = ConsoleColor.Red;
                            UIDraw.DrawBoxedMessage(rm.GetString("ErrorOccurred", culture));
                            UIDraw.TextColor = ConsoleColor.White;
                            goto Retry;
                        }

                        break;
                    }
            }

            // Phase 2
            Console.Clear();
        seconds:
            UIDraw.TextColor = Variables.MenuColor;

            if (DateTime.Now.ToString("tt") == "")
            {
                UIDraw.DrawBoxedMessage(string.Format(" {0} {1} ", rm.GetString("Schedule_CurrentTime", culture), Time.GetTime(true)));
            }
            else
            {
                if (DateTime.Now.ToString("tt") == "PM" | DateTime.Now.ToString("tt") == "AM")
                {
                    UIDraw.DrawBoxedMessage(string.Format(" {0} {1} ", rm.GetString("Schedule_CurrentTime", culture), Time.GetTime(false)));
                }
            }
            UIDraw.DrawBoxedMessage(rm.GetString("Schedule_PromptWhen", culture));
            UIDraw.DrawBoxedMessage(rm.GetString("Schedule_EnterSeconds", culture));
            UIDraw.TextColor = ConsoleColor.White;
            UIDraw.Draw(rm.GetString("Schedule_InputPrompt", culture) + " ");
            var inputStr = Console.ReadLine();

            if (!int.TryParse(inputStr, out var input))
            {
                UIDraw.TextColor = ConsoleColor.Red;
                UIDraw.DrawBoxedMessage(rm.GetString("Schedule_ErrorNumberInput", culture));
                goto seconds;
            }
            // Phase 3
            Console.Clear();
        phase3retry:
            var minutes = input / 60;
            var hours = minutes / 60;
            UIDraw.TextColor = Variables.MenuColor;

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

            UIDraw.DrawCenteredLine("╭" + new string('─', borderLength) + "╮");
            UIDraw.DrawCenteredLine("│ " + header.PadRight(maxLength) + "   │");
            UIDraw.DrawCenteredLine("│ " + info.PadRight(maxLength) + "   │");
            UIDraw.DrawCenteredLine("├" + new string('─', borderLength) + "┤");
            UIDraw.DrawCenteredLine("│ " + option1.PadRight(maxLength) + "   │");
            UIDraw.DrawCenteredLine("│ " + option2.PadRight(maxLength) + "   │");
            UIDraw.DrawCenteredLine("│ " + option0.PadRight(maxLength) + "   │");
            UIDraw.DrawCenteredLine("╰" + new string('─', borderLength) + "╯");
            UIDraw.TextColor = ConsoleColor.White;
            UIDraw.Draw(rm.GetString("Schedule_InputPrompt", culture) + " ");
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
                        UIDraw.TextColor = ConsoleColor.Green;
                        UIDraw.DrawBoxedMessage(rm.GetString("Schedule_ActionScheduled", culture));
                        UIDraw.TextColor = ConsoleColor.White;
                        return true;
                    }
                    catch
                    {
                        UIDraw.TextColor = ConsoleColor.Red;
                        UIDraw.DrawBoxedMessage(rm.GetString("Schedule_ErrorOccurred", culture));
                        UIDraw.TextColor = ConsoleColor.White;
                        return false;
                    }
            }

            Console.Clear();
            UIDraw.TextColor = ConsoleColor.Red;
            UIDraw.DrawBoxedMessage(rm.GetString("Schedule_ErrorOccurred", culture));
            UIDraw.TextColor = ConsoleColor.White;
            goto phase3retry;
        }
    }
}
