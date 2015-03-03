using System;
using System.Linq;
using Pluton;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_PlayerConnected(Player player)
        {
            TimeRestrictReset(player, "attacked");

            SetSetting("user_" + player.SteamID, "SteamID", player.SteamID);
            SetSetting("user_" + player.SteamID, "LastJoined", player.SteamID);
            SetSetting("user_" + player.SteamID, "LastName", player.Name);

            if (GetSettingBool("user_" + player.SteamID, "inArena"))
            {
                SetSettingBool("user_" + player.SteamID, "inArena", false);
                player.Kill();
            }

            if (GetSettingBool("user_" + player.SteamID, "god"))
                SetSettingBool("user_" + player.SteamID, "god", false);

            SendMessage(null, null, player.Name + " " + GetText("Words_HasJoined"));

            var arenaCount = PlayersInArena().Count();
            string arenaS = arenaCount == 1 ? "" : GetText("Words_plural");
            string playerS = (Server.ActivePlayers.Count - 1) == 1 ? "" : GetText("Words_plural");


            SendMessage(player, null, String.Format(GetText("Words_Welcome"), server.hostname));
            SendMessage(player, null, String.Format(GetText("Words_Welcome2"), (Server.ActivePlayers.Count - 1), playerS, arenaCount, arenaS));

            var motd = GetSetting("Settings", "motd");
            if (motd.Length > 0)
                SendMessage(player, null, motd);

            if (GetText("Words_Welcome3").Length > 0)
                SendMessage(player, null, GetText("Words_Welcome3"));

            if (GetText("Words_Welcome4").Length > 0)
                SendMessage(player, null, GetText("Words_Welcome4"));

            if (GetSettingBool("user_" + player.SteamID, "inArena"))
                SetSettingBool("user_" + player.SteamID, "inArena", false);
        }
    }
}
