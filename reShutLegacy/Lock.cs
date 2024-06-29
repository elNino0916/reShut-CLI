﻿
using System;
using System.Linq;

namespace reShutCLI
{
    internal class Lock
    {
        public static void Years()
        {
            string[] allowedYears = { "2024" };
            string currentYear = DateTime.Now.ToString("yyyy");
            if (!allowedYears.Contains(currentYear))
            {
                // ErrorHandler.ShowError("Invalid date detected (YEAR_MISMATCH)", true);
                Console.Title = "Error";
                Console.ForegroundColor = Variables.LogoColor;
                Console.WriteLine("An error occurred.\n");
                Console.ForegroundColor = Variables.MenuColor;
                Console.WriteLine($"Check your date and try again.\n\nError Code: YEAR_MISMATCH");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}
