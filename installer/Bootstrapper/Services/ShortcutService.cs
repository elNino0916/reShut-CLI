using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace reShutCLI.Bootstrapper.Services;

/// <summary>
/// Creates .lnk shortcuts via the WScript.Shell COM component, late-bound through
/// reflection so no COM reference/tlbimp step is needed in the project.
/// </summary>
internal static class ShortcutService
{
    public static void CreateShortcut(string shortcutPath, string targetPath, string? workingDirectory = null, string? iconPath = null)
    {
        var shellType = Type.GetTypeFromProgID("WScript.Shell")
                         ?? throw new InvalidOperationException("WScript.Shell COM component is unavailable.");
        var shell = Activator.CreateInstance(shellType)!;
        try
        {
            var shortcut = shellType.InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, null, shell, [shortcutPath])!;
            var shortcutType = shortcut.GetType();
            try
            {
                shortcutType.InvokeMember("TargetPath", BindingFlags.SetProperty, null, shortcut, [targetPath]);
                shortcutType.InvokeMember("WorkingDirectory", BindingFlags.SetProperty, null, shortcut,
                    [workingDirectory ?? Path.GetDirectoryName(targetPath) ?? ""]);
                if (iconPath is not null)
                    shortcutType.InvokeMember("IconLocation", BindingFlags.SetProperty, null, shortcut, [iconPath]);
                shortcutType.InvokeMember("Save", BindingFlags.InvokeMethod, null, shortcut, null);
            }
            finally
            {
                Marshal.ReleaseComObject(shortcut);
            }
        }
        finally
        {
            Marshal.ReleaseComObject(shell);
        }
    }
}
