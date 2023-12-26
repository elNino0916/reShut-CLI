using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace reShutLegacy
{
    internal class NewYear
    {
        private static Timer countdownTimer;
        private static DateTime newYear;
        public static void StartTimer()
        {

            SetupCountdownTimer();
            while (true)
            {

            }
        }
        private static void SetupCountdownTimer()
        {
            countdownTimer = new Timer(1000);
            countdownTimer.Elapsed += OnTimedEvent;
            countdownTimer.AutoReset = true;
            countdownTimer.Enabled = true;

            var now = DateTime.Now;
            newYear = new DateTime(now.Year, 12, 31, 23, 59, 59);
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            var now = DateTime.Now;

            if (DateTime.Now.ToString("yyyy") == "2024")
            {
                countdownTimer.Stop();
                countdownTimer.Dispose();
                Lock.YearLock("2024");
                Environment.Exit(0);
            }
            else
            {
                var timeLeft = newYear - now;
                Console.Clear();
                Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Themes.NewYear23.RandomColor());
                Console.WriteLine($"Time left until New Year: {timeLeft.Days} days, {timeLeft.Hours} hours, {timeLeft.Minutes} minutes, {timeLeft.Seconds} seconds");
            }
        }
    }
}
