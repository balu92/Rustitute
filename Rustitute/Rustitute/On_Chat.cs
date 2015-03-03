using System;
using Pluton.Events;

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
                SendMessage(arg.User, null, GetText("Words_YouAreMuted"));
            }
        }
    }
}
