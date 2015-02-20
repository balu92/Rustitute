using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pluton;
using Pluton.Events;
using UnityEngine;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_CombatEntityHurt(CombatEntityHurtEvent he)
        {
            try
            {
                int copySize = GetSettingInt("user_" + he.Attacker.ToPlayer().SteamID, "copy");
                if (copySize != 0)
                {
                    if (he.Victim.Prefab.StartsWith("build/"))
                    {
                        var part = he.Victim.ToBuildingPart();
                        Vector3 pos = new Vector3(part.Location.x, part.Location.y + copySize, part.Location.z);
                        BaseEntity ent = GameManager.server.CreateEntity(part.Prefab, pos, part.buildingBlock.transform.rotation);
                        ent.SpawnAsMapEntity();
                        ent.GetComponent<BuildingBlock>().Heal(100000f);
                    }
                }
            }
            catch (Exception ex) { }

            try
            {
                float x = float.Parse(GetSetting("Arena", "locationX"));
                float z = float.Parse(GetSetting("Arena", "locationZ"));

                Vector2 arenaLocation = new Vector2(x, z);

                bool arenaPart = false;
                try
                {
                    arenaPart = (he.Victim.ToBuildingPart().Prefab.Length > 0);
                }
                catch (Exception ex) { }

                Player attacker = null;
                try
                {
                    attacker = he.Attacker.ToPlayer();
                }
                catch (Exception ex) { }

                if (arenaPart && attacker != null && GetSettingBool("user_" + he.Attacker.ToPlayer().SteamID, "koall"))
                {
                    if (!GetSettingBool("user_" + he.Attacker.ToPlayer().SteamID, "inArena"))
                    {
                        KOAll(he.Victim.Location);
                    }
                }
                else if (arenaPart && attacker != null && GetSettingBool("user_" + he.Attacker.ToPlayer().SteamID, "ko"))
                {
                    if (!GetSettingBool("user_" + he.Attacker.ToPlayer().SteamID, "inArena"))
                    {
                        he.Victim.ToBuildingPart().Health = 0;
                    }
                }
                else if (arenaPart)
                {
                    if (Vector2.Distance(arenaLocation, new Vector2(he.Victim.Location.x, he.Victim.Location.z)) <= arenaBuildRestrictionSpace)
                    {
                        if (attacker == null || !GetSettingBool("user_" + he.Attacker.ToPlayer().SteamID, "arenabuild"))
                        {
                            for (int i = 0; i < he.DamageAmounts.Count(); i++)
                            {
                                he.DamageAmounts[i] = 0;
                            }

                            he.Victim.ToBuildingPart().Health += 100000f;

                            //he.Victim.ToBuildingPart().buildingBlock.InitializeHealth(100000f, 100000f);

                            if(attacker != null)
                                if (!GetSettingBool("user_" + he.Attacker.ToPlayer().SteamID, "inArena"))
                                    SendMessage(he.Attacker.ToPlayer(), null, "You cannot damage the arena!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //SendMessageToAdmins(ex.ToString());
            }
        }
    }
}
