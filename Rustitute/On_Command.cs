using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pluton;
using Pluton.Events;
using ProtoBuf;
using UnityEngine;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_Command(CommandEvent cmd)
        {
            if (cmd.cmd == "help" || cmd.cmd == "commands")
            {
                Help(cmd.User);
            }
            else if (cmd.cmd == "players" || cmd.cmd == "who")
            {
                SendMessage(cmd.User, null, "There are " + (Server.ActivePlayers.Count) + " players online and " + (Server.GetServer().SleepingPlayers.Count) + " sleeping players.");

                String playersWithNoInfo = "";
                for (var i = 0; i < Server.ActivePlayers.Count; i++)
                {
                    playersWithNoInfo += Server.ActivePlayers[i].Name + ". ";
                }
                SendMessage(cmd.User, null, "Online players: " + playersWithNoInfo);
            }
            else if (cmd.cmd == "arenaplayers")
            {
                IDictionary<string, string> arenaPlayers = PlayersInArena();
                string arenaS = arenaPlayers.Count() == 1 ? "" : "s";

                SendMessage(cmd.User, null, "There are " + arenaPlayers.Count() + " player" + arenaS + " in the arena.");

                if (arenaPlayers.Any())
                {
                    String playersWithNoInfo = "";
                    foreach (var player in arenaPlayers)
                    {
                        playersWithNoInfo += player.Value + ". ";
                    }
                    SendMessage(cmd.User, null, "Arena players: " + playersWithNoInfo);
                }
            }
            else if ((cmd.cmd == "aplayers" || cmd.cmd == "awho") && (cmd.User.Owner))
            {
                SendMessage(cmd.User, null, "There are " + (Server.ActivePlayers.Count) + " players online and " + (Server.GetServer().SleepingPlayers.Count) + " sleeping players.");

                String playersWithInfo = "";
                for (var i = 0; i < Server.ActivePlayers.Count; i++)
                {
                    playersWithInfo += Server.ActivePlayers[i].Name + " (H:" + Server.ActivePlayers[i].Health + ",D:" + Int32.Parse(Vector3.Distance(cmd.User.Location, Server.ActivePlayers[i].Location).ToString()) + "). ";
                }
                SendMessage(cmd.User, null, "Online players: " + playersWithInfo);
            }
            else if (cmd.cmd == "location" || cmd.cmd == "loc" || cmd.cmd == "whereami")
            {
                SendMessage(cmd.User, null,
                    "Your location: X=" + (cmd.User.X) + " Y=" + (cmd.User.Y) + " Z=" + (cmd.User.Z) + " facing " + GetDirectionFromAngle(cmd.User.basePlayer.eyes.rotation.eulerAngles.y));
            }
            else if ((cmd.cmd == "alocation" || cmd.cmd == "aloc") && cmd.User.Owner)
            {
                Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                if (otherPlayer == null)
                {
                    SendMessage(cmd.User, null, "Player not found!");
                    return;
                }
                SendMessage(cmd.User, null, "Location of " + otherPlayer.Name + ": X=" + (otherPlayer.X) + " Y=" + (otherPlayer.Y) + " Z=" + (otherPlayer.Z) + " facing " + GetDirectionFromAngle(cmd.User.basePlayer.eyes.rotation.eulerAngles.y));
            }
            else if (cmd.cmd == "tphome")
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
                {
                    SendMessage(cmd.User, null, "You cannot teleport while in the arena!");
                    return;
                }

                if (GetSetting("user_" + cmd.User.SteamID, "tpHomeX").Length == 0)
                    SendMessage(cmd.User, null, "Your have no home position to teleport to!");
                else
                {
                    float x = float.Parse(GetSetting("user_" + cmd.User.SteamID, "tpHomeX"));
                    float y = float.Parse(GetSetting("user_" + cmd.User.SteamID, "tpHomeY"));
                    float z = float.Parse(GetSetting("user_" + cmd.User.SteamID, "tpHomeZ"));
                    cmd.User.Teleport(x, y, z);
                    SendMessage(cmd.User, null, "You have been teleported home!");
                }
            }
            else if (cmd.cmd == "tpsethome")
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
                {
                    SendMessage(cmd.User, null, "You cannot teleport while in the arena!");
                    return;
                }

                SetSetting("user_" + cmd.User.SteamID, "tpHomeX", cmd.User.Location.x.ToString());
                SetSetting("user_" + cmd.User.SteamID, "tpHomeY", cmd.User.Location.y.ToString());
                SetSetting("user_" + cmd.User.SteamID, "tpHomeZ", cmd.User.Location.z.ToString());

                SendMessage(cmd.User, null, "Your home position has been set");
            }
            else if (cmd.cmd == "tp")
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
                {
                    SendMessage(cmd.User, null, "You cannot teleport while in the arena!");
                    return;
                }

                if (cmd.quotedArgs.Count() == 2 && cmd.quotedArgs[1].Length > 0 && cmd.User.Owner)
                {
                    Player firstPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    Player secondPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[1]);

                    if (firstPlayer == null)
                    {
                        SendMessage(cmd.User, null, "First player not found!");
                        return;
                    }

                    if (secondPlayer == null)
                    {
                        SendMessage(cmd.User, null, "Second player not found!");
                        return;
                    }

                    if (GetSettingBool("user_" + firstPlayer.SteamID, "inArena"))
                    {
                        SendMessage(cmd.User, null, firstPlayer.Name + " is in the arena and cannot be teleported!");
                        return;
                    }

                    if (GetSettingBool("user_" + secondPlayer.SteamID, "inArena"))
                    {
                        SendMessage(cmd.User, null, secondPlayer.Name + " is in the arena and cannot be teleported!");
                        return;
                    }

                    if (firstPlayer != null && secondPlayer != null)
                    {
                        firstPlayer.Teleport(secondPlayer.Location);
                        SendMessage(cmd.User, null, firstPlayer.Name + " has been teleported to " + secondPlayer.Name);
                    }
                }
                else if (cmd.quotedArgs.Count() == 1)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, "Player not found!");
                        return;
                    }

                    if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
                    {
                        SendMessage(cmd.User, null, otherPlayer.Name + "is in the arena and cannot be teleported!");
                        return;
                    }

                    if (otherPlayer != null)
                    {
                        cmd.User.Teleport(otherPlayer.Location);
                        SendMessage(cmd.User, null, "You have been teleported to " + otherPlayer.Name);
                    }
                }
            }
            else if (cmd.cmd == "tparena" && cmd.User.Owner)
            {
                float x = float.Parse(GetSetting("Arena", "locationX"));
                float y = float.Parse(GetSetting("Arena", "locationY"));
                float z = float.Parse(GetSetting("Arena", "locationZ"));

                cmd.User.Teleport(x, y, z);
                SendMessage(cmd.User, null, "You have been teleported to the arena!");
            }
            else if (cmd.cmd == "tpto" && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
                {
                    SendMessage(cmd.User, null, "You cannot teleport while in the arena!");
                    return;
                }

                if (cmd.quotedArgs.Count() == 4)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, "Player not found!");
                        return;
                    }

                    if (GetSettingBool("user_" + otherPlayer.SteamID, "inArena"))
                    {
                        SendMessage(cmd.User, null, otherPlayer.Name + " is in the arean and cannot be teleported!");
                        return;
                    }

                    otherPlayer.Teleport(float.Parse(cmd.quotedArgs[1]), float.Parse(cmd.quotedArgs[2]), float.Parse(cmd.quotedArgs[3]));
                    SendMessage(cmd.User, null, otherPlayer.Name + "has been teleported!");
                }
                else if (cmd.quotedArgs.Count() == 3)
                {
                    cmd.User.Teleport(float.Parse(cmd.quotedArgs[0]), float.Parse(cmd.quotedArgs[1]), float.Parse(cmd.quotedArgs[2]));
                    SendMessage(cmd.User, null, "You have been teleported!");
                }
            }
            else if (cmd.cmd == "tpx" && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
                {
                    SendMessage(cmd.User, null, "You cannot teleport while in the arena!");
                    return;
                }

                Vector3 lookPoint = cmd.User.GetLookPoint(2000f);
                if (Math.Abs(lookPoint.x) <= 0 && Math.Abs(lookPoint.y) <= 0 && Math.Abs(lookPoint.z) <= 0)
                {
                    SendMessage(cmd.User, null, "Crosshair position too far or invalid, aim a little closer!");
                }
                else
                {
                    cmd.User.Teleport(lookPoint);
                    SendMessage(cmd.User, null, "You have been teleported!");
                }
            }
            else if (cmd.cmd == "heal" && cmd.User.Owner)
            {
                Heal(cmd.User);
                SendMessage(cmd.User, null, "You have been healed!");
            }
            else if (cmd.cmd == "jump" && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    float jumpAmount = float.Parse(cmd.quotedArgs[0]);
                    cmd.User.Teleport(cmd.User.X, cmd.User.Y + jumpAmount, cmd.User.Z);
                }
                else if (cmd.quotedArgs.Count() == 2)
                {
                    /*
                    float jumpAmountUp = float.Parse(cmd.quotedArgs[0]);
                    float jumpAmountForward = float.Parse(cmd.quotedArgs[1]);

                    var facing = cmd.User.basePlayer.eyes.rotation.eulerAngles.y;

                    cmd.User.Teleport(cmd.User.X, cmd.User.Y + jumpAmountUp, cmd.User.Z);
                    */
                }
            }
            else if (cmd.cmd == "starter")
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
                    return;

                var belt = cmd.User.Inventory._inv.containerBelt;
                var wear = cmd.User.Inventory._inv.containerWear;
                var main = cmd.User.Inventory._inv.containerMain;

                cmd.User.Inventory.Add(new InvItem("hammer", 1), belt);
                cmd.User.Inventory.Add(new InvItem("pistol_revolver", 1), belt);
                cmd.User.Inventory.Add(new InvItem("hatchet", 1), belt);
                cmd.User.Inventory.Add(new InvItem("building_planner", 1), belt);

                cmd.User.Inventory.Add(new InvItem("sleepingbag", 1), main);
                cmd.User.Inventory.Add(new InvItem("wolfmeat_cooked", 20), main);
                cmd.User.Inventory.Add(new InvItem("ammo_pistol", 25), main);

                SendMessage(cmd.User, null, "A starter kit has been given to you!");
            }
            else if (cmd.cmd == "adminkit" && cmd.User.Owner)
            {
                var belt = cmd.User.Inventory._inv.containerBelt;
                var wear = cmd.User.Inventory._inv.containerWear;
                var main = cmd.User.Inventory._inv.containerMain;

                cmd.User.Inventory.Add(new InvItem("rifle_ak", 1), belt);
                cmd.User.Inventory.Add(new InvItem("rifle_bolt", 1), belt);
                cmd.User.Inventory.Add(new InvItem("hammer", 1), belt);
                cmd.User.Inventory.Add(new InvItem("building_planner", 1), belt);

                cmd.User.Inventory.Add(new InvItem("shotgun_waterpipe", 1), main);
                cmd.User.Inventory.Add(new InvItem("smg_thompson", 1), main);
                cmd.User.Inventory.Add(new InvItem("bow_hunting", 1), main);
                cmd.User.Inventory.Add(new InvItem("hatchet", 1), main);
                cmd.User.Inventory.Add(new InvItem("ammo_pistol", 10000), main);
                cmd.User.Inventory.Add(new InvItem("ammo_rifle", 10000), main);
                cmd.User.Inventory.Add(new InvItem("ammo_shotgun", 10000), main);
                cmd.User.Inventory.Add(new InvItem("arrow_wooden", 10000), main);
                cmd.User.Inventory.Add(new InvItem("cupboard.tool", 10), main);
                cmd.User.Inventory.Add(new InvItem("lock.code", 10), main);
                cmd.User.Inventory.Add(new InvItem("furnace", 10), main);
                cmd.User.Inventory.Add(new InvItem("box_wooden_large", 10), main);
                cmd.User.Inventory.Add(new InvItem("metal_fragments", 1000000), main);
                cmd.User.Inventory.Add(new InvItem("stones", 1000000), main);
                cmd.User.Inventory.Add(new InvItem("wood", 1000000), main);
                cmd.User.Inventory.Add(new InvItem("cloth", 1000000), main);
                cmd.User.Inventory.Add(new InvItem("bandage", 500), main);
                cmd.User.Inventory.Add(new InvItem("largemedkit", 500), main);
                cmd.User.Inventory.Add(new InvItem("wolfmeat_cooked", 1000), main);

                cmd.User.Inventory.Add(new InvItem("metal_facemask", 1), wear);
                cmd.User.Inventory.Add(new InvItem("metal_plate_torso", 1), wear);
                cmd.User.Inventory.Add(new InvItem("urban_pants", 1), wear);
                cmd.User.Inventory.Add(new InvItem("urban_boots", 1), wear);
                cmd.User.Inventory.Add(new InvItem("burlap_gloves", 1), wear);

                SendMessage(cmd.User, null, "An admin kit has been given to you!");
            }
            else if (cmd.cmd == "time")
            {
                if (cmd.quotedArgs.Count() == 1 && cmd.quotedArgs[0].Length > 0 && cmd.User.Owner)
                {
                    World.Time = float.Parse(cmd.quotedArgs[0]);
                    SendMessage(cmd.User, null, "Time set!");
                }
                else
                {
                    var time = Mathf.RoundToInt(World.Time);
                    string when = " in the morning";
                    if (time == 0)
                        when = " at night";
                    else if (time == 12)
                        when = " noon";
                    else if (time >= 13)
                    {
                        when = " in the afternoon";
                        if (time >= 17)
                            when = " in the evening";
                        else if (time >= 20)
                            when = " at night";
                        time -= 12;
                    }
                    SendMessage(cmd.User, null, "It is currently around " + time + when);
                }

                CheckLanterns();
            }
            else if (cmd.cmd == "airdrop" && cmd.User.Owner && false)
            {
                if (cmd.quotedArgs.Count() == 1 && cmd.quotedArgs[0].Length > 0)
                {
                    World.AirDropAtPlayer(GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]));
                    SendMessage(cmd.User, null, "An airdrop has been triggered above " + GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]).Name);
                }
                else
                {
                    World.AirDrop();
                    SendMessage(cmd.User, null, "An airdrop has been triggered");
                }
            }
            else if (cmd.cmd == "animal" && cmd.User.Owner && false)
            {
                World.SpawnAnimal("wolf", GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]).Location);
                SendMessage(cmd.User, null, "A wolf has been spawned by " + GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]).Name);
            }
            else if (cmd.cmd == "adminmsg" && cmd.User.Owner)
            {
                SendMessageToAdmins(cmd.User.Name + ": " + String.Join(" ", cmd.args));
            }
            else if (cmd.cmd == "botname" && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1 && cmd.quotedArgs[0].Length > 0)
                {
                    botName = SetSetting("Settings", "BotName", cmd.quotedArgs[0]);
                    Server.server_message_name = botName;
                    SendMessage(cmd.User, null, "Bot name set to " + botName);
                }
            }
            else if (cmd.cmd == "get" && cmd.User.Owner && false)
            {
                if (cmd.quotedArgs.Count() == 2 && cmd.quotedArgs[0].Length > 0 && cmd.quotedArgs[1].Length > 0)
                {
                    SendMessage(cmd.User, null, GetSetting(cmd.quotedArgs[0], cmd.quotedArgs[1]));
                }
            }
            else if (cmd.cmd == "ko" && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "ko"))
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "ko", false);
                    SendMessage(cmd.User, null, "KO mode disabled!");
                }
                else
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "ko", true);
                    SendMessage(cmd.User, null, "KO mode enabled!");
                }
            }
            else if (cmd.cmd == "koall" && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "koall"))
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "koall", false);
                    SendMessage(cmd.User, null, "KO All mode disabled!");
                }
                else
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "koall", true);
                    SendMessage(cmd.User, null, "KO All mode enabled!");
                }
            }
            else if (cmd.cmd == "god" && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, "Player not found!");
                        return;
                    }

                    if (GetSettingBool("user_" + otherPlayer.SteamID, "god"))
                    {
                        SetSettingBool("user_" + otherPlayer.SteamID, "god", false);
                        otherPlayer.basePlayer.InitializeHealth(100, 100);
                        SendMessage(cmd.User, null, "God mode disabled for " + otherPlayer.Name);
                        SendMessage(otherPlayer, null, "Your god mode has been disabled!");
                    }
                    else
                    {
                        Heal(otherPlayer);
                        otherPlayer.basePlayer.InitializeHealth(float.MaxValue, float.MaxValue);
                        SetSettingBool("user_" + otherPlayer.SteamID, "god", true);
                        SendMessage(cmd.User, null, "God mode enabled for " + otherPlayer.Name);
                        SendMessage(otherPlayer, null, "You have been given god mode!");
                    }
                }
                else
                {
                    if (GetSettingBool("user_" + cmd.User.SteamID, "god"))
                    {
                        SetSettingBool("user_" + cmd.User.SteamID, "god", false);
                        cmd.User.basePlayer.InitializeHealth(100, 100);
                        SendMessage(cmd.User, null, "God mode disabled!");
                    }
                    else
                    {
                        Heal(cmd.User);
                        cmd.User.basePlayer.InitializeHealth(float.MaxValue, float.MaxValue);
                        SetSettingBool("user_" + cmd.User.SteamID, "god", true);
                        SendMessage(cmd.User, null, "God mode enabled!");
                    }
                }
            }
            else if (cmd.cmd == "arenabuild" && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "arenabuild"))
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "arenabuild", false);
                    SendMessage(cmd.User, null, "You can no longer build at the arena!");
                }
                else
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "arenabuild", true);
                    SendMessage(cmd.User, null, "You can now build at the arena!");
                }
            }
            else if (cmd.cmd == "instabuild" && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "instabuild"))
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "instabuild", false);
                    SendMessage(cmd.User, null, "Instabuild disabled!");
                }
                else
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "instabuild", true);
                    SendMessage(cmd.User, null, "Instabuild enabled!");
                }
            }
            else if (cmd.cmd == "instamax" && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "instamax"))
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "instamax", false);
                    SendMessage(cmd.User, null, "Insta Max disabled!");
                }
                else
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "instamax", true);
                    SendMessage(cmd.User, null, "Insta Max enabled!");
                }
            }
            else if (cmd.cmd == "nosleep")
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "nosleep"))
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "nosleep", false);
                    SendMessage(cmd.User, null, "You will now be sleeping after spawning");
                }
                else
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "nosleep", true);
                    SendMessage(cmd.User, null, "You will now be awake after spawning");
                }
            }
            else if (cmd.cmd == "motd" && cmd.User.Owner)
            {
                if (!cmd.quotedArgs.Any())
                {
                    SetSetting("Settings", "motd", "");
                }
                else
                {
                    SetSetting("Settings", "motd", String.Join(" ", cmd.args));
                }
                SendMessage(cmd.User, null, "Motd set!");
            }
            else if (cmd.cmd == "copy" && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    SetSetting("user_" + cmd.User.SteamID, "copy", cmd.quotedArgs[0]);
                    SendMessage(cmd.User, null, "Copy mode set to " + cmd.quotedArgs[0]);
                }
                else
                {
                    SetSetting("user_" + cmd.User.SteamID, "copy", "");
                    SendMessage(cmd.User, null, "Copy mode disabled!");
                }
            }
            else if (cmd.cmd == "destroy" && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    DestroyEverything(cmd.User.Location, float.Parse(cmd.quotedArgs[0]));
                }
            }
            else if (cmd.cmd == "destroylantern" && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    DestroyLanterns(cmd.User.Location, float.Parse(cmd.quotedArgs[0]));
                }
            }
            else if (cmd.cmd == "top10")
            {

            }
            else if (cmd.cmd == "kick" && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, "Player not found!");
                        return;
                    }

                    otherPlayer.Kick();
                }
            }
            else if (cmd.cmd == "kill" && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, "Player not found!");
                        return;
                    }

                    otherPlayer.Kill();
                }
            }
            else if (cmd.cmd == "ban" && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, "Player not found!");
                        return;
                    }

                    otherPlayer.Ban();
                }
            }
            else if (cmd.cmd == "mute" && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, "Player not found!");
                        return;
                    }

                    if (GetSettingBool("user_" + otherPlayer.SteamID, "muted"))
                    {
                        SetSettingBool("user_" + otherPlayer.SteamID, "muted", false);
                        SendMessage(cmd.User, null, otherPlayer.Name + " has been unmuted");
                    }
                    else
                    {
                        SetSettingBool("user_" + otherPlayer.SteamID, "muted", true);
                        SendMessage(cmd.User, null, otherPlayer.Name + " has been muted");
                    }
                }
            }
            else if (cmd.cmd == "silence" && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, "Player not found!");
                        return;
                    }

                    if (GetSettingBool("user_" + otherPlayer.SteamID, "silenced"))
                    {
                        SetSettingBool("user_" + otherPlayer.SteamID, "silenced", false);
                        SendMessage(cmd.User, null, otherPlayer.Name + " has been unsilenced");
                    }
                    else
                    {
                        SetSettingBool("user_" + otherPlayer.SteamID, "silenced", true);
                        SendMessage(cmd.User, null, otherPlayer.Name + " has been silenced");
                    }
                }
            }
            else if (cmd.cmd == "save" && cmd.User.Owner)
            {
                ini.Save();
                iniArena.Save();
                SendMessage(cmd.User, null, "Rustitute data saved!");
            }
            else if (cmd.cmd == "load" && cmd.User.Owner)
            {
                ini = Plugin.GetIni(pluginIni);
                iniArena = Plugin.GetIni(pluginIni + "Arena");
                SendMessage(cmd.User, null, "Rustitute data loaded!");
            }
            else if (cmd.cmd == "togglearena" && cmd.User.Owner)
            {
                if (GetSettingBool("Settings", "arenaEnabled"))
                {
                    SetSettingBool("Settings", "arenaEnabled", false);
                    SendMessage(cmd.User, null, "The arena has been disabled");
                }
                else
                {
                    SetSettingBool("Settings", "arenaEnabled", true);
                    SendMessage(cmd.User, null, "The arena has been enabled");
                }
            }
            else if (cmd.cmd == "logarena" && cmd.User.Owner)
            {
                LogArena(cmd);
            }
            else if (cmd.cmd == "respawnarena" && cmd.User.Owner)
            {
                DestroyArena(cmd);
                SpawnArena(cmd);
            }
            else if (cmd.cmd == "spawnarena" && cmd.User.Owner)
            {
                SpawnArena(cmd);
            }
            else if (cmd.cmd == "destroyarena" && cmd.User.Owner)
            {
                DestroyArena(cmd);
            }
            else if (cmd.cmd == "addspawn" && cmd.User.Owner)
            {
                AddSpawn(cmd);
            }
            else if (cmd.cmd == "arenahere" && cmd.User.Owner)
            {
                ArenaHere(cmd);
            }
            else if (cmd.cmd == "arena")
            {
                Arena(cmd);
            }
            else if (cmd.cmd == "achievements")
            {
                Player player = cmd.User;
                if (cmd.quotedArgs.Count() == 1)
                {
                    player = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (player == null)
                    {
                        SendMessage(cmd.User, null, "Player not found!");
                        return;
                    }
                }

                var achievements = Achievements();

                String[] userData = ini.EnumSection("user_" + player.SteamID);

                System.Array.Sort(userData);

                SendMessage(cmd.User, null, "Achievements for " + player.Name);

                foreach (var item in userData)
                {
                    if (item.StartsWith("achievement_"))
                    {
                        SendMessage(cmd.User, null, "[Achievement] " + achievements[item.Replace("achievement_", "")]);
                    }
                }
            }
            else if (cmd.cmd == "stats" || cmd.cmd == "mystats")
            {
                Player player = cmd.User;
                if (cmd.quotedArgs.Count() == 1)
                {
                    player = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (player == null)
                    {
                        SendMessage(cmd.User, null, "Player not found!");
                        return;
                    }
                }

                SendMessage(cmd.User, null, "Stats for " + player.Name);
                SendMessage(cmd.User, null, "Kills: " + GetSetting("user_" + player.SteamID, "kills"));
                SendMessage(cmd.User, null, "Arena Kills: " + GetSetting("user_" + player.SteamID, "killsArena"));
                SendMessage(cmd.User, null, "Headshots: " + GetSetting("user_" + player.SteamID, "headshots"));
                SendMessage(cmd.User, null, "Deaths: " + GetSetting("user_" + player.SteamID, "deaths"));
                SendMessage(cmd.User, null, "Arena Deaths: " + GetSetting("user_" + player.SteamID, "deathsArena"));
            }
            else if (cmd.cmd == "fx" && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    cmd.User.basePlayer.SendEffect(cmd.quotedArgs[0]);
                }
                else if (cmd.quotedArgs.Count() == 2)
                {
                    Player player = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (player == null)
                    {
                        SendMessage(cmd.User, null, "Player not found!");
                        return;
                    }
                    player.basePlayer.SendEffect(cmd.quotedArgs[1]);
                }
            }
            else if (cmd.cmd == "test" && cmd.User.Owner)
            {
                
            }
        }
    }
}
