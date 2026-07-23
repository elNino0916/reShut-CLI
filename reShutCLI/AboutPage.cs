using reShutCLI.Helpers;

namespace reShutCLI;

internal static class AboutPage
{
    public static void Show()
    {
        var prereleaseTag = Variables.IsPreRelease ? Localization.Get("PreRelease") : "";
        var header = "reShut CLI " + prereleaseTag + Variables.Version;
        var releaseStatus = $"{Localization.Get("PreRelease")}: {Localization.Get(Variables.IsPreRelease ? "Yes" : "No")}";
        var registryVersion = $"{Localization.Get("RegistryVersion")}: v{Variables.RegistryVersion}";
        var copyrightLine = "© 2023-2026 elNino0916 and contributors.";
        var githubLine = "https://github.com/elNino0916/reShut-CLI";

        Console.Clear();

        UIDraw.TextColor = new CliColor("#FF9A02");
        UIDraw.DrawCenteredLine("╔═══════════╗");
        UIDraw.DrawCenteredLine("║ ╭───────╮ ║");
        UIDraw.DrawCenteredLine("║ │ > _   │ ║");
        UIDraw.DrawCenteredLine("║ │       │ ║");
        UIDraw.DrawCenteredLine("║ ╰───────╯ ║");
        UIDraw.DrawCenteredLine("╚═══════════╝");
        UIDraw.DrawCenteredLine("reShut CLI v2");

        UIDraw.TextColor = Variables.MenuColor;
        UIDraw.DrawCenteredLine("");
        UIDraw.DrawCenteredLine("");
        UIDraw.DrawBoxedMessages([header, releaseStatus, registryVersion, copyrightLine, githubLine]);

        UIDraw.DrawBoxedMessage(Localization.Get("PressAnyKeyToGoBack"));
        Console.ReadKey(intercept: true);
        Console.Clear();
    }
}
