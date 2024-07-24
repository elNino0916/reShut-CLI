using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace reShutCLI
{
    internal class ErrorHandler
    {
        public static void ShowError(string error, bool needsRestart)
        {
            Console.Title = "reShut CLI - Error";
            Console.OutputEncoding = Encoding.UTF8;

            // Prepare all lines of the message
            var lines = new[]
            {
                "An error occurred!",
                "Here is an error description:",
                error,
                needsRestart ? "reShut CLI needs a restart." : "reShut CLI does not need a restart.",
                needsRestart ? "Press any key to restart reShut CLI." : "Press any key to go into settings."
            };

            // Determine the width of the box based on the longest line
            int boxWidth = lines.Max(line => line.Length) + 2; // Adding padding

            // Create the top, middle, and bottom borders of the box
            string topBorder = "╭" + new string('─', boxWidth) + "╮";
            string middleBorder = "├" + new string('─', boxWidth) + "┤";
            string bottomBorder = "╰" + new string('─', boxWidth) + "╯";

            // Print the error box
            Console.Clear();
            Console.ForegroundColor = Variables.SecondaryColor;
            Console.WriteLine(topBorder);
            Console.WriteLine("│ " + lines[0].PadRight(boxWidth - 2) + " │");
            Console.WriteLine(middleBorder);
            Console.ForegroundColor = Variables.SecondaryColor;
            Console.WriteLine("│ " + lines[1].PadRight(boxWidth - 2) + " │");
            Console.WriteLine(middleBorder);
            Console.WriteLine("│ " + lines[2].PadRight(boxWidth - 2) + " │");
            Console.WriteLine(middleBorder);
            if (needsRestart)
            {
                Console.ForegroundColor = Variables.SecondaryColor;
                Console.WriteLine("│ " + lines[3].PadRight(boxWidth - 2) + " │");
                Console.WriteLine(middleBorder);
                Console.WriteLine("│ " + lines[4].PadRight(boxWidth - 2) + " │");
                Console.WriteLine(bottomBorder);
                Console.ReadKey();
                AutoRestart.Init();
            }
            else
            {
                Console.ForegroundColor = Variables.SecondaryColor;
                Console.WriteLine("│ " + lines[3].PadRight(boxWidth - 2) + " │");
                Console.WriteLine(middleBorder);
                Console.WriteLine("│ " + lines[4].PadRight(boxWidth - 2) + " │");
                Console.WriteLine(bottomBorder);
                Console.ReadKey();
                Settings.Show();
            }
        }
    }
}
