using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace reShutCLI.Bootstrapper.Theme;

/// <summary>
/// Attached property that fades an element up into place once it loads.
/// Giving the elements of a view increasing delays (<c>theme:Reveal.Delay="0.06"</c>,
/// <c>"0.12"</c>, ...) staggers them into a single wave instead of a flat pop-in.
/// </summary>
public static class Reveal
{
    private static readonly Duration Length = new(TimeSpan.FromMilliseconds(420));

    public static readonly DependencyProperty DelayProperty = DependencyProperty.RegisterAttached(
        "Delay", typeof(double), typeof(Reveal), new PropertyMetadata(double.NaN, OnDelayChanged));

    /// <summary>
    /// How far below its final position an element starts, in device-independent pixels.
    /// Elements flush against the window edge (the footer bar) want 0 - anything else
    /// starts them outside the window, where they are clipped rather than sliding.
    /// </summary>
    public static readonly DependencyProperty RiseProperty = DependencyProperty.RegisterAttached(
        "Rise", typeof(double), typeof(Reveal), new PropertyMetadata(14.0));

    public static void SetDelay(DependencyObject target, double value) => target.SetValue(DelayProperty, value);

    public static double GetDelay(DependencyObject target) => (double)target.GetValue(DelayProperty);

    public static void SetRise(DependencyObject target, double value) => target.SetValue(RiseProperty, value);

    public static double GetRise(DependencyObject target) => (double)target.GetValue(RiseProperty);

    private static void OnDelayChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
    {
        if (target is not FrameworkElement element || double.IsNaN((double)e.NewValue)) return;

        // Hide immediately, at parse time - waiting for Loaded would let the element
        // paint once at full opacity before the reveal takes over, which flickers.
        element.Opacity = 0;

        // Loaded fires again whenever the view is swapped back into the content host,
        // so the reveal simply replays - which is what a re-entered view should do.
        element.Loaded += (_, _) => Play(element);
        if (element.IsLoaded) Play(element);
    }

    private static void Play(FrameworkElement element)
    {
        var begin = TimeSpan.FromSeconds(Math.Max(0, GetDelay(element)));
        var ease = new CubicEase { EasingMode = EasingMode.EaseOut };

        element.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0, 1, Length)
        {
            BeginTime = begin,
            EasingFunction = ease,
        });

        var rise = GetRise(element);
        if (rise == 0) return;

        var slide = new TranslateTransform();
        element.RenderTransform = slide;

        slide.BeginAnimation(TranslateTransform.YProperty, new DoubleAnimation(rise, 0, Length)
        {
            BeginTime = begin,
            EasingFunction = ease,
        });
    }
}
