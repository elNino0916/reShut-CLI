using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reShutCLI
{
    internal class UIDraw
    {
        public static void DisplayBoxedMessage(string message)
        {
            DisplayBoxedMessages(new List<string> { message });
        }
        public static void DisplayBoxedMessages(IEnumerable<string> messages)
        {
            var lines = messages.ToList();
            if (!lines.Any()) return;

            int maxContentWidth = lines.Max(line => line.Length);
            int totalWidth = maxContentWidth + 4;

            string topBorder = "╭" + new string('─', totalWidth - 2) + "╮";
            string bottomBorder = "╰" + new string('─', totalWidth - 2) + "╯";

            DisplayCentered(topBorder);

            foreach (string line in lines)
            {
                string paddedLine = $"│ {line.PadRight(maxContentWidth)} │";
                DisplayCentered(paddedLine);
            }

            DisplayCentered(bottomBorder);
        }
        public static void DisplayCentered(string message)
        {
            var consoleWidth = Console.WindowWidth;
            var padding = (consoleWidth - message.Length) / 2;

            Console.WriteLine(new string(' ', padding) + message);
        }
    }
}
