using System;
using System.Linq;
using Pluton;
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
                Player attacker = null;
                try
                {
                    attacker = de.Attacker.ToPlayer();
                }
                catch (Exception ex) { }

                if (de.Victim.SteamID.Length > 0)
                {
                    int arenaCheck = 0;
                    if (attacker != null)
                    {
                        if (GetSettingBool("user_" + de.Victim.SteamID, "inArena")) arenaCheck++;
                        if (GetSettingBool("user_" + attacker.SteamID, "inArena")) arenaCheck++;
                    }

                    if (arenaCheck == 1)
                    {
                        SendMessage(de.Attacker.ToPlayer(), null, GetText("Words_CantHurtPlayerHalfArena"));

                        for (int i = 0; i < de.DamageAmounts.Count(); i++)
                        {
                            de.DamageAmounts[i] = 0;
                        }
                    }
                    else
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

                        if (attacker != null && (attacker.SteamID == de.Victim.SteamID) && (GetSettingBool("user_" + de.Victim.SteamID, "inArena")) && (GetSettingBool("user_" + de.Victim.SteamID, "godArena")))
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
                                if (attacker != null)
                                    SendMessage(de.Attacker.ToPlayer(), null, GetText("Arena_CantHarmThey"));

                                for (int i = 0; i < de.DamageAmounts.Count(); i++)
                                {
                                    de.DamageAmounts[i] = 0;
                                }
                            }

                            if (attacker != null && (GetSettingBool("user_" + attacker.SteamID, "godArena")) && (GetSettingBool("user_" + attacker.SteamID, "inArena")))
                            {
                                if (attacker != null)
                                    SendMessage(de.Attacker.ToPlayer(), null, GetText("Arena_CantHarmYou"));

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
                                if (attacker != null && de.Victim.SteamID != de.Attacker.ToPlayer().SteamID)
                                {
                                    SendMessage(de.Attacker.ToPlayer(), null, GetText("Words_GodHurtThem"));
                                    SendMessage(de.Victim, null, String.Format(GetText("Words_GodHurtYou"), de.Attacker.Name));
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

                        TimeRestrictSet(de.Victim, "attacked");

                        if (attacker != null)
                        {
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
                }
            }
            catch (Exception ex)
            {
                //SendMessageToAdmins("[Exception] On_PlayerHurt: " + ex.Message);
            }
        }
    }
}
