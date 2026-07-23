using System.Diagnostics;

namespace reShutCLI.Bootstrapper.Services;

/// <summary>
/// .NET Framework 4.8 has no Process.WaitForExitAsync (added in .NET 5), so this
/// wraps the classic blocking WaitForExit on a background thread instead.
/// </summary>
internal static class ProcessExtensions
{
    public static Task WaitForExitAsync(this Process process, CancellationToken ct = default) =>
        Task.Run(() => process.WaitForExit(), ct);
}
