namespace reShutCLI.Helpers;

/// <summary>
/// Console drawing helpers: colored output, centered lines and rounded boxes/menus.
/// </summary>
internal static class UIDraw
{
    private static CliColor _textColor;
    public static CliColor TextColor
    {
        get => _textColor;
        set
        {
            _textColor = value;
            _textColor.ApplyForeground();
        }
    }

    private static CliColor _backgroundColor;
    public static CliColor BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            _backgroundColor = value;
            _backgroundColor.ApplyBackground();
        }
    }

    public static void DrawLine(string text) => Console.WriteLine(text);

    public static void Draw(string text) => Console.Write(text);

    public static void DrawCenteredLine(string message) => DrawLine(Center(message));

    public static void DrawCentered(string message) => Draw(Center(message));

    private static string Center(string message)
    {
        var padding = (Console.WindowWidth - message.Length) / 2;
        return new string(' ', Math.Max(0, padding)) + message;
    }

    public static void DrawBoxedMessage(string message) => DrawBoxedMessages([message]);

    public static void DrawBoxedMessages(IEnumerable<string> messages)
    {
        var lines = messages.ToList();
        if (lines.Count == 0) return;

        var maxContentWidth = lines.Max(line => line.Length);

        DrawCenteredLine("╭" + new string('─', maxContentWidth + 2) + "╮");
        foreach (var line in lines)
        {
            DrawCenteredLine($"│ {line.PadRight(maxContentWidth)} │");
        }
        DrawCenteredLine("╰" + new string('─', maxContentWidth + 2) + "╯");
    }

    /// <summary>
    /// Draws a menu box with an optional centered title and one or more groups of
    /// items. Groups are separated by a horizontal divider.
    /// </summary>
    public static void DrawMenu(string? title, params IReadOnlyList<string>[] groups)
    {
        var allItems = groups.SelectMany(g => g).ToList();
        if (allItems.Count == 0) return;

        var innerWidth = Math.Max(title?.Length ?? 0, allItems.Max(item => item.Length)) + 2;
        if (innerWidth % 2 != 0) innerWidth++;

        var separator = "├" + new string('─', innerWidth) + "┤";

        DrawCenteredLine("╭" + new string('─', innerWidth) + "╮");

        if (title is not null)
        {
            var paddingLeft = (innerWidth - title.Length) / 2;
            DrawCenteredLine("│" + new string(' ', paddingLeft) + title +
                             new string(' ', innerWidth - title.Length - paddingLeft) + "│");
            DrawCenteredLine(separator);
        }

        for (var i = 0; i < groups.Length; i++)
        {
            if (i > 0) DrawCenteredLine(separator);
            foreach (var item in groups[i])
            {
                DrawCenteredLine("│ " + item.PadRight(innerWidth - 1) + "│");
            }
        }

        DrawCenteredLine("╰" + new string('─', innerWidth) + "╯");
    }
}
