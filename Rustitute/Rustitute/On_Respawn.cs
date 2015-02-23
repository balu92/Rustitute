using Pluton.Events;
using UnityEngine;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_Respawn(RespawnEvent re)
        {
            if (GetSettingBool("user_" + re.Player.SteamID, "god"))
            {
                Heal(re.Player);
                re.Player.basePlayer.InitializeHealth(float.MaxValue, float.MaxValue);
            }

            if (GetSettingBool("user_" + re.Player.SteamID, "nosleep"))
                re.WakeUp = true;

            if (GetSettingBool("user_" + re.Player.SteamID, "inArena"))
            {
                Vector3 arenaLocation = GetRandomArenaSpawn();

                re.ChangePos = true;
                re.SpawnPos = arenaLocation;
                re.GiveDefault = false;
                re.WakeUp = true;

                var loadout = Server.LoadOuts["arena"];
                loadout.ToInv(re.Player.Inventory);

                re.StartHealth = 100f;

                SendToArena(re.Player);
            }
        }
    }
}
