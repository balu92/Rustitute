using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pluton.Events;
using Rust;
using UnityEngine;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_PlayerHurt(PlayerHurtEvent de)
        {
            try
            {
                if (de.Victim.SteamID.Length > 0)
                {
                    if (de.DamageType == DamageType.Stab || de.DamageType == DamageType.Slash)
                    {
                        if (GetSettingBool("user_" + de.Victim.SteamID, "inArena"))
                        {
                            if (!GetSettingBool("user_" + de.Victim.SteamID, "godArena"))
                            {
                                for (int i = 0; i < de.DamageAmounts.Count(); i++)
                                {
                                    de.DamageAmounts[i] = 100;
                                }
                            }
                        }
                    }

                    if ((de.Attacker.ToPlayer().SteamID == de.Victim.SteamID) && (GetSettingBool("user_" + de.Victim.SteamID, "inArena")) && (GetSettingBool("user_" + de.Victim.SteamID, "godArena")))
                    {
                        for (int i = 0; i < de.DamageAmounts.Count(); i++)
                        {
                            de.DamageAmounts[i] = 0;
                        }
                    }
                    else
                    {
                        if ((GetSettingBool("user_" + de.Victim.SteamID, "godArena")) && (GetSettingBool("user_" + de.Victim.SteamID, "inArena")))
                        {
                            SendMessage(de.Attacker.ToPlayer(), null, "You cannot harm players for 10 seconds after they spawn!");
                            for (int i = 0; i < de.DamageAmounts.Count(); i++)
                            {
                                de.DamageAmounts[i] = 0;
                            }
                        }

                        if ((GetSettingBool("user_" + de.Attacker.ToPlayer().SteamID, "godArena")) && (GetSettingBool("user_" + de.Attacker.ToPlayer().SteamID, "inArena")))
                        {
                            SendMessage(de.Attacker.ToPlayer(), null, "You cannot harm players for 10 seconds after you spawn!");
                            for (int i = 0; i < de.DamageAmounts.Count(); i++)
                            {
                                de.DamageAmounts[i] = 0;
                            }
                        }
                    }

                    if (GetSettingBool("user_" + de.Victim.SteamID, "god"))
                    {
                        if (!GetSettingBool("user_" + de.Victim.SteamID, "inArena"))
                        {
                            Heal(de.Victim);
                            if (de.Victim.SteamID != de.Attacker.ToPlayer().SteamID)
                            {
                                SendMessage(de.Attacker.ToPlayer(), null, "This person has god mode and cannot be damaged!");
                                SendMessage(de.Victim, null, "You have god mode and were just attacked by " + de.Attacker.Name);
                            }
                        }
                    }

                    if (GetSettingBool("user_" + de.Victim.SteamID, "inArena"))
                    {
                        if (de.DamageType == DamageType.Fall)
                        {
                            for (int i = 0; i < de.DamageAmounts.Count(); i++)
                            {
                                de.DamageAmounts[i] = 0;
                            }
                        }
                        if (de.DamageType == DamageType.Bleeding)
                        {
                            for (int i = 0; i < de.DamageAmounts.Count(); i++)
                            {
                                de.DamageAmounts[i] = 0;
                            }

                            de.Victim.basePlayer.metabolism.bleeding.Reset();
                        }
                    }

                    float hitDistance = Vector3.Distance(de.Victim.Location, de.Attacker.Location);

                    if (hitDistance >= 100)
                        Achievement("Hit100", de.Attacker.ToPlayer());
                    if (hitDistance >= 200)
                        Achievement("Hit200", de.Attacker.ToPlayer());
                    if (hitDistance >= 300)
                        Achievement("Hit300", de.Attacker.ToPlayer());
                    if (hitDistance >= 500)
                        Achievement("Hit500", de.Attacker.ToPlayer());

                    if (de.Victim.basePlayer.IsRunning())
                    {
                        if (hitDistance >= 50)
                            Achievement("HitRunning50", de.Attacker.ToPlayer());
                        if (hitDistance >= 100)
                            Achievement("HitRunning100", de.Attacker.ToPlayer());
                        if (hitDistance >= 200)
                            Achievement("HitRunning200", de.Attacker.ToPlayer());
                    }

                    if (!de.Victim.basePlayer.IsOnGround())
                    {
                        if (hitDistance >= 50)
                            Achievement("HitAir50", de.Attacker.ToPlayer());
                        if (hitDistance >= 100)
                            Achievement("HitAir100", de.Attacker.ToPlayer());
                        if (hitDistance >= 200)
                            Achievement("HitAir200", de.Attacker.ToPlayer());
                    }

                }
            }
            catch (Exception ex)
            {
                //SendMessageToAdmins("[Exception] On_PlayerHurt: " + ex.Message);
            }
        }
    }
}
