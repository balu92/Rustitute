using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pluton;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_PlayerDisconnected(Player player)
        {
            SendMessage(null, null, player.Name + " has left!");
            if (GetSettingBool("user_" + player.SteamID, "inArena"))
            {
                // todo: test this
                player.Kill();
                SetSettingBool("user_" + player.SteamID, "inArena", false);
            }
        }
    }
}
