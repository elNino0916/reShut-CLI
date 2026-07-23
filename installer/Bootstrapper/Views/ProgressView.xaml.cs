using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace reShutCLI.Bootstrapper.Views;

public partial class ProgressView : UserControl
{
    private string _status = string.Empty;

    public ProgressView(string title)
    {
        InitializeComponent();
        TitleText.Text = title;

        // The label reads the bar's own animated value rather than the reported one, so the
        // number counts up in step with the fill instead of snapping ahead of it.
        Bar.ValueChanged += (_, e) => PercentText.Text = $"{(int)Math.Round(e.NewValue)}%";
    }

    public void SetProgress(double percent, string status)
    {
        percent = Math.Max(0, Math.Min(100, percent));

        Bar.BeginAnimation(RangeBase.ValueProperty, new DoubleAnimation(percent, TimeSpan.FromMilliseconds(400))
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut },
        });

        // Steps report progress far more often than they change their label; re-fading the
        // same text on every tick would leave it permanently flickering.
        if (status == _status) return;

        _status = status;
        StatusText.Text = status;
        StatusText.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(280)));
    }
}
