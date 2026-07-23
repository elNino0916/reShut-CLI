using System.Globalization;

namespace reShutCLI.Helpers;

/// <summary>
/// A console color that is either a classic <see cref="ConsoleColor"/> or a
/// 24-bit hex color rendered via ANSI escape sequences.
/// </summary>
internal readonly struct CliColor
{
    private const char Esc = (char)27;

    private readonly string? _hex;
    private readonly ConsoleColor? _consoleColor;

    public CliColor(ConsoleColor consoleColor)
    {
        _consoleColor = consoleColor;
        _hex = null;
    }

    public CliColor(string? color)
    {
        if (!string.IsNullOrEmpty(color) && color.StartsWith('#'))
        {
            _hex = color;
            _consoleColor = null;
        }
        else if (Enum.TryParse<ConsoleColor>(color, true, out var cc))
        {
            _consoleColor = cc;
            _hex = null;
        }
        else
        {
            _consoleColor = ConsoleColor.White;
            _hex = null;
        }
    }

    public void ApplyForeground()
    {
        if (_consoleColor.HasValue)
        {
            Console.ForegroundColor = _consoleColor.Value;
        }
        else if (TryParseHex(out var r, out var g, out var b))
        {
            Console.Write($"{Esc}[38;2;{r};{g};{b}m");
        }
    }

    public void ApplyBackground()
    {
        if (_consoleColor.HasValue)
        {
            Console.BackgroundColor = _consoleColor.Value;
        }
        else if (TryParseHex(out var r, out var g, out var b))
        {
            Console.Write($"{Esc}[48;2;{r};{g};{b}m");
        }
    }

    private bool TryParseHex(out int r, out int g, out int b)
    {
        r = g = b = 0;
        var hex = _hex?.TrimStart('#');
        if (hex is not { Length: 6 }) return false;

        return int.TryParse(hex.AsSpan(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out r)
               && int.TryParse(hex.AsSpan(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out g)
               && int.TryParse(hex.AsSpan(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out b);
    }

    public override string ToString() => _consoleColor?.ToString() ?? _hex ?? "";

    public static implicit operator CliColor(ConsoleColor color) => new(color);
    public static implicit operator CliColor(string color) => new(color);
}
