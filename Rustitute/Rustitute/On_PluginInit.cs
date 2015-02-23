using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

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

            try
            {
                if (GlobalData.ContainsKey("Rustitute_disappearList"))
                {
                    disappearList = (List<DisappearItem>) GlobalData["Rustitute_disappearList"];
                    disappearUnique = (List<string>) GlobalData["Rustitute_disappearUnique"];
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }

            // :(
            //lanternList = GetArenaLanterns();

            workTimer.Interval = 1000 * 60 * 5;
            workTimer.Elapsed += (sender, e) => { Work(); };
            workTimer.Enabled = true;

            lanternTimer.Interval = 1000 * 5;
            lanternTimer.Elapsed += (sender, e) => { CheckLanterns(); };
            lanternTimer.Enabled = true;

            disappearTimer.Interval = 1000 * 3;
            disappearTimer.Elapsed += (sender, e) => { DisappearTimer(); };
            disappearTimer.Enabled = true;

            campingTimer.Interval = 1000 * 15;
            campingTimer.Elapsed += (sender, e) => { CampingTimer(); };
            campingTimer.Enabled = true;
        }
    }
}
