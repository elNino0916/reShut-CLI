
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
                ErrorHandler.ShowError("Invalid date detected (YEAR_MISMATCH)", true);
            }
        }
    }
}
