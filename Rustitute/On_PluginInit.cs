using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_PluginInit()
        {
            if (!Plugin.IniExists(pluginIni))
                Plugin.CreateIni(pluginIni);

            if (!Plugin.IniExists(pluginIni + "Arena"))
                Plugin.CreateIni(pluginIni + "Arena");

            ini = Plugin.GetIni(pluginIni);
            iniArena = Plugin.GetIni(pluginIni + "Arena");

            LoadSettings();

            // :(
            //lanternList = GetArenaLanterns();

            workTimer.Interval = 1000 * 60 * 5;
            workTimer.Elapsed += (sender, e) => { Work(); };
            workTimer.Enabled = true;

            lanternTimer.Interval = 1000 * 5;
            lanternTimer.Elapsed += (sender, e) => { CheckLanterns(); };
            lanternTimer.Enabled = true;
        }
    }
}
