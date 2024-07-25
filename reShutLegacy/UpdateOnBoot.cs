using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reShutCLI
{
    internal class UpdateOnBoot
    {
        public static void Enable()
        {
            string startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolderPath, "reShutCLI_Update.lnk");
            string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reShutCLI.exe");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = targetPath;
            shortcut.Arguments = "-update";
            shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
            shortcut.Description = "Updates reShut CLI on startup";
            shortcut.Save();
        }
        public static void Disable() 
        {
            string startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolderPath, "reShutCLI_Update.lnk");
            if (System.IO.File.Exists(shortcutPath))
            {
                System.IO.File.Delete(shortcutPath);
            }
            else
            {
            }
        }
    }
}
