﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reShutLegacy
{
    internal class StartupSettings
    {
        public static void Show()
        {
        // NEW: Startup Settings
        invalidstartup:
            Console.Clear();

            Console.Clear();
            if (!File.Exists(@"config\startup.cfg"))
            {
                Directory.CreateDirectory("config");
                File.WriteAllText(@"config\startup.cfg", string.Empty);
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            if (!File.Exists(@"config\update.cfg"))
                File.WriteAllText(@"config\update.cfg", "search=enabled");
            if (File.ReadAllText(@"config\startup.cfg") == "fast=disabled")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("╭───────────────────────────╮");
                Console.WriteLine("│ Fast Startup is disabled. │");
                Console.WriteLine("╰───────────────────────────╯");
            }
            else
            {
                if (File.ReadAllText(@"config\startup.cfg") == "fast=enabled")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("╭──────────────────────────╮");
                    Console.WriteLine("│ Fast Startup is enabled. │");
                    Console.WriteLine("╰──────────────────────────╯");
                }
            }

            if (File.ReadAllText(@"config\update.cfg") == "search=disabled")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("╭────────────────────────────╮");
                Console.WriteLine("│ Update Search is disabled. │");
                Console.WriteLine("╰────────────────────────────╯");
            }
            else
            {
                if (File.ReadAllText(@"config\update.cfg") == "search=enabled")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("╭───────────────────────────╮");
                    Console.WriteLine("│ Update Search is enabled. │");
                    Console.WriteLine("╰───────────────────────────╯");
                }
            }

            Console.ForegroundColor = Variables.MenuColor;
            Console.WriteLine("╭───────────────────────────────────────╮");
            Console.WriteLine("│                Startup:               │");
            Console.WriteLine("├───────────────────────────────────────┤");
            Console.WriteLine("│ 1) Enable Fast Startup                │");
            Console.WriteLine("│ 2) Disable Fast Startup (recommended) │");
            Console.WriteLine("├───────────────────────────────────────┤");
            Console.WriteLine("│ 4) Enable Update Search (recommended) │");
            Console.WriteLine("│ 5) Disable Update Search              │");
            Console.WriteLine("├───────────────────────────────────────┤");
            Console.WriteLine("│ 9) Back                               │");
            Console.WriteLine("╰───────────────────────────────────────╯");
            var setInfoS = Console.ReadKey();
            var setS = setInfoS.KeyChar.ToString();
            switch (setS)
            {
                case "1":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("╭──────────────────────────────╮");
                    Console.WriteLine("│ Successfully saved settings! │");
                    Console.WriteLine("╰──────────────────────────────╯");
                    File.WriteAllText(@"config\startup.cfg", "fast=enabled");
                    goto invalidstartup;
                case "9":
                    Console.Clear();
                    return;
                case "2":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("╭──────────────────────────────╮");
                    Console.WriteLine("│ Successfully saved settings! │");
                    Console.WriteLine("╰──────────────────────────────╯");
                    File.WriteAllText(@"config\startup.cfg", "fast=disabled");
                    goto invalidstartup;
                case "4":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("╭──────────────────────────────╮");
                    Console.WriteLine("│ Successfully saved settings! │");
                    Console.WriteLine("╰──────────────────────────────╯");
                    File.WriteAllText(@"config\update.cfg", "search=enabled");
                    goto invalidstartup;
                case "5":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("╭──────────────────────────────╮");
                    Console.WriteLine("│ Successfully saved settings! │");
                    Console.WriteLine("╰──────────────────────────────╯");
                    File.WriteAllText(@"config\update.cfg", "search=disabled");
                    goto invalidstartup;
                default:
                    goto invalidstartup;
            }
        }
    } 
}