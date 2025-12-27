using System;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Runtime.Versioning;

namespace reShutCLI.Helpers
{
    internal class ErrorHandler
    {
        [SupportedOSPlatform("windows")]
        public static void SetConsoleSize(int width, int height)
        {
            if (Console.LargestWindowWidth >= width && Console.LargestWindowHeight >= height)
            {
                Console.SetWindowSize(width, height);
            }
            else
            {
                Console.Clear();
                UIDraw.TextColor = ConsoleColor.Red;
                UIDraw.DrawCenteredLine("Error: Cannot resize the console window. It's too small for the required dimensions.");
                Console.ResetColor();
            }
        }
        public static void ShowError(string error, bool needsRestart)
        {
            CultureInfo culture = new CultureInfo(Variables.lang);
            ResourceManager rm = new ResourceManager("reShutCLI.Resources.Strings", typeof(Program).Assembly);
            Console.Title = "reShut CLI";
            Console.OutputEncoding = Encoding.UTF8;

            // Prepare all lines of the message
            var lines = new[]
            {
                rm.GetString("ErrorOccurred", culture),
                error,
                needsRestart ? rm.GetString("NeedRestart", culture) : "",
                needsRestart ? rm.GetString("ErrorHandler_PressKeyRestart",culture) : rm.GetString("ErrorHandler_PressKeyBack",culture)
            };

            // Determine the width of the box based on the longest line
            int boxWidth = lines.Max(line => line.Length) + 2; // Adding padding

            // Create the top, middle, and bottom borders of the box
            string topBorder = "\u256D" + new string('\u2500', boxWidth) + "\u256E";
            string middleBorder = "\u251C" + new string('\u2500', boxWidth) + "\u2524";
            string bottomBorder = "\u2570" + new string('\u2500', boxWidth) + "\u256F";

            // Print the error box
            Console.Clear();
            UIDraw.TextColor = Variables.LogoColor;
            UIDraw.DrawBoxedMessage(rm.GetString("ErrorHandler_BeingImproved", culture));
            UIDraw.DrawLine(" ");
            UIDraw.TextColor = Variables.SecondaryColor;
            UIDraw.DrawCenteredLine(topBorder);
            UIDraw.DrawCenteredLine("\u2502 " + lines[0].PadRight(boxWidth - 2) + " \u2502");
            UIDraw.DrawCenteredLine(middleBorder);
            UIDraw.TextColor = Variables.SecondaryColor;
            UIDraw.DrawCenteredLine("\u2502 " + lines[1].PadRight(boxWidth - 2) + " \u2502");
            UIDraw.DrawCenteredLine(middleBorder);
            UIDraw.DrawCenteredLine("\u2502 " + lines[2].PadRight(boxWidth - 2) + " \u2502");
            if (needsRestart)
            {
                UIDraw.TextColor = Variables.SecondaryColor;
                UIDraw.DrawCenteredLine("\u2502 " + lines[3].PadRight(boxWidth - 2) + " \u2502");
                UIDraw.DrawCenteredLine(bottomBorder);
                Console.ReadKey();
                AutoRestart.Init();
            }
            else
            {
                UIDraw.TextColor = Variables.SecondaryColor;
                UIDraw.DrawCenteredLine("\u2502 " + lines[3].PadRight(boxWidth - 2) + " \u2502");
                UIDraw.DrawCenteredLine(bottomBorder);
                Console.ReadKey();
                Program.Main(Array.Empty<string>());
            }
        }
    }
}
