﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pluton;
using Pluton.Events;
using Steamworks;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_Chat(ChatEvent arg)
        {
            if (GetSettingBool("user_" + arg.User.SteamID, "silenced"))
            {
                SendMessage(arg.User, arg.User, arg.FinalText);
                arg.BroadcastName = "";
                arg.FinalText = "";
            }
            else if (GetSettingBool("user_" + arg.User.SteamID, "muted"))
            {
                arg.BroadcastName = "";
                arg.FinalText = "";
                SendMessage(arg.User, null, "You have been muted and cannot chat!");
            }
        }
    }
}
