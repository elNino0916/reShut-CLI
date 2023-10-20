using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace reShutLegacy
{
    internal class Debugger
    {
        public static bool Check()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                    Console.Clear();
                    Console.Title = "Debugger is not allowed.";
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please start reShut Legacy without an debugger.");
                    Console.WriteLine("You can look into the source code if you want to:");
                    Console.WriteLine(@"https://github.com/elNino0916/reShut-Legacy");
                    Console.WriteLine(@"Open an issue on github if you think that this is an mistake. Also attach the information below:");
                    Console.WriteLine(@"---");
                    Console.WriteLine(@"Information:");
                    Console.WriteLine(@"reShut " + variables.version);
                    Console.WriteLine(@"PreRelease: " + variables.prerelease);
                    Console.WriteLine(@"Identifier: " + "dbg-" + HWID.GetHWID() + "-" + DateTime.Now.ToString("HH_mm_ss"));
                    Console.WriteLine(@"---");
                Console.WriteLine("reShut-Legacy has been freezed. Restart reShut without Debugger to continue.");

            }
            return false;
        }
    }
}
