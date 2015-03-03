using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pluton;
using Pluton.Events;
using UnityEngine;
using ItemContainer = ProtoBuf.ItemContainer;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_Command(CommandEvent cmd)
        {
            if (cmd.cmd == GetCmd("help"))
            {
                Help(cmd.User);
            }
            else if (cmd.cmd == GetCmd("players"))
            {
                SendMessage(cmd.User, null, String.Format(GetText("Words_players"), (Server.ActivePlayers.Count), (Server.GetServer().SleepingPlayers.Count)));

                String playersWithNoInfo = "";
                for (var i = 0; i < Server.ActivePlayers.Count; i++)
                {
                    playersWithNoInfo += Server.ActivePlayers[i].Name + ". ";
                }
                SendMessage(cmd.User, null, GetText("Words_playersOnline") + " " + playersWithNoInfo);
            }
            else if (cmd.cmd == GetCmd("arenaplayers"))
            {
                IDictionary<string, string> arenaPlayers = PlayersInArena();
                string arenaS = arenaPlayers.Count() == 1 ? "" : GetText("Words_plural");

                SendMessage(cmd.User, null, String.Format(GetText("Words_arenaPlayers"), arenaPlayers.Count(), arenaS));

                if (arenaPlayers.Any())
                {
                    String playersWithNoInfo = "";
                    foreach (var player in arenaPlayers)
                    {
                        playersWithNoInfo += player.Value + ". ";
                    }
                    SendMessage(cmd.User, null, GetText("Words_arenaPlayersOnline") + " " + playersWithNoInfo);
                }
            }
            else if (cmd.cmd == GetCmd("aplayers") && cmd.User.Owner)
            {
                SendMessage(cmd.User, null, String.Format(GetText("Words_players"), (Server.ActivePlayers.Count), (Server.GetServer().SleepingPlayers.Count)));

                String playersWithInfo = "";
                for (var i = 0; i < Server.ActivePlayers.Count; i++)
                {
                    int health = 0;
                    int distance = -1;

                    try
                    {
                        health = Convert.ToInt32(Server.ActivePlayers[i].Health);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("H: " + ex.ToString());
                    }

                    try
                    {
                        distance = Convert.ToInt32(Vector3.Distance(cmd.User.Location, Server.ActivePlayers[i].Location));
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("D: " + ex.ToString());
                    }

                    playersWithInfo += Server.ActivePlayers[i].Name + " (H:" + health + ",D:" + (distance == -1 ? "?" : distance.ToString()) + "). ";
                }
                SendMessage(cmd.User, null, GetText("Words_playersOnline") + " " + playersWithInfo);
            }
            else if (cmd.cmd == GetCmd("location"))
            {
                SendMessage(cmd.User, null, String.Format(GetText("Words_location"), cmd.User.X, cmd.User.Y, cmd.User.Z, GetDirectionFromAngle(cmd.User.basePlayer.eyes.rotation.eulerAngles.y)));
            }
            else if (cmd.cmd == GetCmd("alocation") && cmd.User.Owner)
            {
                Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                if (otherPlayer == null)
                {
                    SendMessage(cmd.User, null, GetText("Words_PlayerNotFound"));
                    return;
                }
                SendMessage(cmd.User, null, String.Format(GetText("Words_location"), otherPlayer.X, otherPlayer.Y, otherPlayer.Z, GetDirectionFromAngle(otherPlayer.basePlayer.eyes.rotation.eulerAngles.y)));
            }
            else if (cmd.cmd == GetCmd("tphome"))
            {
                if (TimeRestrict(cmd.User, "attacked", 15, GetText("Words_TPUnderAttack")))
                    return;

                if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
                {
                    SendMessage(cmd.User, null, GetText("Words_TPArena"));
                    return;
                }

                if (GetSetting("user_" + cmd.User.SteamID, "tpHomeX").Length == 0)
                    SendMessage(cmd.User, null, GetText("Words_TPNoHome"));
                else
                {
                    float x = float.Parse(GetSetting("user_" + cmd.User.SteamID, "tpHomeX"));
                    float y = float.Parse(GetSetting("user_" + cmd.User.SteamID, "tpHomeY"));
                    float z = float.Parse(GetSetting("user_" + cmd.User.SteamID, "tpHomeZ"));
                    cmd.User.Teleport(x, y, z);
                    SendMessage(cmd.User, null, GetText("Words_TPHome"));
                }
            }
            else if (cmd.cmd == GetCmd("tpsethome"))
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
                {
                    SendMessage(cmd.User, null, GetText("Words_TPArena"));
                    return;
                }

                if (IsInArena(cmd.User.Location))
                {
                    SendMessage(cmd.User, null, GetText("Words_TPTooCloseToArena"));
                    return;
                }

                SetSetting("user_" + cmd.User.SteamID, "tpHomeX", cmd.User.Location.x.ToString());
                SetSetting("user_" + cmd.User.SteamID, "tpHomeY", cmd.User.Location.y.ToString());
                SetSetting("user_" + cmd.User.SteamID, "tpHomeZ", cmd.User.Location.z.ToString());

                SendMessage(cmd.User, null, GetText("Words_TPHomeSet"));
            }
            else if (cmd.cmd == GetCmd("tp"))
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
                {
                    SendMessage(cmd.User, null, GetText("Words_TPArena"));
                    return;
                }

                if (cmd.quotedArgs.Count() == 2 && cmd.quotedArgs[1].Length > 0 && cmd.User.Owner)
                {
                    Player firstPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    Player secondPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[1]);

                    if (firstPlayer == null)
                    {
                        SendMessage(cmd.User, null, GetText("Words_FirstNotFound"));
                        return;
                    }

                    if (secondPlayer == null)
                    {
                        SendMessage(cmd.User, null, GetText("Words_SecondNotFound"));
                        return;
                    }

                    if (GetSettingBool("user_" + firstPlayer.SteamID, "inArena"))
                    {
                        SendMessage(cmd.User, null, String.Format(GetText("Words_TPUserInArena"), firstPlayer.Name));
                        return;
                    }

                    if (GetSettingBool("user_" + secondPlayer.SteamID, "inArena"))
                    {
                        SendMessage(cmd.User, null, String.Format(GetText("Words_TPUserInArena2"), secondPlayer.Name));
                        return;
                    }

                    if (firstPlayer != null && secondPlayer != null)
                    {
                        firstPlayer.Teleport(secondPlayer.Location);
                        SendMessage(cmd.User, null, String.Format(GetText("Words_TPUserToUser"), firstPlayer.Name, secondPlayer.Name));
                    }
                }
                else if (cmd.quotedArgs.Count() == 1)
                {
                    if (TimeRestrict(cmd.User, "attacked", 15, GetText("Words_TPUnderAttack")))
                        return;

                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, GetText("Words_PlayerNotFound"));
                        return;
                    }

                    if (otherPlayer.SteamID == cmd.User.SteamID)
                    {
                        SendMessage(cmd.User, null, GetText("Words_TPNoSelf"));
                        return;
                    }

                    if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
                    {
                        SendMessage(cmd.User, null, String.Format(GetText("Words_TPUserInArena2"), otherPlayer.Name));
                        return;
                    }

                    if (TimeRestrict(cmd.User, cmd.cmd, 60 * 3, GetText("Words_TPWaitTime")))
                        return;

                    if (otherPlayer != null)
                    {
                        if (GetSettingBool("user_" + otherPlayer.SteamID, "tpw_" + cmd.User.SteamID))
                        {
                            TimeRestrictSet(cmd.User, cmd.cmd);
                            cmd.User.Teleport(otherPlayer.Location);
                            SendMessage(cmd.User, null, String.Format(GetText("Words_TPToUser"), otherPlayer.Name));
                        }
                        else
                        {
                            int otherTime = Epoch() - Convert.ToInt32(GetSettingInt("user_" + otherPlayer.SteamID, "tpFromTime"));

                            if ((GetSetting("user_" + otherPlayer.SteamID, "tpFrom").Length > 1) && (otherTime <= 30))
                            {
                                SendMessage(cmd.User, null, GetText("Words_TPPending"));
                                return;
                            }

                            SetSetting("user_" + otherPlayer.SteamID, "tpFrom", cmd.User.SteamID);
                            SetSetting("user_" + otherPlayer.SteamID, "tpFromTime", Epoch().ToString());

                            SendMessage(cmd.User, null, String.Format(GetText("Words_TPRequestSent"), otherPlayer.Name));
                            SendMessage(otherPlayer, null, String.Format(GetText("Words_TPRequestFrom"), cmd.User.Name));
                        }
                    }
                }
            }
            else if (cmd.cmd == GetCmd("tpa"))
            {
                try
                {
                    int tpTime = Epoch() - Convert.ToInt32(GetSettingInt("user_" + cmd.User.SteamID, "tpFromTime"));
                    ulong tpFrom = Convert.ToUInt64(GetSetting("user_" + cmd.User.SteamID, "tpFrom"));

                    if (tpFrom > 0 && tpTime <= 30)
                    {
                        var fromPlayer = Server.FindPlayer(tpFrom);
                        if (fromPlayer.Offline)
                        {
                            SendMessage(cmd.User, null, GetText("Words_TPNotOnline"));
                            return;
                        }

                        TimeRestrictSet(fromPlayer, "tp");

                        SetSetting("user_" + cmd.User.SteamID, "tpFrom", "0");
                        SetSetting("user_" + cmd.User.SteamID, "tpFromTime", "0");

                        fromPlayer.Teleport(cmd.User.Location);
                        SendMessage(fromPlayer, null, String.Format(GetText("Words_TPdTo"), cmd.User.Name));
                    }
                    else
                    {
                        SendMessage(cmd.User, null, GetText("Words_TPNoRequest"));
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
            else if (cmd.cmd == GetCmd("tpw"))
            {
                Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                if (otherPlayer == null)
                {
                    SendMessage(cmd.User, null, GetText("Words_PlayerNotFound"));
                    return;
                }

                if (GetSettingBool("user_" + cmd.User.SteamID, "tpw_" + otherPlayer.SteamID))
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "tpw_" + otherPlayer.SteamID, false);
                    SendMessage(cmd.User, null, String.Format(GetText("Words_TPWRemoved"), otherPlayer.Name));
                }
                else
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "tpw_" + otherPlayer.SteamID, true);
                    SendMessage(cmd.User, null, String.Format(GetText("Words_TPWAdded"), otherPlayer.Name));
                }
            }
            else if (cmd.cmd == GetCmd("tparena") && cmd.User.Owner)
            {
                float x = float.Parse(GetSetting("Arena", "locationX"));
                float y = float.Parse(GetSetting("Arena", "locationY"));
                float z = float.Parse(GetSetting("Arena", "locationZ"));

                cmd.User.Teleport(x, y, z);
                SendMessage(cmd.User, null, GetText("Words_TPToArena"));
            }
            else if (cmd.cmd == GetCmd("tpto") && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
                {
                    SendMessage(cmd.User, null, GetText("Words_TPArena"));
                    return;
                }

                if (cmd.quotedArgs.Count() == 4)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, GetText("Words_PlayerNotFound"));
                        return;
                    }

                    if (GetSettingBool("user_" + otherPlayer.SteamID, "inArena"))
                    {
                        SendMessage(cmd.User, null, String.Format(GetText("Words_TPUserInArena"), otherPlayer.Name));
                        return;
                    }

                    otherPlayer.Teleport(float.Parse(cmd.quotedArgs[1]), float.Parse(cmd.quotedArgs[2]), float.Parse(cmd.quotedArgs[3]));
                    SendMessage(cmd.User, null, String.Format(GetText("Words_TPUserTo"), otherPlayer.Name));
                }
                else if (cmd.quotedArgs.Count() == 3)
                {
                    cmd.User.Teleport(float.Parse(cmd.quotedArgs[0]), float.Parse(cmd.quotedArgs[1]), float.Parse(cmd.quotedArgs[2]));
                    SendMessage(cmd.User, null, GetText("Words_TPd"));
                }
            }
            else if (cmd.cmd == GetCmd("tpx") && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
                {
                    SendMessage(cmd.User, null, GetText("Words_TPArena"));
                    return;
                }

                Vector3 lookPoint = cmd.User.GetLookPoint(2000f);
                if (Math.Abs(lookPoint.x) <= 0 && Math.Abs(lookPoint.y) <= 0 && Math.Abs(lookPoint.z) <= 0)
                {
                    SendMessage(cmd.User, null, GetText("Words_TPXFail"));
                }
                else
                {
                    cmd.User.Teleport(lookPoint);
                    SendMessage(cmd.User, null, GetText("Words_TPd"));
                }
            }
            else if (cmd.cmd == GetCmd("heal") && cmd.User.Owner)
            {
                Heal(cmd.User);
                SendMessage(cmd.User, null, GetText("Words_Healed"));
            }
            else if (cmd.cmd == GetCmd("owner") && cmd.User.Owner)
            {
                Collider[] castHits = Physics.OverlapSphere(cmd.User.Location, 50f, 1 << 8);

                foreach (Collider collider in castHits)
                {
                    var sleepingBag = collider.GetComponent<SleepingBag>();
                    var toolCupboard = collider.GetComponent<BuildingPrivlidge>();

                    int distance = Convert.ToInt32(Vector3.Distance(cmd.User.Location, collider.transform.position));

                    if (sleepingBag != null)
                        SendMessage(cmd.User, null, String.Format(GetText("Words_OwnerSleepingBag"), distance, GetText("Words_meters"), BasePlayer.FindByID(sleepingBag.deployerUserID).displayName));

                    if (toolCupboard != null)
                    {
                        if (toolCupboard.authorizedPlayers.Count > 0)
                        {
                            foreach (var player in toolCupboard.authorizedPlayers)
                            {
                                SendMessage(cmd.User, null, String.Format(GetText("Words_OwnerToolCupboard"), distance, GetText("Words_meters"), player.username));
                            }
                        }
                    }
                }
            }
            else if (cmd.cmd == GetCmd("give") && cmd.User.Owner)
            {
                Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                var item = cmd.quotedArgs[1];
                var qty = 1;
                if (cmd.quotedArgs.Count() == 3)
                    qty = Convert.ToInt32(cmd.quotedArgs[2]);

                var itemId = InvItem.GetItemID(item);
                if (itemId <= 0)
                {
                    SendMessage(cmd.User, null, GetText("Words_GiveNotFound"));
                    return;
                }

                otherPlayer.Inventory.Add(itemId, qty);

                if (cmd.User.SteamID != otherPlayer.SteamID)
                    SendMessage(cmd.User, null, String.Format(GetText("Words_GiveTo"), qty, item, otherPlayer.Name));
                SendMessage(otherPlayer, null, String.Format(GetText("Words_GiveToYou"), qty, item));
            }
            else if (cmd.cmd == GetCmd("jump") && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    float jumpAmount = float.Parse(cmd.quotedArgs[0]);
                    cmd.User.Teleport(cmd.User.X, cmd.User.Y + jumpAmount, cmd.User.Z);
                }
                else if (cmd.quotedArgs.Count() == 2)
                {

                }
            }
            else if (cmd.cmd == GetCmd("starter"))
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
                    return;

                if (TimeRestrict(cmd.User, cmd.cmd, 60 * 30, GetText("Words_StarterKitWaitTime")))
                    return;
                
                TimeRestrictSet(cmd.User, cmd.cmd);

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

                SendMessage(cmd.User, null, GetText("Words_StarterKitGiven"));
            }
            else if (cmd.cmd == GetCmd("adminkit") && cmd.User.Owner)
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

                SendMessage(cmd.User, null, GetText("Words_AdminKitGiven"));
            }
            else if (cmd.cmd == GetCmd("time"))
            {
                if (cmd.quotedArgs.Count() == 1 && cmd.quotedArgs[0].Length > 0 && cmd.User.Owner)
                {
                    World.Time = float.Parse(cmd.quotedArgs[0]);
                    SendMessage(cmd.User, null, GetText("Words_TimeSet"));
                }
                else
                {
                    var time = Mathf.RoundToInt(World.Time);
                    string when = " " + GetText("Words_TimeMorning");
                    if (time == 0)
                        when = " " + GetText("Words_TimeNight");
                    else if (time == 12)
                        when = " " + GetText("Words_TimeNoon");
                    else if (time >= 13)
                    {
                        when = " " + GetText("Words_TimeAfternoon");
                        if (time >= 17)
                            when = " " + GetText("Words_TimeEvening");
                        else if (time >= 20)
                            when = " " + GetText("Words_TimeNight");
                        time -= 12;
                    }
                    SendMessage(cmd.User, null, GetText("Words_TimeCurrentlyAround") + " " + time + when);
                }

                CheckLanterns();
            }
            else if (cmd.cmd == GetCmd("adminmsg") && cmd.User.Owner)
            {
                SendMessageToAdmins(cmd.User.Name + ": " + String.Join(" ", cmd.args));
            }
            else if (cmd.cmd == GetCmd("botname") && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1 && cmd.quotedArgs[0].Length > 0)
                {
                    botName = SetSetting("Settings", "BotName", cmd.quotedArgs[0]);
                    Server.server_message_name = botName;
                    SendMessage(cmd.User, null, GetText("Words_BotnameSet") + " " + botName);
                }
            }
            else if (cmd.cmd == GetCmd("disappear") && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "disappear"))
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "disappear", false);
                    SendMessage(cmd.User, null, GetText("Words_DisappearDisabled"));
                }
                else
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "disappear", true);
                    SendMessage(cmd.User, null, GetText("Words_DisappearEnabled"));
                }
            }
            else if (cmd.cmd == GetCmd("ko") && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "ko"))
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "ko", false);
                    SendMessage(cmd.User, null, GetText("Words_KODisabled"));
                }
                else
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "ko", true);
                    SendMessage(cmd.User, null, GetText("Words_KOEnabled"));
                }
            }
            else if (cmd.cmd == GetCmd("koall") && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "koall"))
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "koall", false);
                    SendMessage(cmd.User, null, GetText("Words_KOAllDisabled"));
                }
                else
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "koall", true);
                    SendMessage(cmd.User, null, GetText("Words_KOAllEnabled"));
                }
            }
            else if (cmd.cmd == GetCmd("god") && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, GetText("Words_PlayerNotFound"));
                        return;
                    }

                    if (GetSettingBool("user_" + otherPlayer.SteamID, "god"))
                    {
                        SetSettingBool("user_" + otherPlayer.SteamID, "god", false);
                        otherPlayer.basePlayer.InitializeHealth(100, 100);
                        SendMessage(cmd.User, null, String.Format(GetText("Words_GodDisabledFor"),otherPlayer.Name));
                        SendMessage(otherPlayer, null, GetText("Words_YourGodDisabled"));
                    }
                    else
                    {
                        Heal(otherPlayer);
                        otherPlayer.basePlayer.InitializeHealth(float.MaxValue, float.MaxValue);
                        SetSettingBool("user_" + otherPlayer.SteamID, "god", true);
                        SendMessage(cmd.User, null, String.Format(GetText("Words_GodEnabledFor"), otherPlayer.Name));
                        SendMessage(otherPlayer, null, "You have been given god mode!");
                    }
                }
                else
                {
                    if (GetSettingBool("user_" + cmd.User.SteamID, "god"))
                    {
                        SetSettingBool("user_" + cmd.User.SteamID, "god", false);
                        cmd.User.basePlayer.InitializeHealth(100, 100);
                        SendMessage(cmd.User, null, GetText("Words_GodDisabled"));
                    }
                    else
                    {
                        Heal(cmd.User);
                        cmd.User.basePlayer.InitializeHealth(float.MaxValue, float.MaxValue);
                        SetSettingBool("user_" + cmd.User.SteamID, "god", true);
                        SendMessage(cmd.User, null, GetText("Words_GodEnabled"));
                    }
                }
            }
            else if (cmd.cmd == GetCmd("arenabuild") && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "arenabuild"))
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "arenabuild", false);
                    SendMessage(cmd.User, null, GetText("Words_ArenaBuildDisabled"));
                }
                else
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "arenabuild", true);
                    SendMessage(cmd.User, null, GetText("Words_ArenaBuildEnabled"));
                }
            }
            else if (cmd.cmd == GetCmd("instamax") && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "instamax"))
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "instamax", false);
                    SendMessage(cmd.User, null, GetText("Words_InstaMaxDisabled"));
                }
                else
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "instamax", true);
                    SendMessage(cmd.User, null, GetText("Words_InstaMaxEnabled"));
                }
            }
            else if (cmd.cmd == GetCmd("nosleep") && cmd.User.Owner)
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "nosleep"))
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "nosleep", false);
                    SendMessage(cmd.User, null, GetText("Words_NoSleepDisabled"));
                }
                else
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "nosleep", true);
                    SendMessage(cmd.User, null, GetText("Words_NoSleepEnabled"));
                }
            }
            else if (cmd.cmd == GetCmd("motd") && cmd.User.Owner)
            {
                if (!cmd.quotedArgs.Any())
                {
                    SetSetting("Settings", "motd", "");
                }
                else
                {
                    SetSetting("Settings", "motd", String.Join(" ", cmd.args));
                }
                SendMessage(cmd.User, null, GetText("Words_MotdSet"));
            }
            else if (cmd.cmd == GetCmd("copy") && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    SetSetting("user_" + cmd.User.SteamID, "copy", cmd.quotedArgs[0]);
                    SendMessage(cmd.User, null, GetText("Words_CopyEnabled") + " " + cmd.quotedArgs[0]);
                }
                else
                {
                    SetSetting("user_" + cmd.User.SteamID, "copy", "");
                    SendMessage(cmd.User, null, GetText("Words_CopyDisabled"));
                }
            }
            else if (cmd.cmd == GetCmd("destroy") && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    DestroyEverything(cmd.User.Location, float.Parse(cmd.quotedArgs[0]));
                }
            }
            else if (cmd.cmd == GetCmd("kick") && (cmd.User.Owner || cmd.User.Moderator))
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, GetText("Words_PlayerNotFound"));
                        return;
                    }

                    otherPlayer.Kick();
                }
            }
            else if (cmd.cmd == GetCmd("kill") && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, GetText("Words_PlayerNotFound"));
                        return;
                    }

                    otherPlayer.Kill();
                }
            }
            else if (cmd.cmd == GetCmd("ban") && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, GetText("Words_PlayerNotFound"));
                        return;
                    }

                    otherPlayer.Ban();
                }
            }
            else if (cmd.cmd == GetCmd("mute") && (cmd.User.Owner || cmd.User.Moderator))
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, GetText("Words_PlayerNotFound"));
                        return;
                    }

                    if (GetSettingBool("user_" + otherPlayer.SteamID, "muted"))
                    {
                        SetSettingBool("user_" + otherPlayer.SteamID, "muted", false);
                        SendMessage(cmd.User, null, String.Format(GetText("Words_Unmuted"), otherPlayer.Name));
                    }
                    else
                    {
                        SetSettingBool("user_" + otherPlayer.SteamID, "muted", true);
                        SendMessage(cmd.User, null, String.Format(GetText("Words_Muted"), otherPlayer.Name));
                    }
                }
            }
            else if (cmd.cmd == GetCmd("silence") && cmd.User.Owner)
            {
                if (cmd.quotedArgs.Count() == 1)
                {
                    Player otherPlayer = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (otherPlayer == null)
                    {
                        SendMessage(cmd.User, null, GetText("Words_PlayerNotFound"));
                        return;
                    }

                    if (GetSettingBool("user_" + otherPlayer.SteamID, "silenced"))
                    {
                        SetSettingBool("user_" + otherPlayer.SteamID, "silenced", false);
                        SendMessage(cmd.User, null, String.Format(GetText("Words_Unsilenced"), otherPlayer.Name));
                    }
                    else
                    {
                        SetSettingBool("user_" + otherPlayer.SteamID, "silenced", true);
                        SendMessage(cmd.User, null, String.Format(GetText("Words_Silenced"), otherPlayer.Name));
                    }
                }
            }
            else if (cmd.cmd == GetCmd("save") && cmd.User.Owner)
            {
                ini.Save();
                iniArena.Save();
                iniLang.Save();
                SendMessage(cmd.User, null, GetText("Words_Saved"));
            }
            else if (cmd.cmd == GetCmd("load") && cmd.User.Owner)
            {
                ini = Plugin.GetIni(pluginIni);
                iniArena = Plugin.GetIni(pluginIni + "Arena");
                iniLang = Plugin.GetIni(pluginIni + "Lang");
                SendMessage(cmd.User, null, GetText("Words_Loaded"));
            }
            else if (cmd.cmd == GetCmd("togglearena") && cmd.User.Owner)
            {
                if (GetSettingBool("Settings", "arenaEnabled"))
                {
                    SetSettingBool("Settings", "arenaEnabled", false);
                    SendMessage(cmd.User, null, GetText("Words_ArenaDisabled"));
                }
                else
                {
                    SetSettingBool("Settings", "arenaEnabled", true);
                    SendMessage(cmd.User, null, GetText("Words_ArenaEnabled"));
                }
            }
            else if (cmd.cmd == GetCmd("logarena") && cmd.User.Owner)
            {
                LogArena(cmd);
            }
            else if (cmd.cmd == GetCmd("respawnarena") && cmd.User.Owner)
            {
                DestroyArena(cmd);
                SpawnArena(cmd);
            }
            else if (cmd.cmd == GetCmd("spawnarena") && cmd.User.Owner)
            {
                SpawnArena(cmd);
            }
            else if (cmd.cmd == GetCmd("destroyarena") && cmd.User.Owner)
            {
                DestroyArena(cmd);
            }
            else if (cmd.cmd == GetCmd("addspawn") && cmd.User.Owner)
            {
                AddSpawn(cmd);
            }
            else if (cmd.cmd == GetCmd("arenahere") && cmd.User.Owner)
            {
                ArenaHere(cmd);
            }
            else if (cmd.cmd == GetCmd("arena"))
            {
                Arena(cmd);
            }
            else if (cmd.cmd == GetCmd("achievements"))
            {
                Player player = cmd.User;
                if (cmd.quotedArgs.Count() == 1)
                {
                    player = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (player == null)
                    {
                        SendMessage(cmd.User, null, GetText("Words_PlayerNotFound"));
                        return;
                    }
                }

                var achievements = Achievements();

                String[] userData = ini.EnumSection("user_" + player.SteamID);

                System.Array.Sort(userData);

                SendMessage(cmd.User, null, String.Format(GetText("Words_AchievementsFor"), player.Name));

                foreach (var item in userData)
                {
                    if (item.StartsWith("achievement_"))
                    {
                        SendMessage(cmd.User, null, "[" + GetText("Achievement") + "] " + achievements[item.Replace("achievement_", "")]);
                    }
                }
            }
            else if (cmd.cmd == GetCmd("stats"))
            {
                Player player = cmd.User;
                if (cmd.quotedArgs.Count() == 1)
                {
                    player = GetPlayerFromPotentialPartialName(cmd.quotedArgs[0]);
                    if (player == null)
                    {
                        SendMessage(cmd.User, null, GetText("Words_PlayerNotFound"));
                        return;
                    }
                }

                SendMessage(cmd.User, null, String.Format(GetText("Words_StatsFor"), player.Name));
                SendMessage(cmd.User, null, GetText("Words_StatsKills") + ": " + GetSetting("user_" + player.SteamID, "kills"));
                SendMessage(cmd.User, null, GetText("Words_ArenaKills") + ": " + GetSetting("user_" + player.SteamID, "killsArena"));
                SendMessage(cmd.User, null, GetText("Words_Headshots") + ": " + GetSetting("user_" + player.SteamID, "headshots"));
                SendMessage(cmd.User, null, GetText("Words_Deaths") + ": " + GetSetting("user_" + player.SteamID, "deaths"));
                SendMessage(cmd.User, null, GetText("Words_ArenaDeaths") + ": " + GetSetting("user_" + player.SteamID, "deathsArena"));
            }
            else if (cmd.cmd == GetCmd("fx") && cmd.User.Owner)
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
                        SendMessage(cmd.User, null, GetText("Words_PlayerNotFound"));
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
