using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

namespace reShutCLI
{
    internal class CmdLine
    {

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
                            UIDraw.TextColor = Variables.LogoColor;
                            UIDraw.DrawLine($"reShut CLI ({Variables.fullversion})");
                            UIDraw.TextColor = Variables.MenuColor;
                            UIDraw.DrawLine("╭────────────────────────────────────────╮");
                            UIDraw.DrawLine("│           Available Arguments:         │");
                            UIDraw.DrawLine("├────────────────────────────────────────┤");
                            UIDraw.DrawLine("│ [-s] [-shutdown] Shutdown this PC.     │");
                            UIDraw.DrawLine("│ [-r] [-reboot] Reboot this PC.         │");
                            UIDraw.DrawLine("│ [-l] [-logoff] Logout.                 │");
                            UIDraw.DrawLine("│ [-h] [-help] Prints this information.  │");
                            UIDraw.DrawLine("│ [-reset] Resets reShut CLI.            │");
                            UIDraw.DrawLine("╰────────────────────────────────────────╯");
                            UIDraw.TextColor = Variables.SecondaryColor;
                            UIDraw.TextColor = ConsoleColor.Gray;
                            Environment.Exit(0);
                            break;
                        case "reset":
                            RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI", "RegistryPopulated", "STRING", "0");
                            RegistryWorker.WriteToRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "SetupComplete", "STRING", "0");
                            UIDraw.TextColor = Variables.SecondaryColor;
                            UIDraw.DrawLine("reShut CLI has been reset.");
                            UIDraw.TextColor = ConsoleColor.Gray;
                            Environment.Exit(0);
                            break;
                        default:
                            UIDraw.TextColor = Variables.SecondaryColor;
                            UIDraw.DrawLine($"Unknown argument: {arg}");
                            UIDraw.TextColor = ConsoleColor.Gray;
                            Environment.Exit(0);
                            break;
                    }
                }
                else
                {
                    // Handle arguments without a valid prefix
                    UIDraw.TextColor = Variables.SecondaryColor;
                    UIDraw.DrawLine($"Missing Prefix in argument: {arg}");
                    UIDraw.TextColor = ConsoleColor.Gray;
                    Environment.Exit(0);
                }
            }
        }
    }
}
