using Pluton;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_PlayerDisconnected(Player player)
        {
            string leftMessage = "";
            int lastDeath = GetSettingInt("user_" + player.SteamID, "lastDeath");

            if (Epoch() - lastDeath <= 15)
            {
                leftMessage = " " + GetText("Words_RageQuit");
            }

            SendMessage(null, null, player.Name + " " + GetText("Words_HasLeft") + leftMessage);
            if (GetSettingBool("user_" + player.SteamID, "inArena"))
            {
                player.Kill();
                SetSettingBool("user_" + player.SteamID, "inArena", false);
            }
        }
    }
}
