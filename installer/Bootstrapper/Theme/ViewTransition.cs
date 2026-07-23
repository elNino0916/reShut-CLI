using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace reShutCLI.Bootstrapper.Theme;

/// <summary>
/// Swaps the content of the shell's host with a short cross-fade, so moving between
/// setup steps reads as one continuous surface rather than a hard cut.
/// </summary>
internal static class ViewTransition
{
    private static readonly Duration FadeOut = new(TimeSpan.FromMilliseconds(130));

    public static void Show(ContentControl host, object view)
    {
        // Nothing to fade away from on the very first step.
        if (host.Content is null)
        {
            host.Content = view;
            return;
        }

        var fade = new DoubleAnimation(1, 0, FadeOut)
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn },
        };

        fade.Completed += (_, _) =>
        {
            host.Content = view;
            // Release the animation's hold on Opacity before restoring it, otherwise the
            // held value (0) keeps winning and the incoming view never becomes visible.
            host.BeginAnimation(UIElement.OpacityProperty, null);
            host.Opacity = 1;
        };

        host.BeginAnimation(UIElement.OpacityProperty, fade);
    }
}
