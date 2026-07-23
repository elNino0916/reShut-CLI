using System.Text;

namespace reShutCLI.Helpers;

internal static class ErrorHandler
{
    /// <summary>
    /// Displays a boxed error message. When <paramref name="needsRestart"/> is true the
    /// application restarts after a key press; otherwise control returns to the caller.
    /// </summary>
    public static void ShowError(string error, bool needsRestart)
    {
        Console.Title = "reShut CLI";
        Console.OutputEncoding = Encoding.UTF8;
        Console.Clear();

        UIDraw.TextColor = Variables.LogoColor;
        UIDraw.DrawBoxedMessage(Localization.Get("ErrorHandler_BeingImproved"));
        UIDraw.DrawLine(" ");

        UIDraw.TextColor = Variables.SecondaryColor;
        var lines = new List<string> { Localization.Get("ErrorOccurred"), error };
        if (needsRestart) lines.Add(Localization.Get("NeedRestart"));
        lines.Add(Localization.Get(needsRestart ? "ErrorHandler_PressKeyRestart" : "ErrorHandler_PressKeyBack"));
        UIDraw.DrawBoxedMessages(lines);

        Console.ReadKey(intercept: true);

        if (needsRestart)
        {
            AutoRestart.Init();
        }
        else
        {
            Console.Clear();
        }
    }
}
