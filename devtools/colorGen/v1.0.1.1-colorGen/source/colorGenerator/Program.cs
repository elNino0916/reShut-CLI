using System;
using System.Linq;
using System.Text;

namespace colorGenerator
{
    internal class Program
    {
        public static string RandomColor()
        {
            var consoleColors = ((ConsoleColor[])Enum.GetValues(typeof(ConsoleColor)))
                .Where(color => color != ConsoleColor.Black && color != ConsoleColor.Gray && color != ConsoleColor.White && color != ConsoleColor.DarkGray)
                .ToArray();

            var random = new Random();
            var randomIndex = random.Next(consoleColors.Length);
            return consoleColors[randomIndex].ToString();
        }

        private static void CenterText(string color1, string color2)
        {
            string[] lines =
            {
                @"           ____  _           _      ____ _     ___ ",
                @"  _ __ ___/ ___|| |__  _   _| |_   / ___| |   |_ _|",
                @" | '__/ _ \___ \| '_ \| | | | __| | |   | |    | | ",
                @" | | |  __/___) | | | | |_| | |_  | |___| |___ | | ",
                @" |_|  \___|____/|_| |_|\__,_|\__|  \____|_____|___|",
                @"                                                   "
            };

            var maxLength = lines.Max(line => line.Length);
            var padding = (Console.WindowWidth - maxLength) / 2;

            foreach (var line in lines)
            {
                var centeredLine = new string(' ', padding) + line;
                Console.ForegroundColor = ConvertToConsoleColor(color1);
                Console.WriteLine(centeredLine);
            }

            var centeredVersionText = new string(' ', (Console.WindowWidth - "v.1.0.1.1-colorGen".Length) / 2) + "v.1.0.1.1-colorGen";
            var centeredCopyright = new string(' ', (Console.WindowWidth - "Copyright (c) 2024 elNino0916".Length) / 2) + "Copyright (c) 2024 elNino0916";
            var centeredUpdate = new string(' ', (Console.WindowWidth - "reShut is up to date!".Length) / 2) + "reShut is up to date!";

            Console.ForegroundColor = ConvertToConsoleColor(color1);
            Console.WriteLine(centeredVersionText);
            Console.WriteLine(centeredCopyright);
            Console.ForegroundColor = ConvertToConsoleColor(color2);
            Console.WriteLine(centeredUpdate);
            string[] menuItems = ["Shutdown", "Reboot", "Logoff", "Schedule...", "Settings", "Quit"];

            Console.WriteLine("╭────────────────────────╮");
            Console.WriteLine("│       Main Menu        │");
            Console.WriteLine("├────────────────────────┤");

            for (var i = 1; i < menuItems.Length - 1; i++)
                Console.WriteLine("│ " + i + ") " + menuItems[i - 1].PadRight(20) + "│");
            Console.WriteLine("├────────────────────────┤");
            Console.WriteLine("│ 9) " + menuItems[4].PadRight(20) + "│");
            Console.WriteLine("│ 0) " + menuItems[5].PadRight(20) + "│");

            Console.WriteLine("╰────────────────────────╯");
            Console.ResetColor();
        }

        private static ConsoleColor ConvertToConsoleColor(string color)
        {
            if (Enum.TryParse<ConsoleColor>(color, out var consoleColor))
            {
                return consoleColor;
            }
            return ConsoleColor.White;
        }

        private static void Main(string[] args)
        {
            Console.OutputEncoding = UTF8Encoding.UTF8;
            Console.Title = "reShut CLI | (c) 2024 elNino0916";
            nextcolor:
            string color1 = RandomColor();
            string color2 = RandomColor();
            if (color1 == color2)
            {
                goto nextcolor;
            }
            CenterText(color1, color2);
            Console.WriteLine($"Color 1:  {color1} | Color 2: {color2}");
            Console.ReadKey();
            Console.Clear();
            goto nextcolor;
        }
    }
}