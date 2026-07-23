using System.Text.Json.Serialization;

namespace reShutCLI.Services;

/// <summary>A GitHub release as returned by the releases API.</summary>
internal sealed class GitHubRelease
{
    [JsonPropertyName("tag_name")]
    public string? TagName { get; set; }

    [JsonPropertyName("assets")]
    public GitHubAsset[] Assets { get; set; } = [];
}

internal sealed class GitHubAsset
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("browser_download_url")]
    public string? BrowserDownloadUrl { get; set; }
}

/// <summary>Theme colors served by the elNino0916 theme API.</summary>
internal sealed class ApiTheme
{
    public string? MenuColor { get; set; }
    public string? LogoColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? ThemeName { get; set; }
}

/// <summary>Source-generated JSON serialization context (trim-safe, reflection-free).</summary>
[JsonSerializable(typeof(GitHubRelease))]
[JsonSerializable(typeof(ApiTheme))]
internal sealed partial class ApiJsonContext : JsonSerializerContext;
