namespace reShutCLI.Services;

/// <summary>
/// Shared <see cref="HttpClient"/> for all web requests (update checks, theme API,
/// installer downloads). A single instance avoids socket exhaustion and gets a
/// consistent user agent and timeout.
/// </summary>
internal static class Http
{
    public static HttpClient Client { get; } = CreateClient();

    private static HttpClient CreateClient()
    {
        var client = new HttpClient { Timeout = TimeSpan.FromSeconds(100) };
        client.DefaultRequestHeaders.UserAgent.ParseAdd($"reShutCLI/{Variables.Version}");
        return client;
    }
}
