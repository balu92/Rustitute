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
        // :(
        //private static List<DeployedItem> lanternList = new List<DeployedItem>();
        private static List<GameObject> disappearBlocks = new List<GameObject>(); // used across reloads
        private static List<DisappearItem> disappearList = new List<DisappearItem>();
        private static List<string> disappearUnique = new List<string>();
        private bool disappearShowing = true;
        private bool SavingArena = false;
        private IniParser ini;
        private IniParser iniArena;

        private class Top10List
        {
            public string name { get; set; }
            public int count { get; set; }
        }

        private void Help(Player player)
        {
            SendMessage(player, null, "Available Commands:");

            SendMessage(player, null, "  /starter - Get a basic kit to get you started.");
            SendMessage(player, null, "  /tp <user> - Send a teleport request to that user.");
            SendMessage(player, null, "  /tpa - Accept a teleport request.");
            SendMessage(player, null, "  /tpw <user> - Whitelist a user so they can instantly teleport to you.");
            SendMessage(player, null, "  /tpsethome - Set your home position.");
            SendMessage(player, null, "  /tphome - Teleport home.");
            SendMessage(player, null, "  /players - Get a list of online players.");
            SendMessage(player, null, "  /arenaplayers - Get a list of players in the arena.");
            SendMessage(player, null, "  /location - Get your current XYZ position.");
            SendMessage(player, null, "  /nosleep - Toggle whether you are sleeping after spawning.");
            SendMessage(player, null, "  /time - Get the current time of day.");
            SendMessage(player, null, "  /stats <user> - View your stats or those of another player.");
            SendMessage(player, null, "  /achievements <user> - View your achievements or those of another player.");
            SendMessage(player, null, "  /arena - Join or leave the arena.");

            if (player.Owner)
            {
                SendMessage(player, null, "Admin Commands:");

                SendMessage(player, null, "  /aplayers - Get a list of online player with additional information.");
                SendMessage(player, null, "  /alocation <user> - Get the location of that player.");
                SendMessage(player, null, "  /tp <user> <toUser> - Teleport the first user to the second.");
                SendMessage(player, null, "  /tpto <x> <y> <z> - Teleport to the given location.");
                SendMessage(player, null, "  /tpto <user> <x> <y> <z> - Teleport that player to the given location.");
                SendMessage(player, null, "  /tpx - Teleport to the position at your crosshair.");
                SendMessage(player, null, "  /tparena - Teleport inside the arena.");
                SendMessage(player, null, "  /heal - Completely heal yourself.");
                SendMessage(player, null, "  /owner - List the owners of all sleeping bags within 50m.");
                SendMessage(player, null, "  /give <user> <item> <qty> - Give an item to a user. Qty defaults to 1.");
                //SendMessage(player, null, "  /airdrop <optionalUser> - Create an airdrop optionally above a given user.");
                //SendMessage(player, null, "  /animal <user> - Spawn an animal at the users position.");
                SendMessage(player, null, "  /jump <distanceUp> - Teleport by the given distance.");
                SendMessage(player, null, "  /adminkit - Kit yourself out with all the stuff you might need.");
                SendMessage(player, null, "  /adminmsg <message> - Send message to other logged in admins.");
                SendMessage(player, null, "  /botname <name> - Change the bot's name.");
                SendMessage(player, null, "  /ko - Toggle KO mode.");
                SendMessage(player, null, "  /koall - Toggle KO All mode. Make sure no other building part is within 10m of what you are destroying.");
                SendMessage(player, null, "  /god <optionalUser> - Toggle god mode.");
                SendMessage(player, null, "  /motd <optionalMotd> - Set or remove the motd.");
                SendMessage(player, null, "  /instamax - Toggle building parts being placed at maximum grade (stone, metal, etc).");
                SendMessage(player, null, "  /copy <distance> - Enable copy mode. No arg to disable. Shoot a block to copy it by this distance.");
                SendMessage(player, null, "  /arenabuild - Allow yourself to build at the arena.");
                SendMessage(player, null, "  /destroy <distance> - Destroy all objects within a radius of you.");
                SendMessage(player, null, "  /save - Save all Rustitute data");
                SendMessage(player, null, "  /load - Load all Rustitute data without saving first.");
                SendMessage(player, null, "  /kick <user> - Kick this user off the server.");
                SendMessage(player, null, "  /ban <user> - Ban the user from the server.");
                SendMessage(player, null, "  /kill <user> - Kill this user.");
                SendMessage(player, null, "  /mute <user> - Toggle mute for this user.");
                SendMessage(player, null, "  /silence <user> - Toggle silence mode for this user. This is similar to mute but the user isn't aware they are muted.");

                SendMessage(player, null, "  /logarena - Save the arena structure to file.");
                SendMessage(player, null, "  /destroyarena - Destroy the arena. Do this before saving the server data.");
                SendMessage(player, null, "  /spawnarena - Spawn the arena from the saved file.");
                SendMessage(player, null, "  /respawnarena - Destroys then Spawns the arena.");
                SendMessage(player, null, "  /togglearena - Toggle if the arena is enabled.");
            }
        }
    }
}
