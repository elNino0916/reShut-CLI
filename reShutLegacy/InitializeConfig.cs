using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reShutCLI
{
    internal class InitializeConfig
    {
        public static void Initialize(string component)
        {
            if (component == "eula")
            {
                if (!File.Exists(@"config\eula.cfg"))
                {
                    File.WriteAllText(@"config\eula.cfg", "NotAccepted");
                }
            }
            else 
            {
                return;
            }
        }
    }
}
