using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace reShutCLI
{
    internal class SoundManager
    {
        public static void PlayWelcome(bool ignoreSoundsDisabled)
        {
            if (ignoreSoundsDisabled)
            {
                PlaySoundFromFile("welcome.wav");
            }
            else
            {
                if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableSounds") == "1") { PlaySoundFromFile("welcome.wav"); }
            }

        }
        public static void PlayError(bool ignoreSoundsDisabled)
        {
            if (ignoreSoundsDisabled)
            {
                PlaySoundFromFile("error.wav");
            }
            else
            {
                if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableSounds") == "1") { PlaySoundFromFile("error.wav"); }
            }

        }
        public static void PlayReboot(bool ignoreSoundsDisabled)
        {
            if (ignoreSoundsDisabled)
            {
                PlaySoundFromFile("reboot.wav");
            }
            else
            {
                if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableSounds") == "1") { PlaySoundFromFile("reboot.wav"); }
            }
        }
        public static void PlayShutdown(bool ignoreSoundsDisabled)
        {

            if (ignoreSoundsDisabled)
            {
                
                PlaySoundFromFile("poweroff.wav");
            }
            else
            {
                if (RegistryWorker.ReadFromRegistry(@"HKEY_CURRENT_USER\Software\elNino0916\reShutCLI\config", "EnableSounds") == "1") { PlaySoundFromFile("poweroff.wav"); }
            }
        }
        private static void PlaySoundFromFile(string filePath)
        {
            try
            {
                using (SoundPlayer player = new SoundPlayer(filePath))
                {
                    player.Play(); // Play sound async
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
