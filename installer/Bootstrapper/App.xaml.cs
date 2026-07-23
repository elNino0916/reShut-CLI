using System.IO;
using System.Reflection;
using System.Windows;
using reShutCLI.Bootstrapper.Services;
using reShutCLI.Bootstrapper.Views;

namespace reShutCLI.Bootstrapper;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        var options = LaunchOptions.Parse(e.Args);

        // .NET Framework 4.8 has no Environment.ProcessPath (added in .NET 6);
        // the executing assembly's own location is the net48-safe equivalent.
        var currentExePath = Assembly.GetExecutingAssembly().Location;

        // Launched as "uninstall.exe", or explicitly asked to uninstall: this
        // is the same exe used both ways, matching the previous NSIS pattern.
        var isUninstall = options.Uninstall ||
                           string.Equals(Path.GetFileName(currentExePath), AppConstants.UninstallExeName, StringComparison.OrdinalIgnoreCase);

        if (isUninstall)
        {
            RunUninstall(options);
            return;
        }

        if (options.Silent)
        {
            RunSilentInstall(options);
            return;
        }

        var window = new MainWindow(new InstallContext(options));
        MainWindow = window;
        ShutdownMode = ShutdownMode.OnMainWindowClose;
        window.Show();
    }

    private void RunSilentInstall(LaunchOptions options)
    {
        var exitCode = 0;
        try
        {
            var request = new InstallRequest(
                InstallDir: options.InstallDir ?? InstallEngine.DefaultInstallDir,
                CreateShortcut: true,
                LaunchWhenFinished: false);
            InstallEngine.RunAsync(request, progress: null, CancellationToken.None).GetAwaiter().GetResult();
        }
        catch
        {
            exitCode = 1;
        }
        Shutdown(exitCode);
    }

    private void RunUninstall(LaunchOptions options)
    {
        if (!options.Silent)
        {
            var window = new MainWindow(new UninstallContext());
            MainWindow = window;
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            window.Show();
            return;
        }

        var exitCode = 0;
        try
        {
            UninstallEngine.RunAsync(removeUserSettings: false, new Progress<InstallProgress>(), CancellationToken.None)
                .GetAwaiter().GetResult();
        }
        catch
        {
            exitCode = 1;
        }
        Shutdown(exitCode);
    }
}
