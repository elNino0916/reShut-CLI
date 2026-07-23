using reShutCLI.Helpers;

namespace reShutCLI.Services;

internal static class ConfigManager
{
    public static void Reset()
    {
        while (true)
        {
            Console.Clear();
            UIDraw.TextColor = Variables.SecondaryColor;
            UIDraw.DrawBoxedMessage(Localization.Get("ResetWarning"));
            UIDraw.DrawCenteredLine("");
            UIDraw.DrawMenu(null, ["1) Reset"], ["2) Cancel"]);
            UIDraw.TextColor = Variables.MenuColor;

            var confirmation = Console.ReadKey().KeyChar;
            UIDraw.TextColor = Variables.SecondaryColor;

            switch (confirmation)
            {
                case '1':
                    Console.Clear();
                    UIDraw.DrawBoxedMessage(Localization.Get("ResetProg"));
                    RegistryWorker.WriteToRegistry(Constants.RegistryPathBase, Constants.RegistryValueRegistryPopulated, Constants.RegistryValueTypeString, "0");
                    RegistryWorker.WriteToRegistry(Constants.RegistryPathConfig, Constants.RegistryValueSetupComplete, Constants.RegistryValueTypeString, "0");
                    Thread.Sleep(500);
                    AutoRestart.Init();
                    return;
                case '2':
                    return;
            }
        }
    }
}
