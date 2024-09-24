using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

namespace reShutCLI
{
    internal class CmdLine
    {
        [SupportedOSPlatform("windows")]
        public CmdLine(IEnumerable<string> args)
        {
            List<string> validPrefixes = ["-", "/"];

            foreach (var arg in args)
            {
                var prefix = validPrefixes.FirstOrDefault(p => arg.StartsWith(p));
                if (prefix != null)
                {
                    // Remove the prefix to get the option name
                    var option = arg[prefix.Length..].ToLower();

                    switch (option)
                    {
                        case "r":
                        case "reboot":
                            Handler.Reboot();
                            Environment.Exit(1);
                            break;
                        case "l":
                        case "logoff":
                            Handler.Logoff();
                            Environment.Exit(1);
                            break;
                        case "s":
                        case "shutdown":
                        case "poweroff":
                            Handler.Shutdown();
                            Environment.Exit(1);
                            break;
                        case "help":
                        case "?":
                        case "h":
                            Console.ForegroundColor = Variables.LogoColor;
                            Console.WriteLine($"reShut CLI ({Variables.fullversion})");
                            Console.ForegroundColor = Variables.MenuColor;
                            Console.WriteLine("╭────────────────────────────────────────╮");
                            Console.WriteLine("│           Available Arguments:         │");
                            Console.WriteLine("├────────────────────────────────────────┤");
                            Console.WriteLine("│ [-s] [-shutdown] Shutdown this PC.     │");
                            Console.WriteLine("│ [-r] [-reboot] Reboot this PC.         │");
                            Console.WriteLine("│ [-l] [-logoff] Logout.                 │");
                            Console.WriteLine("│ [-h] [-help] Prints this information.  │");
                            Console.WriteLine("│ [-reset] Resets reShut CLI.            │");
                            Console.WriteLine("╰────────────────────────────────────────╯");
                            Console.ForegroundColor = Variables.SecondaryColor;
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Environment.Exit(0);
                            break;
                        case "reset":
                            RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryPopulated", "STRING", "0");
                            RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SetupComplete", "STRING", "0");
                            Console.ForegroundColor = Variables.SecondaryColor;
                            Console.WriteLine("reShut CLI has been reset.");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Environment.Exit(0);
                            break;
                        default:
                            Console.ForegroundColor = Variables.SecondaryColor;
                            Console.WriteLine($"Unknown argument: {arg}");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Environment.Exit(0);
                            break;
                    }
                }
                else
                {
                    // Handle arguments without a valid prefix
                    Console.ForegroundColor = Variables.SecondaryColor;
                    Console.WriteLine($"Missing Prefix in argument: {arg}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Environment.Exit(0);
                }
            }
        }
    }
}
