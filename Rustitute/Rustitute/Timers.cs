using System;
using Pluton;
using UnityEngine;

namespace Rustitute
{
    partial class Rustitute
    {
        private class DisappearItem
        {
            public string Prefab { get; set; }
            public Vector3 Location { get; set; }
            public Quaternion Rotation { get; set; }
            public BuildingBlock Block { get; set; }
            public int Grade { get; set; }
        }


        private class BuildingPartTimer
        {
            public BuildingPart part { get; set; }
            public System.Threading.Timer Timer { get; set; }
        }

        private void Work()
        {
            for (var i = 0; i < Server.ActivePlayers.Count; i++)
            {
                Player player = Server.ActivePlayers[i];
                bool hasGod = GetSettingBool("user_" + player.SteamID, "god");
                bool hasKO = GetSettingBool("user_" + player.SteamID, "ko");
                bool hasKOAll = GetSettingBool("user_" + player.SteamID, "koall");
                bool hasArenaBuild = GetSettingBool("user_" + player.SteamID, "arenabuild");
                bool hasInstaMax = GetSettingBool("user_" + player.SteamID, "instamax");
                bool hasCopy = GetSettingInt("user_" + player.SteamID, "copy") != 0;
                bool hasDisappear = GetSettingInt("user_" + player.SteamID, "disappear") != 0;

                if (hasGod)
                    SendMessage(player, null, "[Reminder] God mode is active!");
                if (hasKO)
                    SendMessage(player, null, "[Reminder] KO mode is active!");
                if (hasKOAll)
                    SendMessage(player, null, "[Reminder] KO All mode is active!");
                if (hasArenaBuild)
                    SendMessage(player, null, "[Reminder] Arena build mode is active!");
                if (hasInstaMax)
                    SendMessage(player, null, "[Reminder] Insta Max mode is active!");
                if (hasCopy)
                    SendMessage(player, null, "[Reminder] Copy mode is active!");
                if (hasDisappear)
                    SendMessage(player, null, "[Reminder] Disappear mode is active!");
            }

            ini.Save();
        }

        public void DisappearTimer()
        {
            disappearShowing = !disappearShowing;

            foreach (var state in disappearList)
            {
                try
                {
                    if (disappearShowing)
                    {
                        BaseEntity baseEntity = state.Block.gameObject.ToBaseEntity();
                        if (baseEntity != null)
                        {
                            if (!baseEntity.isDestroyed)
                            {
                                baseEntity.SendMessage("PreDie", SendMessageOptions.DontRequireReceiver);
                                baseEntity.Kill(BaseNetworkable.DestroyMode.None);
                            }
                        }
                    }
                    else
                    {
                        BaseEntity ent = GameManager.server.CreateEntity(state.Prefab, state.Location, state.Rotation);
                        ent.SpawnAsMapEntity();

                        state.Block = (BuildingBlock)ent;
                        state.Block.grade = state.Grade;
                        try
                        {
                            state.Block.Heal(100000f);
                        }
                        catch (Exception ex) { }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("[DisappearTimer] " + ex.ToString());
                }
            }
        }

        private void CampingTimer()
        {
            for (var i = 0; i < Server.ActivePlayers.Count; i++)
            {
                if (GetSettingBool("user_" + Server.ActivePlayers[i].SteamID, "inArena"))
                {
                    var campingCounter = GetSettingInt("user_" + Server.ActivePlayers[i].SteamID, "campingCounter");
                    var campingX = GetSettingInt("user_" + Server.ActivePlayers[i].SteamID, "campingX");
                    var campingY = GetSettingInt("user_" + Server.ActivePlayers[i].SteamID, "campingY");
                    var campingZ = GetSettingInt("user_" + Server.ActivePlayers[i].SteamID, "campingZ");

                    var currentX = Convert.ToInt32(Server.ActivePlayers[i].Location.x);
                    var currentY = Convert.ToInt32(Server.ActivePlayers[i].Location.y);
                    var currentZ = Convert.ToInt32(Server.ActivePlayers[i].Location.z);

                    if (Math.Abs(campingX - currentX) <= 3 && Math.Abs(campingY - currentY) <= 3 && Math.Abs(campingZ - currentZ) <= 3)
                        campingCounter++;
                    else
                        campingCounter = 0;

                    if (campingCounter >= 10)
                    {
                        SendMessage(null, null, "[ARENA] " + Server.ActivePlayers[i].Name + " is being removed from the arena for camping/idling.");

                        SetSettingBool("user_" + Server.ActivePlayers[i].SteamID, "inArena", false);
                        SetSettingBool("user_" + Server.ActivePlayers[i].SteamID, "godArena", false);
                        Server.ActivePlayers[i].Kill();
                    }
                    else if (campingCounter >= 2 && (campingCounter % 2) == 0)
                    {
                        for (var ii = 0; ii < Server.ActivePlayers.Count; ii++)
                        {
                            SendMessage(Server.ActivePlayers[ii], null, "[ARENA] " + Server.ActivePlayers[i].Name + " is camping!" + (campingCounter == 8 ? " Last Warning!" : ""));
                        }
                    }

                    SetSetting("user_" + Server.ActivePlayers[i].SteamID, "campingCounter", campingCounter.ToString());
                    SetSetting("user_" + Server.ActivePlayers[i].SteamID, "campingX", currentX.ToString());
                    SetSetting("user_" + Server.ActivePlayers[i].SteamID, "campingY", currentY.ToString());
                    SetSetting("user_" + Server.ActivePlayers[i].SteamID, "campingZ", currentZ.ToString());
                }
            }
        }
    }
}
