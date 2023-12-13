using System;
using System.Collections.Generic;
using System.Linq;

namespace reShutLegacy
{
    class CmdLine
    {
        public CmdLine(string[] args)
        {
            // Define the prefixes that you want to support
            List<string> validPrefixes = new List<string> { "-", "/" };

            foreach (string arg in args)
            {
                // Check if the argument starts with a valid prefix
                string prefix = validPrefixes.FirstOrDefault(p => arg.StartsWith(p));
                if (prefix != null)
                {
                    // Remove the prefix to get the option name
                    string option = arg.Substring(prefix.Length).ToLower();

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
                        case "schedule":
                            Console.WriteLine("Not implemented yet.");
                            break;
                        default:
                            Console.WriteLine($"Unknown argument: {arg}");
                            break;
                    }
                }
                else
                {
                    // Handle arguments without a valid prefix as needed
                    Console.WriteLine($"Unknown argument: {arg}");
                }
            }
        }
    }
}
