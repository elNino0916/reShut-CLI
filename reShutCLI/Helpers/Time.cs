namespace reShutCLI.Helpers;

internal static class Time
{
    public static string GetTime(bool use24HoursFormat) =>
        DateTime.Now.ToString(use24HoursFormat ? "HH:mm:ss" : "hh:mm:ss tt");
}
