using System.Windows;
using System.Windows.Controls;

namespace reShutCLI.Bootstrapper.Views;

public partial class ErrorView : UserControl
{
    public event Action? RetryRequested;
    public event Action? CloseRequested;

    public ErrorView(string message, bool canRetry = true)
    {
        InitializeComponent();
        MessageText.Text = message;
        RetryButton.Visibility = canRetry ? Visibility.Visible : Visibility.Collapsed;
    }

    private void OnRetryClick(object sender, RoutedEventArgs e) => RetryRequested?.Invoke();
    private void OnCloseClick(object sender, RoutedEventArgs e) => CloseRequested?.Invoke();
}
