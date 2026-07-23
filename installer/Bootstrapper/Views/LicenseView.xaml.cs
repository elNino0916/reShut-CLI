using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace reShutCLI.Bootstrapper.Views;

public partial class LicenseView : UserControl
{
    public event Action? AcceptedRequested;
    public event Action? CancelRequested;

    public LicenseView()
    {
        InitializeComponent();
        LicenseText.Text = LoadLicense();
    }

    /// <summary>
    /// Reads the LICENSE embedded as a WPF resource by the project file.
    /// </summary>
    private static string LoadLicense()
    {
        try
        {
            var resource = Application.GetResourceStream(new Uri("pack://application:,,,/Assets/LICENSE.txt"));
            if (resource is null) return FallbackText;

            using var reader = new StreamReader(resource.Stream);
            return reader.ReadToEnd();
        }
        catch
        {
            // Never block the install on a missing resource - point at the canonical copy instead.
            return FallbackText;
        }
    }

    private static string FallbackText =>
        "The license text could not be loaded from this installer.\r\n\r\n" +
        $"reShut CLI is licensed under CC BY-NC-SA 4.0. The full terms are available at\r\n{AppConstants.ProjectUrl}\r\n\r\n" +
        "Continuing the installation means you accept those terms.";

    private void OnAcceptChanged(object sender, RoutedEventArgs e) =>
        ContinueButton.IsEnabled = AcceptCheck.IsChecked == true;

    private void OnCancelClick(object sender, RoutedEventArgs e) => CancelRequested?.Invoke();

    private void OnContinueClick(object sender, RoutedEventArgs e)
    {
        if (AcceptCheck.IsChecked != true) return;
        AcceptedRequested?.Invoke();
    }
}
