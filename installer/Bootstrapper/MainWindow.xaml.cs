using System.Diagnostics;
using System.IO;
using System.Windows;
using reShutCLI.Bootstrapper.Native;
using reShutCLI.Bootstrapper.Services;
using reShutCLI.Bootstrapper.Theme;
using reShutCLI.Bootstrapper.Views;

namespace reShutCLI.Bootstrapper;

public partial class MainWindow : Window
{
    private readonly InstallContext? _installContext;
    private readonly bool _isUninstall;

    public MainWindow(InstallContext context)
    {
        InitializeComponent();
        _installContext = context;
        SourceInitialized += OnSourceInitialized;
        Loaded += OnLoaded;
    }

    public MainWindow(UninstallContext _)
    {
        InitializeComponent();
        _isUninstall = true;
        SourceInitialized += OnSourceInitialized;
        Loaded += OnLoaded;
    }

    // DWM's dark title bar/rounded corners must be requested before the native window
    // first paints (SourceInitialized, when the HWND exists) - applying it in Loaded is
    // one frame too late and leaves the native caption briefly (or persistently) white.
    private void OnSourceInitialized(object? sender, EventArgs e) => DwmHelper.ApplyDarkRoundedChrome(this);

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // Uninstalling does not re-present the licence - it is only a gate on installing.
        if (_isUninstall) ShowUninstallConfirm();
        else ShowLicense();
    }

    private void OnMinimizeClick(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void OnCloseClick(object sender, RoutedEventArgs e) => Close();

    private void ShowLicense()
    {
        var view = new LicenseView();
        view.AcceptedRequested += ShowSetup;
        view.CancelRequested += Close;
        ViewTransition.Show(ContentHost, view);
    }

    private void ShowSetup()
    {
        var defaultDir = _installContext!.Options.InstallDir ?? InstallEngine.DefaultInstallDir;
        var view = new SetupView(defaultDir);
        view.InstallRequested += request => _ = RunInstallAsync(request);
        view.CancelRequested += Close;
        ViewTransition.Show(ContentHost, view);
    }

    private async Task RunInstallAsync(InstallRequest request)
    {
        var progressView = new ProgressView("Installing reShut CLI");
        ViewTransition.Show(ContentHost, progressView);
        CloseButton.IsEnabled = false;

        var progress = new Progress<InstallProgress>(p => progressView.SetProgress(p.Percent, p.Status));

        try
        {
            await InstallEngine.RunAsync(request, progress, CancellationToken.None);
            ShowInstallFinish(request);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, retry: () => _ = RunInstallAsync(request), canRetry: true);
        }
        finally
        {
            CloseButton.IsEnabled = true;
        }
    }

    private void ShowInstallFinish(InstallRequest request)
    {
        var view = new FinishView("Installation complete", "reShut CLI has been installed successfully.");
        view.CloseRequested += () =>
        {
            if (request.LaunchWhenFinished)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(Path.Combine(request.InstallDir, AppConstants.AppExeName)) { UseShellExecute = true });
                }
                catch
                {
                    // The user can still launch it manually; not worth blocking setup completion over.
                }
            }
            Close();
        };
        ViewTransition.Show(ContentHost, view);
    }

    private void ShowUninstallConfirm()
    {
        var view = new UninstallConfirmView();
        view.UninstallRequested += removeSettings => _ = RunUninstallAsync(removeSettings);
        view.CancelRequested += Close;
        ViewTransition.Show(ContentHost, view);
    }

    private async Task RunUninstallAsync(bool removeSettings)
    {
        var progressView = new ProgressView("Removing reShut CLI");
        ViewTransition.Show(ContentHost, progressView);
        CloseButton.IsEnabled = false;

        var progress = new Progress<InstallProgress>(p => progressView.SetProgress(p.Percent, p.Status));

        try
        {
            await UninstallEngine.RunAsync(removeSettings, progress, CancellationToken.None);
            var view = new FinishView("Uninstall complete", "reShut CLI has been removed from this computer.");
            view.CloseRequested += Close;
            ViewTransition.Show(ContentHost, view);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, retry: () => _ = RunUninstallAsync(removeSettings), canRetry: true);
        }
        finally
        {
            CloseButton.IsEnabled = true;
        }
    }

    private void ShowError(string message, Action? retry, bool canRetry)
    {
        var view = new ErrorView(message, canRetry);
        if (retry is not null) view.RetryRequested += retry;
        view.CloseRequested += Close;
        ViewTransition.Show(ContentHost, view);
    }
}
