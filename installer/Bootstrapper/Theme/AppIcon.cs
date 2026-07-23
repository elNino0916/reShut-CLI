using System.Linq;
using System.Windows.Media.Imaging;

namespace reShutCLI.Bootstrapper.Theme;

/// <summary>
/// The app .ico embeds several resolutions up to 256x256, but Image.Source's default
/// frame selection tends to pick a small one (e.g. 32x32) and upscale it blockily at
/// high DPI. This explicitly grabs the largest available frame instead.
/// </summary>
internal static class AppIcon
{
    public static BitmapSource Large { get; } = LoadLargestFrame();

    private static BitmapSource LoadLargestFrame()
    {
        var uri = new Uri("pack://application:,,,/Assets/app.ico");
        var decoder = new IconBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
        return decoder.Frames.OrderByDescending(f => f.PixelWidth).First();
    }
}
