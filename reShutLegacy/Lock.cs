using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace reShutLegacy
{
    internal class Lock
    {
        public static void YearLock(string lockedYear)
        {
            if (DateTime.Now.ToString("yyyy") == lockedYear || lockedYear == "*")
            {
                Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Themes.NewYear23.RandomColor());
                Console.WriteLine("Update status:");
                UpdateChecker.MainCheck().Wait();
                Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Themes.NewYear23.RandomColor());
                Console.WriteLine($"\nLooks like its already {lockedYear} in your timezone, Happy New Year!\n\nYou cannot use reShut-Legacy at the moment.\n\nA new update will be available soon that will unlock reShut-Legacy again.\n\nA secret party mode will open in 10 seconds.");
                Thread.Sleep(10000);
                Themes.NewYear23.PartyMode();
            }
            else return;
        }
    }
}
