using System.Windows;
using System.Windows.Controls;

namespace reShutCLI.Bootstrapper.Views;

public partial class FinishView : UserControl
{
    public event Action? CloseRequested;

    public FinishView(string title, string message)
    {
        InitializeComponent();
        TitleText.Text = title;
        MessageText.Text = message;
    }

    private void OnFinishClick(object sender, RoutedEventArgs e) => CloseRequested?.Invoke();
}
