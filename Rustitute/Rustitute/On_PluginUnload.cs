using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_PluginUnload()
        {
            workTimer.Close();
            lanternTimer.Close();

            ini.Save();
            iniArena.Save();
        }
    }
}
