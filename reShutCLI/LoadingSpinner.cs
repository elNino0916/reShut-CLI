using System;
using System.Threading;
using System.Threading.Tasks;

namespace reShutCLI
{
    internal static class LoadingSpinner
    {
        public static async Task RunAsync(Func<Task> action, string message, int delay = 1000)
        {
            await RunAsync(async () => { await action(); return true; }, message, delay);
        }

        public static async Task<T> RunAsync<T>(Func<Task<T>> action, string message, int delay = 1000)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            using var cts = new CancellationTokenSource();

            var spinnerTask = Task.Run(async () =>
            {
                char[] frames = { '⠋', '⠙', '⠹', '⠸', '⠼', '⠴', '⠦', '⠧', '⠇', '⠏' };
                int i = 0;
                while (!cts.Token.IsCancellationRequested)
                {
                    UIDraw.DrawCentered($"\r{frames[i++ % frames.Length]} {message}");
                    await Task.Delay(100, cts.Token).ContinueWith(_ => { });
                }
            }, cts.Token);

            var actionTask = action();
            await Task.WhenAll(actionTask, Task.Delay(delay));

            cts.Cancel();
            await spinnerTask;
            Console.Clear();
            Console.ForegroundColor = previousColor;
            return await actionTask;
        }
    }
}
