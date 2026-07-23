using reShutCLI.Helpers;
using reShutCLI.Services;

namespace reShutCLI;

/// <summary>Parses and executes command-line arguments.</summary>
internal sealed class CmdLine
{
    private static readonly string[] ValidPrefixes = ["-", "/"];

    public CmdLine(IEnumerable<string> args)
    {
        Dictionary<string, string?> parsed = new(StringComparer.OrdinalIgnoreCase);

        foreach (var arg in args)
        {
            var prefix = ValidPrefixes.FirstOrDefault(p => arg.StartsWith(p, StringComparison.Ordinal));
            if (prefix is null)
            {
                UIDraw.TextColor = Variables.SecondaryColor;
                UIDraw.DrawLine($"Missing Prefix in argument: {arg}");
                UIDraw.TextColor = ConsoleColor.Gray;
                Environment.Exit(Constants.ExitCodeSuccess);
            }

            var option = arg[prefix.Length..];
            var parts = option.Split([':', '='], 2);
            parsed[parts[0].ToLowerInvariant()] = parts.Length > 1 ? parts[1] : null;
        }

        if (parsed.TryGetValue("remote", out var host))
        {
            HandleRemote(parsed, host);
        }

        foreach (var key in parsed.Keys)
        {
            switch (key)
            {
                case "r":
                case "reboot":
                    PowerManager.Reboot();
                    Environment.Exit(Constants.ExitCodeError);
                    break;
                case "l":
                case "logoff":
                    PowerManager.Logoff();
                    Environment.Exit(Constants.ExitCodeError);
                    break;
                case "s":
                case "shutdown":
                case "poweroff":
                    PowerManager.Shutdown();
                    Environment.Exit(Constants.ExitCodeError);
                    break;
                case "help":
                case "?":
                case "h":
                    PrintHelp();
                    Environment.Exit(Constants.ExitCodeSuccess);
                    break;
                case "reset":
                    RegistryWorker.WriteToRegistry(Constants.RegistryPathBase, Constants.RegistryValueRegistryPopulated, Constants.RegistryValueTypeString, "0");
                    RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSetupComplete, Constants.RegistryValueTypeString, "0");
                    UIDraw.TextColor = Variables.SecondaryColor;
                    UIDraw.DrawLine("reShut CLI has been reset.");
                    UIDraw.TextColor = ConsoleColor.Gray;
                    Environment.Exit(Constants.ExitCodeSuccess);
                    break;
                default:
                    UIDraw.TextColor = Variables.SecondaryColor;
                    UIDraw.DrawLine($"Unknown argument: {key}");
                    UIDraw.TextColor = ConsoleColor.Gray;
                    Environment.Exit(Constants.ExitCodeSuccess);
                    break;
            }
        }
    }

    private static void HandleRemote(Dictionary<string, string?> parsed, string? host)
    {
        if (host is null)
        {
            UIDraw.TextColor = Variables.SecondaryColor;
            UIDraw.DrawLine("Remote host specified but no host value provided.");
            UIDraw.TextColor = ConsoleColor.Gray;
            Environment.Exit(Constants.ExitCodeError);
            return;
        }

        parsed.TryGetValue("user", out var user);
        parsed.TryGetValue("pass", out var pass);
        var reboot = parsed.ContainsKey("r") || parsed.ContainsKey("reboot");
        var shutdown = parsed.ContainsKey("s") || parsed.ContainsKey("shutdown") || parsed.ContainsKey("poweroff");
        bool result;

        if (reboot)
        {
            result = RemoteManager.Trigger(host, user, pass, true);
        }
        else if (shutdown)
        {
            result = RemoteManager.Trigger(host, user, pass, false);
        }
        else
        {
            UIDraw.TextColor = Variables.SecondaryColor;
            UIDraw.DrawLine("Remote host specified but no action (-r/-s) provided.");
            UIDraw.TextColor = ConsoleColor.Gray;
            result = false;
        }

        Environment.Exit(result ? Constants.ExitCodeSuccess : Constants.ExitCodeError);
    }

    private static void PrintHelp()
    {
        UIDraw.TextColor = Variables.LogoColor;
        UIDraw.DrawLine($"reShut CLI ({Variables.FullVersion})");
        UIDraw.TextColor = Variables.MenuColor;
        UIDraw.DrawLine("╭────────────────────────────────────────╮");
        UIDraw.DrawLine("│           Available Arguments:         │");
        UIDraw.DrawLine("├────────────────────────────────────────┤");
        UIDraw.DrawLine("│ [-s] [-shutdown] Shutdown this PC.     │");
        UIDraw.DrawLine("│ [-r] [-reboot] Reboot this PC.         │");
        UIDraw.DrawLine("│ [-l] [-logoff] Logout.                 │");
        UIDraw.DrawLine("│ [-h] [-help] Prints this information.  │");
        UIDraw.DrawLine("│ [-reset] Resets reShut CLI.            │");
        UIDraw.DrawLine("│ [-remote:HOST] [-user:USER] [-pass:PW] │");
        UIDraw.DrawLine("│    with -s or -r to manage remotely.   │");
        UIDraw.DrawLine("╰────────────────────────────────────────╯");
        UIDraw.TextColor = ConsoleColor.Gray;
    }
}
