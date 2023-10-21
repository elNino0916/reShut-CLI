﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reShutLegacy
{
    class CmdLine
    {
        public bool Restart { get; private set; }
        public bool Logoff { get; private set; }
        public bool Shutdown { get; private set; }

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
                        case "reboot":
                            Handler.Reboot();
                            break;
                        case "logoff":
                            Handler.Logoff();
                            break;
                        case "shutdown":
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
