using System;
using System.Collections.Generic;
using System.Linq;

namespace reShutLegacy
{
    class CmdLine
    {
        public CmdLine(string[] args)
        {
            List<string> validPrefixes = ["-", "/"];

            foreach (string arg in args)
            {
                string prefix = validPrefixes.FirstOrDefault(p => arg.StartsWith(p));
                if (prefix != null)
                {
                    // Remove the prefix to get the option name
                    string option = arg[prefix.Length..].ToLower();

                    switch (option)
                    {
                        case "r":
                            Handler.Reboot();
                            break;
                        case "l":
                            Handler.Logoff();
                            break;
                        case "s":
                            Handler.Shutdown(); 
                            break;
                        default:
                            Console.WriteLine($"Unknown argument: {arg}");
                            break;
                    }
                }
                else
                {
                    // Handle arguments without a valid prefix
                    Console.WriteLine($"Unknown argument: {arg}");
                }
            }
        }
    }
}
