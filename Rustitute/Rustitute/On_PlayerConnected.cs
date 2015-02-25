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

            SendMessage(null, null, player.Name + " has joined!");

            var arenaCount = PlayersInArena().Count();
            string arenaS = arenaCount == 1 ? "" : "s";
            string playerS = (Server.ActivePlayers.Count - 1) == 1 ? "" : "s";

            SendMessage(player, null, "Welcome to " + server.hostname + "! This server is running Rustitute.");
            SendMessage(player, null, "There are currently " + (Server.ActivePlayers.Count - 1) + " other player" + playerS + " online and " + arenaCount + " player" + arenaS + " in the arena");

            var motd = GetSetting("Settings", "motd");
            if (motd.Length > 0)
                SendMessage(player, null, motd);

            SendMessage(player, null, "Type /starter to get a basic kit and /help for a full list of available commands.");
            SendMessage(player, null, "Type /arena to join the arena!.");

            if (GetSettingBool("user_" + player.SteamID, "inArena"))
                SetSettingBool("user_" + player.SteamID, "inArena", false);
        }
    }
}
