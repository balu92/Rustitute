using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
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
                if (GlobalData.ContainsKey("Rustitute_disappearBlocks"))
                {
                    disappearBlocks = (List<GameObject>) GlobalData["Rustitute_disappearBlocks"];

                    foreach (var gameObject in disappearBlocks)
                    {
                        try
                        {
                            var block = gameObject.GetComponent<BuildingBlock>();

                            string unique = Unique(block.LookupPrefabName(), block.transform.position, block.transform.rotation);

                            DisappearItem state = new DisappearItem();

                            state.Block = block;
                            state.Prefab = state.Block.LookupPrefabName();
                            state.Location = state.Block.transform.position;
                            state.Rotation = state.Block.transform.rotation;
                            state.Grade = state.Block.grade;

                            disappearList.Add(state);
                            disappearUnique.Add(unique);
                        }
                        catch (Exception ex) { }
                    }
                    //disappearList = (List<DisappearItem>) GlobalData["Rustitute_disappearList"];
                    //disappearUnique = (List<string>) GlobalData["Rustitute_disappearUnique"];
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }

            lanternList = GetArenaLanterns();

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
