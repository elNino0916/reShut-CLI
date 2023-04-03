using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reShutLegacy
{
    internal class Debuggear
    {
        public static bool Check()
        {
            if (Debugger.IsAttached)
            {
                if (variables.buildfromsource == false)
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
                    Console.WriteLine(@"BuiltFromSource: " + variables.buildfromsource);
                    Console.WriteLine(@"Identifier: " + "dbg-" + HWID.GetHWID() + "-" + DateTime.Now.ToString("HH_mm_ss"));
                    Console.WriteLine(@"---");
                }
                else
                {
                    Console.Clear();
                    Console.Title = "Debugger is not allowed.";
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please start reShut Legacy without an debugger.");
                    Console.WriteLine("You are using an version built using the source, which could include malware.");
                    Console.WriteLine("However, you can look into the source code if you want to:");
                    Console.WriteLine(@"https://github.com/elNino0916/reShut-Legacy");
                    Console.WriteLine(@"Open an issue on github if you think that this is an mistake. Also attach the information below:");
                    Console.WriteLine(@"---");
                    Console.WriteLine(@"Information:");
                    Console.WriteLine(@"reShut " + variables.version);
                    Console.WriteLine(@"PreRelease: " + variables.prerelease);
                    Console.WriteLine(@"BuiltFromSource: " + variables.buildfromsource);
                    Console.WriteLine(@"Identifier: " + "dbg-" + HWID.GetHWID() + "-" + DateTime.Now.ToString("HH_mm_ss"));
                    Console.WriteLine(@"---");
                }
                Debugger.Break();
                Environment.Exit(0);
            }
            return false;
        }
    }
}
