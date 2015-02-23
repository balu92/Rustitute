using System;
using Pluton;
using Pluton.Events;
using Rust;
using UnityEngine;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_PlayerDied(PlayerDeathEvent de)
        {
            try
            {
                Player attacker = null;
                try
                {
                    attacker = de.Attacker.ToPlayer();
                }
                catch (Exception ex) { }

                string sleeping = "";

                try
                {
                    SetSetting("user_" + de.Victim.SteamID, "lastDeath", Epoch().ToString());

                    if (attacker != null && de.Victim.basePlayer.IsSleeping())
                    {
                        sleeping = " while they were sleeping";

                        SetSetting("user_" + de.Attacker.ToPlayer().SteamID, "sleeperKills", ((GetSettingInt("user_" + de.Attacker.ToPlayer().SteamID, "sleeperKills")) + 1).ToString());
                        int totalSleeperKills = GetSettingInt("user_" + de.Attacker.ToPlayer().SteamID, "sleeperKills");

                        if (totalSleeperKills == 10)
                            Achievement("SleeperKills10", de.Victim);
                        if (totalSleeperKills == 100)
                            Achievement("SleeperKills100", de.Victim);
                        if (totalSleeperKills == 1000)
                            Achievement("SleeperKills1000", de.Victim);
                        if (totalSleeperKills == 10000)
                            Achievement("SleeperKills10000", de.Victim);
                    }
                }
                catch (Exception ex) { }

                var arena = "";
                if (attacker != null && GetSettingBool("user_" + de.Attacker.ToPlayer().SteamID, "inArena"))
                    arena = "[ARENA] ";

                try
                {
                    if (arena.Length > 0)
                        SetSetting("user_" + de.Victim.SteamID, "deathsArena", ((GetSettingInt("user_" + de.Victim.SteamID, "deathsArena")) + 1).ToString());
                    else
                        SetSetting("user_" + de.Victim.SteamID, "deaths", ((GetSettingInt("user_" + de.Victim.SteamID, "deaths")) + 1).ToString());

                    int totalDeaths = (GetSettingInt("user_" + de.Victim.SteamID, "deaths") + GetSettingInt("user_" + de.Victim.SteamID, "deathsArena"));

                    if (totalDeaths == 10)
                        Achievement("Deaths10", de.Victim);
                    if (totalDeaths == 100)
                        Achievement("Deaths100", de.Victim);
                    if (totalDeaths == 1000)
                        Achievement("Deaths1000", de.Victim);
                    if (totalDeaths == 10000)
                        Achievement("Deaths10000", de.Victim);
                }
                catch (Exception ex) { }

                if (de.DamageType == DamageType.Bullet || de.DamageType == DamageType.Slash || de.DamageType == DamageType.Blunt || de.DamageType == DamageType.Bleeding)
                {
                    if (attacker != null && de.Victim.SteamID.Length > 0 && de.Attacker.ToPlayer().SteamID.Length > 0)
                    {
                        if (de.DamageType == DamageType.Bleeding)
                            SendMessage(null, null, arena + de.Victim.Name + " bled to death");
                        else
                        {
                            if (arena.Length > 0)
                                SetSetting("user_" + de.Attacker.ToPlayer().SteamID, "killsArena", ((GetSettingInt("user_" + de.Attacker.ToPlayer().SteamID, "killsArena")) + 1).ToString());
                            else
                                SetSetting("user_" + de.Attacker.ToPlayer().SteamID, "kills", ((GetSettingInt("user_" + de.Attacker.ToPlayer().SteamID, "kills")) + 1).ToString());

                            int totalKills = (GetSettingInt("user_" + de.Attacker.ToPlayer().SteamID, "kills") + GetSettingInt("user_" + de.Attacker.ToPlayer().SteamID, "killsArena"));

                            if (totalKills == 10)
                                Achievement("Kills10", de.Attacker.ToPlayer());
                            if (totalKills == 100)
                                Achievement("Kills100", de.Attacker.ToPlayer());
                            if (totalKills == 1000)
                                Achievement("Kills1000", de.Attacker.ToPlayer());
                            if (totalKills == 10000)
                                Achievement("Kills10000", de.Attacker.ToPlayer());

                            if (de.Attacker.ToPlayer().Health <= 5)
                                Achievement("5HealthKill", de.Attacker.ToPlayer());

                            string extraInfo = "";
                            if (de.HitBone == "head" || de.HitBone == "jaw" || de.HitBone == "spine4")
                            {
                                SetSetting("user_" + de.Attacker.ToPlayer().SteamID, "headshots", ((GetSettingInt("user_" + de.Attacker.ToPlayer().SteamID, "headshots")) + 1).ToString());

                                var totalHeadshots = GetSettingInt("user_" + de.Attacker.ToPlayer().SteamID, "headshots");
                                if (totalHeadshots == 10)
                                    Achievement("Headshots10", de.Attacker.ToPlayer());
                                if (totalHeadshots == 100)
                                    Achievement("Headshots100", de.Attacker.ToPlayer());
                                if (totalHeadshots == 1000)
                                    Achievement("Headshots1000", de.Attacker.ToPlayer());
                                if (totalHeadshots == 10000)
                                    Achievement("Headshots10000", de.Attacker.ToPlayer());

                                extraInfo = " - HEADSHOT!";
                            }
                            else if (de.HitBone == "pelvis")
                                extraInfo = " - COCK SHOT!";

                            int shotDistance = Mathf.RoundToInt(Vector3.Distance(de.Attacker.Location, de.Victim.Location));

                            SendMessage(null, null, arena + de.Attacker.Name + " killed " + de.Victim.Name + sleeping + " - " + de.Weapon.Name + " @ " + shotDistance + "m" + extraInfo);

                            if (shotDistance >= 50)
                                Achievement("KillDistance50", de.Attacker.ToPlayer());
                            if (shotDistance >= 100)
                                Achievement("KillDistance100", de.Attacker.ToPlayer());
                            if (shotDistance >= 200)
                                Achievement("KillDistance200", de.Attacker.ToPlayer());
                            if (shotDistance >= 300)
                                Achievement("KillDistance300", de.Attacker.ToPlayer());
                            if (shotDistance >= 400)
                                Achievement("KillDistance400", de.Attacker.ToPlayer());
                            if (shotDistance >= 500)
                                Achievement("KillDistance500", de.Attacker.ToPlayer());
                        }
                    }
                }
                else
                {
                    switch (de.DamageType)
                    {
                        case DamageType.Stab:
                            if (de.Attacker.Name == "items/beartrap")
                                SendMessage(null, null, arena + de.Victim.Name + " was killed by a bear trap");
                            else
                                SendMessage(null, null, arena + de.Victim.Name + " died" + sleeping + ": " + de.DamageType);
                            break;
                        case DamageType.Heat:
                            SendMessage(null, null, arena + de.Victim.Name + " burnt to death");
                            break;
                        case DamageType.Suicide:
                            SendMessage(null, null, arena + de.Victim.Name + " commited suicide");
                            break;
                        case DamageType.Fall:
                            SendMessage(null, null, arena + de.Victim.Name + " fell and died");
                            break;
                        case DamageType.Drowned:
                            SendMessage(null, null, arena + de.Victim.Name + " drowned");
                            break;
                        case DamageType.Generic:
                            SendMessage(null, null, arena + de.Victim.Name + " died" + sleeping);
                            break;
                        default:
                            SendMessage(null, null, arena + de.Victim.Name + " died" + sleeping + ": " + de.DamageType);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                //SendMessageToAdmins("[Exception] On_PlayerDied: " + ex.Message);
            }
        }
    }
}
