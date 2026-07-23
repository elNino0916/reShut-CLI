using System.Windows;
using System.Windows.Controls;

namespace reShutCLI.Bootstrapper.Views;

public partial class UninstallConfirmView : UserControl
{
    public event Action<bool>? UninstallRequested;
    public event Action? CancelRequested;

    public UninstallConfirmView() => InitializeComponent();

    private void OnCancelClick(object sender, RoutedEventArgs e) => CancelRequested?.Invoke();

    private void OnUninstallClick(object sender, RoutedEventArgs e) =>
        UninstallRequested?.Invoke(RemoveSettingsCheck.IsChecked == true);
}
