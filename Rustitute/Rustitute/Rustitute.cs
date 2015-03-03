using System.Collections.Generic;
using Pluton;
using UnityEngine;
using Timer = System.Timers.Timer;

namespace Rustitute
{
    public partial class Rustitute : CSharpPlugin
    {
        private string botName = "Rustitute";
        private string pluginIni = "Rustitute";
        private float arenaBuildRestrictionSpace = 300f;
        private Timer workTimer = new Timer();
        private Timer lanternTimer = new Timer();
        private Timer disappearTimer = new Timer();
        private Timer campingTimer = new Timer();
        private static List<Deployable> lanternList = new List<Deployable>();
        private static List<GameObject> disappearBlocks = new List<GameObject>(); // used across reloads
        private static List<DisappearItem> disappearList = new List<DisappearItem>();
        private static List<string> disappearUnique = new List<string>();
        private bool disappearShowing = true;
        private bool SavingArena = false;
        private IniParser ini;
        private IniParser iniArena;
        private IniParser iniLang;

        private class Top10List
        {
            public string name { get; set; }
            public int count { get; set; }
        }

        private void Help(Player player)
        {
            SendMessage(player, null, GetText("Help_AvailableCommands"));

            string[] userCommands =
            {
                "starter",
                "tp",
                "tpa",
                "tpw",
                "tpsethome",
                "tphome",
                "players",
                "arenaplayers",
                "location",
                "nosleep",
                "time",
                "stats",
                "achievements",
                "arena"
            };

            foreach (var cmd in userCommands)
                HelpMessage(player, cmd);

            if (player.Moderator)
            {
                SendMessage(player, null, GetText("Help_ModCommands"));

                string[] modCommands =
                {
                    "kick",
                    "mute"
                };

                foreach (var cmd in modCommands)
                    HelpMessage(player, cmd);
            }
            else if (player.Owner)
            {
                SendMessage(player, null, GetText("Help_AdminCommands"));

                string[] adminCommands =
                {
                    "aplayers",
                    "alocation",
                    "tp_admin",
                    "tpto_1",
                    "tpto_2",
                    "tpx",
                    "tparena",
                    "heal",
                    "owner",
                    "give",
                    "jump",
                    "adminkit",
                    "adminmsg",
                    "botname",
                    "ko",
                    "koall",
                    "god",
                    "motd",
                    "instamax",
                    "copy",
                    "arenabuild",
                    "destroy",
                    "save",
                    "load",
                    "kick",
                    "ban",
                    "kill",
                    "mute",
                    "silence",
                    "logarena",
                    "destroyarena",
                    "spawnarena",
                    "respawnarena",
                    "togglearena"
                };

                foreach (var cmd in adminCommands)
                    HelpMessage(player, cmd);
            }
        }
    }
}
