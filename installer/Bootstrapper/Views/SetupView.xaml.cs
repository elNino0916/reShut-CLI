using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using reShutCLI.Bootstrapper.Native;
using reShutCLI.Bootstrapper.Services;

namespace reShutCLI.Bootstrapper.Views;

public partial class SetupView : UserControl
{
    public event Action<InstallRequest>? InstallRequested;
    public event Action? CancelRequested;

    public SetupView(string defaultInstallDir)
    {
        InitializeComponent();
        SetPathText(defaultInstallDir);
    }

    // Setting .Text alone can leave the box scrolled to an arbitrary position;
    // explicitly parking the caret at the start shows the beginning of the path,
    // and the box (being a normal scrollable TextBox) can then be scrolled to see the rest.
    private void SetPathText(string path)
    {
        PathBox.Text = path;
        PathBox.CaretIndex = 0;
        PathBox.ScrollToHorizontalOffset(0);
    }

    private void OnBrowseClick(object sender, RoutedEventArgs e)
    {
        var owner = new WindowInteropHelper(Window.GetWindow(this)!).Handle;
        var picked = FolderPickerDialog.Show(owner, PathBox.Text, "Select the install location");
        if (picked is not null)
        {
            SetPathText(picked);
        }
    }

    private void OnCancelClick(object sender, RoutedEventArgs e) => CancelRequested?.Invoke();

    private void OnInstallClick(object sender, RoutedEventArgs e)
    {
        var dir = PathBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(dir)) return;

        InstallRequested?.Invoke(new InstallRequest(dir, ShortcutCheck.IsChecked == true, LaunchCheck.IsChecked == true));
    }
}
