using Pluton.Events;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_DoorUse(DoorUseEvent due)
        {
            if (GetSettingBool("user_" + due.Player.SteamID, "inArena") || due.Player.Owner)
            {
                if (GetSettingBool("user_" + due.Player.SteamID, "inArena"))
                {
                    due.Deny("Teleporting you...");
                    //due.Player.Teleport(GetRandomArenaSpawn());

                    due.Player.basePlayer.transform.position = GetRandomArenaSpawn();
                    due.Player.basePlayer.ClientRPC(null, due.Player.basePlayer, "ForcePositionTo", new object[] {due.Player.basePlayer.transform.position});
                    due.Player.basePlayer.TransformChanged();

                    //due.Player.basePlayer.SetPlayerFlag(BasePlayer.PlayerFlags.ReceivingSnapshot, true);
                    //due.Player.basePlayer.UpdateNetworkGroup();
                    //due.Player.basePlayer.SendFullSnapshot();
                }
            }
        }
    }
}
