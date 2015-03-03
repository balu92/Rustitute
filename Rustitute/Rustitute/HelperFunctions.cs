using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Pluton;
using Pluton.Events;
using UnityEngine;

namespace Rustitute
{
    partial class Rustitute
    {
        private void LoadLang()
        {
            if (!HasCmd("help")) SetCmd("help", "help");
            if (!HasCmd("players")) SetCmd("players", "players");
            if (!HasCmd("arenaplayers")) SetCmd("arenaplayers", "arenaplayers");
            if (!HasCmd("aplayers")) SetCmd("aplayers", "aplayers");
            if (!HasCmd("location")) SetCmd("location", "location");
            if (!HasCmd("alocation")) SetCmd("alocation", "alocation");
            if (!HasCmd("tphome")) SetCmd("tphome", "tphome");
            if (!HasCmd("tpsethome")) SetCmd("tpsethome", "tpsethome");
            if (!HasCmd("tp")) SetCmd("tp", "tp");
            if (!HasCmd("tpa")) SetCmd("tpa", "tpa");
            if (!HasCmd("tpw")) SetCmd("tpw", "tpw");
            if (!HasCmd("tparena")) SetCmd("tparena", "tparena");
            if (!HasCmd("tpto")) SetCmd("tpto", "tpto");
            if (!HasCmd("tpx")) SetCmd("tpx", "tpx");
            if (!HasCmd("heal")) SetCmd("heal", "heal");
            if (!HasCmd("owner")) SetCmd("owner", "owner");
            if (!HasCmd("give")) SetCmd("give", "give");
            if (!HasCmd("jump")) SetCmd("jump", "jump");
            if (!HasCmd("starter")) SetCmd("starter", "starter");
            if (!HasCmd("adminkit")) SetCmd("adminkit", "adminkit");
            if (!HasCmd("time")) SetCmd("time", "time");
            if (!HasCmd("adminmsg")) SetCmd("adminmsg", "adminmsg");
            if (!HasCmd("botname")) SetCmd("botname", "botname");
            if (!HasCmd("disappear")) SetCmd("disappear", "disappear");
            if (!HasCmd("ko")) SetCmd("ko", "ko");
            if (!HasCmd("koall")) SetCmd("koall", "koall");
            if (!HasCmd("god")) SetCmd("god", "god");
            if (!HasCmd("arenabuild")) SetCmd("arenabuild", "arenabuild");
            if (!HasCmd("instamax")) SetCmd("instamax", "instamax");
            if (!HasCmd("nosleep")) SetCmd("nosleep", "nosleep");
            if (!HasCmd("motd")) SetCmd("motd", "motd");
            if (!HasCmd("copy")) SetCmd("copy", "copy");
            if (!HasCmd("destroy")) SetCmd("destroy", "destroy");
            if (!HasCmd("kick")) SetCmd("kick", "kick");
            if (!HasCmd("kill")) SetCmd("kill", "kill");
            if (!HasCmd("ban")) SetCmd("ban", "ban");
            if (!HasCmd("mute")) SetCmd("mute", "mute");
            if (!HasCmd("silence")) SetCmd("silence", "silence");
            if (!HasCmd("save")) SetCmd("save", "save");
            if (!HasCmd("load")) SetCmd("load", "load");
            if (!HasCmd("togglearena")) SetCmd("togglearena", "togglearena");
            if (!HasCmd("logarena")) SetCmd("logarena", "logarena");
            if (!HasCmd("respawnarena")) SetCmd("respawnarena", "respawnarena");
            if (!HasCmd("spawnarena")) SetCmd("spawnarena", "spawnarena");
            if (!HasCmd("destroyarena")) SetCmd("destroyarena", "destroyarena");
            if (!HasCmd("addspawn")) SetCmd("addspawn", "addspawn");
            if (!HasCmd("arenahere")) SetCmd("arenahere", "arenahere");
            if (!HasCmd("arena")) SetCmd("arena", "arena");
            if (!HasCmd("achievements")) SetCmd("achievements", "achievements");
            if (!HasCmd("stats")) SetCmd("stats", "stats");
            if (!HasCmd("fx")) SetCmd("fx", "fx");

            if (!HasText("Help_AvailableCommands")) SetText("Help_AvailableCommands", "Available Commands:");
            if (!HasText("Help_ModCommands")) SetText("Help_ModCommands", "Mod Commands:");
            if (!HasText("Help_AdminCommands")) SetText("Help_AdminCommands", "Admin Commands:");
            if (!HasText("Help_starter")) SetText("Help_starter", "- Get a basic kit to get you started.");
            if (!HasText("Help_tp")) SetText("Help_tp", "<user> - Send a teleport request to that user.");
            if (!HasText("Help_tpa")) SetText("Help_tpa", "- Accept a teleport request.");
            if (!HasText("Help_tpw")) SetText("Help_tpw", "<user> - Whitelist a user so they can instantly teleport to you.");
            if (!HasText("Help_tpsethome")) SetText("Help_tpsethome", "- Set your home position.");
            if (!HasText("Help_tphome")) SetText("Help_tphome", "- Teleport home.");
            if (!HasText("Help_players")) SetText("Help_players", "- Get a list of online players.");
            if (!HasText("Help_arenaplayers")) SetText("Help_arenaplayers", "- Get a list of players in the arena.");
            if (!HasText("Help_location")) SetText("Help_location", "- Get your current XYZ position.");
            if (!HasText("Help_nosleep")) SetText("Help_nosleep", "- Toggle whether you are sleeping after spawning.");
            if (!HasText("Help_time")) SetText("Help_time", "- Get the current time of day.");
            if (!HasText("Help_stats")) SetText("Help_stats", "<user> - View your stats or those of another player.");
            if (!HasText("Help_achievements")) SetText("Help_achievements", "- View your achievements or those of another player.");
            if (!HasText("Help_arena")) SetText("Help_arena", "- Join or leave the arena.");
            if (!HasText("Help_aplayers")) SetText("Help_aplayers", "- Get a list of online player with additional information.");
            if (!HasText("Help_alocation")) SetText("Help_alocation", "<user> - Get the location of that player.");
            if (!HasText("Help_tp_admin")) SetText("Help_tp_admin", "<user> <toUser> - Teleport the first user to the second.");
            if (!HasText("Help_tpto_1")) SetText("Help_tpto_1", "<x> <y> <z> - Teleport to the given location.");
            if (!HasText("Help_tpto_2")) SetText("Help_tpto_2", "<user> <x> <y> <z> - Teleport that player to the given location.");
            if (!HasText("Help_tpx")) SetText("Help_tpx", "- Teleport to the position at your crosshair.");
            if (!HasText("Help_tparena")) SetText("Help_tparena", "- Teleport inside the arena.");
            if (!HasText("Help_heal")) SetText("Help_heal", "- Completely heal yourself.");
            if (!HasText("Help_owner")) SetText("Help_owner", "- List the known user items within 50m.");
            if (!HasText("Help_give")) SetText("Help_give", "<user> <item> <qty> - Give an item to a user. Qty defaults to 1.");
            if (!HasText("Help_jump")) SetText("Help_jump", "<distance> - Teleport by the given distance.");
            if (!HasText("Help_adminkit")) SetText("Help_adminkit", "- Kit yourself out with all the stuff you might need.");
            if (!HasText("Help_adminmsg")) SetText("Help_adminmsg", "<message> - Send message to other logged in admins.");
            if (!HasText("Help_botname")) SetText("Help_botname", "<name> - Change the bot's name.");
            if (!HasText("Help_ko")) SetText("Help_ko", "- Toggle KO mode.");
            if (!HasText("Help_koall")) SetText("Help_koall", "- Toggle KO All mode. Make sure no other building part is within 10m of what you are destroying.");
            if (!HasText("Help_god")) SetText("Help_god", "<optionalUser> - Toggle god mode.");
            if (!HasText("Help_motd")) SetText("Help_motd", "<optionalMessage> - Set or remove the motd.");
            if (!HasText("Help_instamax")) SetText("Help_instamax", "- Toggle building parts being placed at maximum grade (stone, metal, etc).");
            if (!HasText("Help_copy")) SetText("Help_copy", "<distance> - Enable copy mode. No arg to disable. Shoot a block to copy it by this distance.");
            if (!HasText("Help_arenabuild")) SetText("Help_arenabuild", "- Allow yourself to build at the arena.");
            if (!HasText("Help_destroy")) SetText("Help_destroy", "<distance> - Destroy all objects within a radius of you.");
            if (!HasText("Help_save")) SetText("Help_save", "- Save all Rustitute data.");
            if (!HasText("Help_load")) SetText("Help_load", "- Load all Rustitute data without saving first.");
            if (!HasText("Help_kick")) SetText("Help_kick", "<user> - Kick this user off the server.");
            if (!HasText("Help_ban")) SetText("Help_ban", "<user> - <user> - Ban the user from the server.");
            if (!HasText("Help_kill")) SetText("Help_kill", "<user> - Kill this user.");
            if (!HasText("Help_mute")) SetText("Help_mute", "<user> - Toggle mute for this user.");
            if (!HasText("Help_silence")) SetText("Help_silence", "<user> - Toggle silence mode for this user. This is similar to mute but the user isn't aware they are muted.");
            if (!HasText("Help_logarena")) SetText("Help_logarena", "- Save the arena structure to file.");
            if (!HasText("Help_destroyarena")) SetText("Help_destroyarena", "- Destroy the arena. Do this before saving the server data.");
            if (!HasText("Help_spawnarena")) SetText("Help_spawnarena", "- Spawn the arena from the saved file.");
            if (!HasText("Help_respawnarena")) SetText("Help_respawnarena", "- Destroys then Spawns the arena.");
            if (!HasText("Help_togglearena")) SetText("Help_togglearena", "- Toggle if the arena is enabled.");

            if (!HasText("Achievement")) SetText("Achievement", "Achievement");
            if (!HasText("Achievement_Unlocked")) SetText("Achievement_Unlocked", "[Achievement Unlocked] Congrats {0}: {1}");
            if (!HasText("Achievement_5HealthKill")) SetText("Achievement_5HealthKill", "Get a kill while you have 5 or less health");
            if (!HasText("Achievement_Kills10")) SetText("Achievement_Kills10", "Get 10 Kills");
            if (!HasText("Achievement_Kills100")) SetText("Achievement_Kills100", "Get 100 Kills");
            if (!HasText("Achievement_Kills1000")) SetText("Achievement_Kills1000", "Get 1,000 Kills");
            if (!HasText("Achievement_Kills10000")) SetText("Achievement_Kills10000", "Get 10,000 Kills");
            if (!HasText("Achievement_Headshots10")) SetText("Achievement_Headshots10", "Get 10 Headshots");
            if (!HasText("Achievement_Headshots100")) SetText("Achievement_Headshots100", "Get 100 Headshots");
            if (!HasText("Achievement_Headshots1000")) SetText("Achievement_Headshots1000", "Get 1,000 Headshots");
            if (!HasText("Achievement_Headshots10000")) SetText("Achievement_Headshots10000", "Get 10,000 Headshots");
            if (!HasText("Achievement_Deaths10")) SetText("Achievement_Deaths10", "Died 10 Times");
            if (!HasText("Achievement_Deaths100")) SetText("Achievement_Deaths100", "Died 100 Times");
            if (!HasText("Achievement_Deaths1000")) SetText("Achievement_Deaths1000", "Died 1,000 Times");
            if (!HasText("Achievement_Deaths10000")) SetText("Achievement_Deaths10000", "Died 10,000 Times");
            if (!HasText("Achievement_SleeperKills10")) SetText("Achievement_SleeperKills10", "Killed 10 sleepers");
            if (!HasText("Achievement_SleeperKills100")) SetText("Achievement_SleeperKills100", "Killed 100 sleepers");
            if (!HasText("Achievement_SleeperKills1000")) SetText("Achievement_SleeperKills1000", "Killed 1,000 sleepers");
            if (!HasText("Achievement_SleeperKills10000")) SetText("Achievement_SleeperKills10000", "Killed 10,000 sleepers");
            if (!HasText("Achievement_HitRunning50")) SetText("Achievement_HitRunning50", "Hit someone over 50m away while they are running");
            if (!HasText("Achievement_HitRunning100")) SetText("Achievement_HitRunning100", "Hit someone over 100m away while they are running");
            if (!HasText("Achievement_HitRunning200")) SetText("Achievement_HitRunning200", "Hit someone over 200m away while they are running");
            if (!HasText("Achievement_HitAir50")) SetText("Achievement_HitAir50", "Hit someone over 50m away while they are in the air");
            if (!HasText("Achievement_HitAir100")) SetText("Achievement_HitAir100", "Hit someone over 100m away while they are in the air");
            if (!HasText("Achievement_HitAir200")) SetText("Achievement_HitAir200", "Hit someone over 200m away while they are in the air");
            if (!HasText("Achievement_Hit100")) SetText("Achievement_Hit100", "Hit someone over 100m away");
            if (!HasText("Achievement_Hit200")) SetText("Achievement_Hit200", "Hit someone over 200m away");
            if (!HasText("Achievement_Hit300")) SetText("Achievement_Hit300", "Hit someone over 300m away");
            if (!HasText("Achievement_Hit500")) SetText("Achievement_Hit500", "Hit someone over 500m away");
            if (!HasText("Achievement_KillDistance50")) SetText("Achievement_KillDistance50", "Killed someone over 50m away");
            if (!HasText("Achievement_KillDistance100")) SetText("Achievement_KillDistance100", "Killed someone over 100m away");
            if (!HasText("Achievement_KillDistance200")) SetText("Achievement_KillDistance200", "Killed someone over 200m away");
            if (!HasText("Achievement_KillDistance300")) SetText("Achievement_KillDistance300", "Killed someone over 300m away");
            if (!HasText("Achievement_KillDistance400")) SetText("Achievement_KillDistance400", "Killed someone over 400m away");
            if (!HasText("Achievement_KillDistance500")) SetText("Achievement_KillDistance500", "Killed someone over 500m away");
            if (!HasText("Achievement_JoinedArena")) SetText("Achievement_JoinedArena", "Joined the Arena");

            if (!HasText("Words_north")) SetText("Words_north", "north");
            if (!HasText("Words_northeast")) SetText("Words_northeast", "north east");
            if (!HasText("Words_east")) SetText("Words_east", "east");
            if (!HasText("Words_southeast")) SetText("Words_southeast", "south east");
            if (!HasText("Words_south")) SetText("Words_south", "south");
            if (!HasText("Words_southwest")) SetText("Words_southwest", "south west");
            if (!HasText("Words_west")) SetText("Words_west", "west");
            if (!HasText("Words_northwest")) SetText("Words_northwest", "north west");
            if (!HasText("Words_north")) SetText("Words_north", "north");
            if (!HasText("Words_Tryagainin")) SetText("Words_Tryagainin", "Try again in");
            if (!HasText("Words_seconds")) SetText("Words_seconds", "seconds");
            if (!HasText("Words_minutes")) SetText("Words_minutes", "minutes");
            if (!HasText("Words_TeleportingYou")) SetText("Words_TeleportingYou", "Teleporting you...");
            if (!HasText("Words_YouAreMuted")) SetText("Words_YouAreMuted", "You have been muted and cannot chat!");
            if (!HasText("Words_RageQuit")) SetText("Words_RageQuit", "RAGE QUIT!");
            if (!HasText("Words_HasLeft")) SetText("Words_HasLeft", "has left!");
            if (!HasText("Words_HasJoined")) SetText("Words_HasJoined", "has joined!");
            if (!HasText("Words_WhileSleeping")) SetText("Words_WhileSleeping", "while they were sleeping");
            if (!HasText("Words_BledToDeath")) SetText("Words_BledToDeath", "bled to death");
            if (!HasText("Words_Headshot")) SetText("Words_Headshot", "- HEADSHOT!");
            if (!HasText("Words_CockShot")) SetText("Words_CockShot", "- COCK SHOT!");
            if (!HasText("Words_killed")) SetText("Words_killed", "killed");
            if (!HasText("Words_meters")) SetText("Words_meters", "m");
            if (!HasText("Words_DeathBearTrap")) SetText("Words_DeathBearTrap", "was killed by a bear trap");
            if (!HasText("Words_died")) SetText("Words_died", "died");
            if (!HasText("Words_DeathBurnt")) SetText("Words_DeathBurnt", "burnt to death");
            if (!HasText("Words_DeathSuicide")) SetText("Words_DeathSuicide", "commited suicide");
            if (!HasText("Words_DeathFell")) SetText("Words_DeathFell", "fell and died");
            if (!HasText("Words_DeathDrowned")) SetText("Words_DeathDrowned", "drowned");
            if (!HasText("Words_plural")) SetText("Words_plural", "s");
            if (!HasText("Words_Welcome")) SetText("Words_Welcome", "Welcome to {0}! This server is running Rustitute");
            if (!HasText("Words_Welcome2")) SetText("Words_Welcome2", "There are currently {0} other player{1} online and {2} player{3} in the arena");
            if (!HasText("Words_Welcome3")) SetText("Words_Welcome3", "Type /starter to get a basic kit and /help for a full list of available commands.");
            if (!HasText("Words_Welcome4")) SetText("Words_Welcome4", "Type /arena to join the arena!");
            if (!HasText("Words_CantHurtPlayerHalfArena")) SetText("Words_CantHurtPlayerHalfArena", "You cannot hurt this player! Only one of you is in arena mode.");
            if (!HasText("Words_GodHurtThem")) SetText("Words_GodHurtThem", "This person has god mode and cannot be damaged!");
            if (!HasText("Words_GodHurtYou")) SetText("Words_GodHurtYou", "You have god mode and were just attacked by {0}");
            if (!HasText("Words_Reminder")) SetText("Words_Reminder", "Reminder");
            if (!HasText("Words_ReminderGod")) SetText("Words_ReminderGod", "God mode is active!");
            if (!HasText("Words_ReminderKo")) SetText("Words_ReminderKo", "KO mode is active!");
            if (!HasText("Words_ReminderKoAll")) SetText("Words_ReminderKoAll", "KO All mode is active!");
            if (!HasText("Words_ReminderArenaBuild")) SetText("Words_ReminderArenaBuild", "Arena build mode is active!");
            if (!HasText("Words_ReminderInstaMax")) SetText("Words_ReminderInstaMax", "Insta Max mode is active!");
            if (!HasText("Words_ReminderCopy")) SetText("Words_ReminderCopy", "Copy mode is active!");
            if (!HasText("Words_ReminderDisappear")) SetText("Words_ReminderDisappear", "Disappear mode is active!");
            if (!HasText("Words_GodDisabled")) SetText("Words_GodDisabled", "God mode disabled!");
            if (!HasText("Words_GodEnabled")) SetText("Words_GodEnabled", "God mode enabled!");
            if (!HasText("Words_players")) SetText("Words_players", "There are {0} players online and {1} sleeping players.");
            if (!HasText("Words_playersOnline")) SetText("Words_playersOnline", "Online players:");
            if (!HasText("Words_arenaPlayers")) SetText("Words_arenaPlayers", "There are {0} player{1} in the arena.");
            if (!HasText("Words_arenaPlayersOnline")) SetText("Words_arenaPlayersOnline", "Arena players:");
            if (!HasText("Words_location")) SetText("Words_location", "Location: X={0} Y={1} Z={2} facing {3}");
            if (!HasText("Words_PlayerNotFound")) SetText("Words_PlayerNotFound", "Player not found!");
            if (!HasText("Words_TPUnderAttack")) SetText("Words_TPUnderAttack", "You cannot teleport while under attack!");
            if (!HasText("Words_TPArena")) SetText("Words_TPArena", "You cannot teleport while in the arena!");
            if (!HasText("Words_TPNoHome")) SetText("Words_TPNoHome", "Your have no home position to teleport to!");
            if (!HasText("Words_TPHome")) SetText("Words_TPHome", "You have been teleported home!");
            if (!HasText("Words_TPTooCloseToArena")) SetText("Words_TPTooCloseToArena", "You cannot set your home position this close to the arena!");
            if (!HasText("Words_TPHomeSet")) SetText("Words_TPHomeSet", "Your home position has been set!");
            if (!HasText("Words_FirstNotFound")) SetText("Words_FirstNotFound", "First player not found!");
            if (!HasText("Words_SecondNotFound")) SetText("Words_SecondNotFound", "Second player not found!");
            if (!HasText("Words_TPUserInArena")) SetText("Words_TPUserInArena", "{0} is in the arena and cannot be teleported!");
            if (!HasText("Words_TPUserInArena2")) SetText("Words_TPUserInArena2", "{0} is in the arena and cannot be teleport to!");
            if (!HasText("Words_TPUserToUser")) SetText("Words_TPUserToUser", "{0} has been teleported to {1}");
            if (!HasText("Words_TPNoSelf")) SetText("Words_TPNoSelf", "You cannot teleport to yourself!");
            if (!HasText("Words_TPWaitTime")) SetText("Words_TPWaitTime", "You must wait 3 minutes between teleports!");
            if (!HasText("Words_TPToUser")) SetText("Words_TPToUser", "You have been teleported to {0}");
            if (!HasText("Words_TPPending")) SetText("Words_TPPending", "That player already has a pending teleport request. Try again in 30 seconds.");
            if (!HasText("Words_TPRequestSent")) SetText("Words_TPRequestSent", "Teleport request sent to {0}");
            if (!HasText("Words_TPRequestFrom")) SetText("Words_TPRequestFrom", "Teleport request from {0}. Type /tpa to accept the request within 30 seconds.");
            if (!HasText("Words_TPNotOnline")) SetText("Words_TPNotOnline", "The user requesting the teleport is not online.");
            if (!HasText("Words_TPdTo")) SetText("Words_TPdTo", "You have been teleported to {0}");
            if (!HasText("Words_TPNoRequest")) SetText("Words_TPNoRequest", "You have no teleport request.");
            if (!HasText("Words_TPWRemoved")) SetText("Words_TPWRemoved", "{0} removed from your teleport whitelist!");
            if (!HasText("Words_TPWAdded")) SetText("Words_TPWAdded", "{0} added to your teleport whitelist!");
            if (!HasText("Words_TPToArena")) SetText("Words_TPToArena", "You have been teleported to the arena!");
            if (!HasText("Words_TPUserTo")) SetText("Words_TPUserTo", "{0} has been teleported!");
            if (!HasText("Words_TPd")) SetText("Words_TPd", "You have been teleported!");
            if (!HasText("Words_TPXFail")) SetText("Words_TPXFail", "Crosshair position too far or invalid, aim a little closer!");
            if (!HasText("Words_Healed")) SetText("Words_Healed", "You have been healed!");
            if (!HasText("Words_OwnerSleepingBag")) SetText("Words_OwnerSleepingBag", "Sleeping Bag {0}{1) away: {2}");
            if (!HasText("Words_OwnerToolCupboard")) SetText("Words_OwnerToolCupboard", "Tool Cupboard {0}{1} away: {2}");
            if (!HasText("Words_GiveNotFound")) SetText("Words_GiveNotFound", "That item could not be found");
            if (!HasText("Arena_CantBuildNear")) SetText("Arena_CantBuildNear", "You cannot build near the arena!");
            if (!HasText("Arena_ARENA")) SetText("Arena_ARENA", "ARENA");
            if (!HasText("Arena_CantHarmThey")) SetText("Arena_CantHarmThey", "You cannot harm players for 10 seconds after they spawn!");
            if (!HasText("Arena_CantHarmYou")) SetText("Arena_CantHarmYou", "You cannot harm players for 10 seconds after you spawn!");
            if (!HasText("Arena_CantDamageArena")) SetText("Arena_CantDamageArena", "You cannot damage the arena!");
            if (!HasText("Arena_CampingRemoved")) SetText("Arena_CampingRemoved", "{0} is being removed from the arena for camping/idling.");
            if (!HasText("Arena_CampingWarning")) SetText("Arena_CampingWarning", "{0} is camping!");
            if (!HasText("Arena_CampingLastWarning")) SetText("Arena_CampingLastWarning", "Last Warning!");
            if (!HasText("Arena_NotEnabled")) SetText("Arena_NotEnabled", "The arena is not enabled.");
            if (!HasText("Arena_NotSetup")) SetText("Arena_NotSetup", "The arena on this server has not been setup yet.");
            if (!HasText("Arena_UserLeft")) SetText("Arena_UserLeft", "{0} has left the arena!");
            if (!HasText("Arena_UserJoined")) SetText("Arena_UserJoined", "{0} has joined the arena!");
            if (!HasText("Arena_JoinMessage")) SetText("Arena_JoinMessage", "The arena has no bleeding and no fall damage. Use doors to teleport. Enjoy!");
            if (!HasText("Arena_LoggingArena")) SetText("Arena_LoggingArena", "Logging arena... The server will be lagged out for a few seconds!");
            if (!HasText("Arena_SavingToFile")) SetText("Arena_SavingToFile", "Saving to file...");
            if (!HasText("Arena_LoggedArena")) SetText("Arena_LoggedArena", "Arena Logged!");
            if (!HasText("Arena_SpawnedArena")) SetText("Arena_SpawnedArena", "Arena Spawned!");
            if (!HasText("Arena_DestroyedArena")) SetText("Arena_DestroyedArena", "Arena Destroyed!");
            if (!HasText("Arena_AddSpawn")) SetText("Arena_AddSpawn", "Arena spawn position added!");
            if (!HasText("Arena_SetCenter")) SetText("Arena_SetCenter", "Arena center position set!");
            if (!HasText("Arena_InvincibleFor")) SetText("Arena_InvincibleFor", "Invincible for {0} more second{1}");
            if (!HasText("Arena_NoLongerInvincible")) SetText("Arena_NoLongerInvincible", "No longer invincible!");
            if (!HasText("Words_GiveTo")) SetText("Words_GiveTo", "{0}x {1} has been given to {2}");
            if (!HasText("Words_GiveToYou")) SetText("Words_GiveToYou", "{0}x {1} has been given to you!");
            if (!HasText("Words_StarterKitWaitTime")) SetText("Words_StarterKitWaitTime", "You must wait 30 minutes between starter kits!");
            if (!HasText("Words_StarterKitGiven")) SetText("Words_StarterKitGiven", "A starter kit has been given to you!");
            if (!HasText("Words_AdminKitGiven")) SetText("Words_AdminKitGiven", "An admin kit has been given to you!");
            if (!HasText("Words_TimeSet")) SetText("Words_TimeSet", "Time set!");
            if (!HasText("Words_TimeCurrentlyAround")) SetText("Words_TimeCurrentlyAround", "It is currently around");
            if (!HasText("Words_TimeMorning")) SetText("Words_TimeMorning", "in the morning");
            if (!HasText("Words_TimeNight")) SetText("Words_TimeNight", "at night");
            if (!HasText("Words_TimeNoon")) SetText("Words_TimeNoon", "noon");
            if (!HasText("Words_TimeAfternoon")) SetText("Words_TimeAfternoon", "in the afternoon");
            if (!HasText("Words_TimeEvening")) SetText("Words_TimeEvening", "in the evening");
            if (!HasText("Words_BotnameSet")) SetText("Words_BotnameSet", "Bot name set to");
            if (!HasText("Words_DisappearEnabled")) SetText("Words_DisappearEnabled", "Disappear mode enabled!");
            if (!HasText("Words_DisappearDisabled")) SetText("Words_DisappearDisabled", "Disappear mode disabled!");
            if (!HasText("Words_KOEnabled")) SetText("Words_KOEnabled", "KO mode enabled!");
            if (!HasText("Words_KODisabled")) SetText("Words_KODisabled", "KO mode disabled!");
            if (!HasText("Words_KOAllEnabled")) SetText("Words_KOAllEnabled", "KO All mode enabled!");
            if (!HasText("Words_KOAllDisabled")) SetText("Words_KOAllDisabled", "KO All mode disabled!");
            if (!HasText("Words_YourGodDisabled")) SetText("Words_YourGodDisabled", "Your god mode has been disabled!");
            if (!HasText("Words_GodDisabledFor")) SetText("Words_GodDisabledFor", "God mode disabled for {0}");
            if (!HasText("Words_GodDisabledFor")) SetText("Words_GodEnabledFor", "God mode enabled for {0}");
            if (!HasText("Words_ArenaBuildDisabled")) SetText("Words_ArenaBuildDisabled", "You can no longer build at the arena!");
            if (!HasText("Words_ArenaBuildEnabled")) SetText("Words_ArenaBuildEnabled", "You can now build at the arena!");
            if (!HasText("Words_InstaMaxDisabled")) SetText("Words_InstaMaxDisabled", "Insta Max disabled!");
            if (!HasText("Words_InstaMaxEnabled")) SetText("Words_InstaMaxEnabled", "Insta Max enabled!");
            if (!HasText("Words_NoSleepDisabled")) SetText("Words_NoSleepDisabled", "You will now be sleeping after spawning");
            if (!HasText("Words_NoSleepEnabled")) SetText("Words_NoSleepEnabled", "You will now be awake after spawning");
            if (!HasText("Words_MotdSet")) SetText("Words_MotdSet", "Motd set!");
            if (!HasText("Words_CopyEnabled")) SetText("Words_CopyEnabled", "Copy mode set to");
            if (!HasText("Words_CopyDisabled")) SetText("Words_CopyDisabled", "Copy mode disabled!");
            if (!HasText("Words_Muted")) SetText("Words_Muted", "{0} has been muted");
            if (!HasText("Words_Unmuted")) SetText("Words_Unmuted", "{0} has been unmuted");
            if (!HasText("Words_Silenced")) SetText("Words_Silenced", "{0} has been silenced");
            if (!HasText("Words_Unsilenced")) SetText("Words_Unsilenced", "{0} has been unsilenced");
            if (!HasText("Words_Saved")) SetText("Words_Saved", "Rustitute data saved!");
            if (!HasText("Words_Loaded")) SetText("Words_Loaded", "Rustitute data loaded!");
            if (!HasText("Words_ArenaDisabled")) SetText("Words_ArenaDisabled", "The arena has been disabled");
            if (!HasText("Words_ArenaEnabled")) SetText("Words_ArenaEnabled", "The arena has been enabled");
            if (!HasText("Words_AchievementsFor")) SetText("Words_AchievementsFor", "Achievements for {0}");
            if (!HasText("Words_StatsFor")) SetText("Words_StatsFor", "Stats for {0}");
            if (!HasText("Words_StatsKills")) SetText("Words_StatsKills", "Kills");
            if (!HasText("Words_ArenaKills")) SetText("Words_ArenaKills", "Arena Kills");
            if (!HasText("Words_Headshots")) SetText("Words_Headshots", "Headshots");
            if (!HasText("Words_Deaths")) SetText("Words_Deaths", "Deaths");
            if (!HasText("Words_ArenaDeaths")) SetText("Words_ArenaDeaths", "Arena Deaths");

            iniLang.Save();
        }

        private void HelpMessage(Player player, string command)
        {
            SendMessage(player, null, "  /" + GetCmd(command) + " " + GetText("Help_" + command));
        }

        private bool TimeRestrict(Player player, string action, int seconds, string message)
        {
            int lastTime = Epoch() - Convert.ToInt32(GetSettingInt("user_" + player.SteamID, "lastTime_" + action));
            if (lastTime <= seconds)
            {
                string nextAvailableWord = " " + GetText("Words_seconds");
                int nextAvailable = seconds - lastTime;
                if (nextAvailable >= 120)
                {
                    nextAvailable = Convert.ToInt32(Mathf.Ceil(nextAvailable / 60));
                    nextAvailableWord = " " + GetText("Words_minutes");
                }

                SendMessage(player, null, message + " " + GetText("Words_Tryagainin") + " " + nextAvailable + nextAvailableWord);
                return true;
            }
            return false;
        }

        private void TimeRestrictReset(Player player, string action)
        {
            SetSetting("user_" + player.SteamID, "lastTime_" + action, "0");
        }

        private void TimeRestrictSet(Player player, string action)
        {
            SetSetting("user_" + player.SteamID, "lastTime_" + action, Epoch().ToString());
        }

        private void SendMessage(Player to, Player from, string message)
        {
            string fromName = botName;
            if (from != null)
                fromName = from.Name;

            if (to == null)
            {
                if(from == null)
                    Server.BroadcastFrom(botName, message);
                else
                    Server.BroadcastFrom(from.Name, message);

                //Debug.Log("[CHAT] " + fromName + ": " + message);
            }
            else
            {
                if(from == null)
                    to.MessageFrom(botName, message);
                else
                    to.MessageFrom(from.Name, message);
            }
        }

        private int Epoch()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return (int)t.TotalSeconds;
        }

        private void Heal(Player player)
        {
            player.basePlayer.metabolism.Reset();
            player.basePlayer.metabolism.calories.Add(10000f);
            player.basePlayer.metabolism.hydration.Add(10000f);

            player.basePlayer.Heal(10000f);
        }

        private void InstaMax(BuildingPart part)
        {
            BuildingPartTimer state = new BuildingPartTimer();
            state.part = part;
            state.Timer = null;

            state.Timer = new System.Threading.Timer(InstaMaxTimer, state, 50, Timeout.Infinite);
        }

        private void InstaMaxTimer(object stateInfo)
        {
            BuildingPartTimer state = (BuildingPartTimer)stateInfo;
            state.Timer.Dispose();

            try
            {
                MaxGrade(state.part.buildingBlock);
                state.part.buildingBlock.SetHealthToMax();
            }
            catch (Exception ex) { }
        }

        private void MaxGrade(BuildingBlock block)
        {
            int grade = ((int)block.blockDefinition.grades.Length) - 1;

            if (grade == block.grade)
                return;

            block.SetGrade(grade);
        }

        private void SetGrade(BuildingBlock block, int grade)
        {
            try
            {
                block.SetGrade(grade);
            }
            catch (Exception ex) { }

            block.SendNetworkUpdate(BasePlayer.NetworkQueue.Update);
        }

        private void DestroyEverything(Vector3 location, float radius)
        {
            Collider[] castHits = Physics.OverlapSphere(location, radius);

            if (castHits.Count() >= 128)
            {
                SendMessageToAdmins("WARNING: Hits >= 128! Some items may not have destroyed at position " + location);
            }

            foreach (Collider collider in castHits)
            {
                try
                {
                    if ((collider.gameObject.GetComponentInParent<BuildingBlock>()) ||
                        (collider.gameObject.GetComponentInParent<Deployable>()) ||
                        (collider.gameObject.GetComponentInParent<StorageContainer>()) ||
                        (collider.gameObject.GetComponentInParent<WorldItem>()) ||
                        (collider.gameObject.GetComponentInParent<WorldItem_EnableDisable>()) ||
                        (collider.name.StartsWith("items/"))
                        )
                    {
                        BaseEntity baseEntity = collider.gameObject.ToBaseEntity();
                        if (baseEntity != null)
                        {
                            if (!baseEntity.isDestroyed)
                            {
                                baseEntity.SendMessage("PreDie", SendMessageOptions.DontRequireReceiver);
                                baseEntity.Kill(BaseNetworkable.DestroyMode.None);
                            }
                        }
                    }
                }
                catch (Exception ex) { }
            }
        }

        private void KOAll(Vector3 location)
        {
            Vector3 origin = new Vector3(location.x, location.y, location.z);
            Collider[] castHits = Physics.OverlapSphere(origin, 8f);

            foreach (Collider collider in castHits)
            {
                try
                {
                    if ((collider.gameObject.GetComponentInParent<BuildingBlock>()) ||
                        (collider.gameObject.GetComponentInParent<Deployable>()) ||
                        (collider.gameObject.GetComponentInParent<StorageContainer>()) ||
                        (collider.gameObject.GetComponentInParent<WorldItem>()) ||
                        (collider.gameObject.GetComponentInParent<WorldItem_EnableDisable>()) ||
                        (collider.name.StartsWith("items/"))
                        )
                    {
                        BaseEntity baseEntity = collider.gameObject.ToBaseEntity();
                        if (baseEntity != null)
                        {
                            if (!baseEntity.isDestroyed)
                            {
                                var nextLocation = collider.transform.position;
                                baseEntity.SendMessage("PreDie", SendMessageOptions.DontRequireReceiver);
                                baseEntity.Kill(BaseNetworkable.DestroyMode.None);

                                KOAll(nextLocation);
                            }
                        }
                    }

                }
                catch (Exception ex) { }
            }

        }

        private static String md5(String TextToHash)
        {
            if (string.IsNullOrEmpty(TextToHash))
                return String.Empty;

            MD5 Smd5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(TextToHash);
            byte[] result = Smd5.ComputeHash(textToHash);

            return System.BitConverter.ToString(result).Replace("-", string.Empty).ToLower();
        }

        private void SendMessageToAdmins(string msg)
        {
            for (var i = 0; i < Server.ActivePlayers.Count; i++)
            {
                if (Server.ActivePlayers[i].Admin)
                    SendMessage(Server.ActivePlayers[i], null, msg);
            }
        }

        private Player GetPlayerFromPotentialPartialName(String name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return null;

                Player player = Server.ActivePlayers.Find(x => x.Name.ToLower().Contains(name.ToLower()));
                if (String.Equals(player.Name, name, StringComparison.CurrentCultureIgnoreCase))
                    return player;

                return Server.FindPlayer(name);
            }
            catch (Exception ex)
            {
                //SendMessageToAdmins("[Exception] GetPlayerFromPotentialPartialName: " + ex.Message);
            }
            return null;
        }

        private string GetDirectionFromAngle(float angle)
        {
            if (angle < 22.5f)
                return GetText("Words_north");
            if (angle < 67.5f)
                return GetText("Words_northeast");
            if (angle < 112.5f)
                return GetText("Words_east");
            if (angle < 157.5f)
                return GetText("Words_southeast");
            if (angle < 202.5f)
                return GetText("Words_south");
            if (angle < 247.5f)
                return GetText("Words_southwest");
            if (angle < 292.5f)
                return GetText("Words_west");
            if (angle < 337.5f)
                return GetText("Words_northwest");
            return GetText("Words_north");
        }
    }
}
