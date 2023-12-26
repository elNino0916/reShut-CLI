using System;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace reShutLegacy
{
    internal class Themes
    {
        internal class Christmas23
        {
            // The Christmas theme 2023 was just the default theme with white background and dark green text.

        }
        internal class NewYear23
        {
            // This contains the New Year 2023 theme:
            // It includes Random colors for all menus and a secret party mode.
            public static string RandomColor()
            {
                var consoleColors = ((ConsoleColor[])Enum.GetValues(typeof(ConsoleColor)))
                    .Where(color => color != ConsoleColor.Black)
                    .ToArray();

                var random = new Random();
                var randomIndex = random.Next(consoleColors.Length);
                return consoleColors[randomIndex].ToString();
            }

            private static ConsoleColor previousColor = ConsoleColor.Black;

            public static void PartyMode()
            {
                Console.Title = "reShut Legacy - Party Mode";
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Thread.Sleep(4000);

                while (true)
                {
                    Thread.Sleep(500);
                    DisplayRandomColorfulScreen();
                }
            }

            static void DisplayRandomColorfulScreen()
            {
                var consoleWidth = Console.WindowWidth;
                var consoleHeight = Console.WindowHeight;

                for (var row = 0; row < consoleHeight; row++)
                {
                    try
                    {
                        Console.SetCursorPosition(0, row);
                    }
                    catch { }

                    Console.ForegroundColor = GetRandomConsoleColor();
                    DrawColorfulRow(consoleWidth, row);
                }
            }

            static void DrawColorfulRow(int width, int row)
            {
                var random = new Random();
                var characters = new[] { '\u2588', '\u2580', '\u2593' };
                int thickness = random.Next(1, 4);

                for (var t = 0; t < thickness; t++)
                {
                    for (var col = 0; col < width; col++)
                    {
                        Console.Write(characters[random.Next(characters.Length)]);
                    }

                    try
                    {
                        Console.SetCursorPosition(0, row + t + 1);
                    }
                    catch { }
                }
            }

            static ConsoleColor GetRandomConsoleColor()
            {
                ConsoleColor[] allColors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));

                var shuffledColors = allColors.OrderBy(x => Guid.NewGuid()).Where(color => color != ConsoleColor.Black).ToArray();

                var newColor = shuffledColors.FirstOrDefault(c => c != previousColor);

                if (newColor == 0)
                {
                    newColor = shuffledColors.First();
                }

                previousColor = newColor; 

                return newColor;
            }

        }
    }
}