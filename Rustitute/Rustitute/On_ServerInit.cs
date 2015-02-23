using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_ServerInit()
        {
            try
            {
                iniArena = Plugin.GetIni(pluginIni + "Arena");

                List<DisappearItem> disappearListInit = new List<DisappearItem>();
                List<string> disappearUniqueInit = new List<string>();
                List<string> disappearItems = new List<string>();

                try
                {
                    var arenaParts = iniArena.EnumSection("Arena_1_Parts");

                    foreach (var arenaPart in arenaParts)
                    {
                        string json = iniArena.GetSetting("Arena_1_Parts", arenaPart);

                        JSON.Object part = JSON.Object.Parse(json);

                        if (part.ContainsKey("disappearItem") && part["disappearItem"].Boolean)
                        {
                            Quaternion rot = new Quaternion(float.Parse(part["rotationX"].Str), float.Parse(part["rotationY"].Str), float.Parse(part["rotationZ"].Str), float.Parse(part["rotationW"].Str));
                            Vector3 pos = new Vector3(float.Parse(part["positionX"].Str), float.Parse(part["positionY"].Str), float.Parse(part["positionZ"].Str));
                            string unique = Unique(part["prefabName"].Str, pos, rot);

                            if (!disappearItems.Contains(unique))
                                disappearItems.Add(unique);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("[ServerInit] 5 " + ex.ToString());
                }

                var buildingBlocks = UnityEngine.Object.FindObjectsOfType<BuildingBlock>();
                foreach (var buildingBlock in buildingBlocks)
                {
                    try
                    {
                        string unique = Unique(buildingBlock.LookupPrefabName(), buildingBlock.transform.position, buildingBlock.transform.rotation);
                        if (disappearItems.Contains(unique))
                        {
                            DisappearItem state = new DisappearItem();

                            state.Block = buildingBlock;
                            state.Prefab = state.Block.LookupPrefabName();
                            state.Location = state.Block.transform.position;
                            state.Rotation = state.Block.transform.rotation;
                            state.Grade = state.Block.grade;

                            disappearListInit.Add(state);
                            disappearUniqueInit.Add(unique);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("[ServerInit] 4 " + ex.ToString());
                    }
                }

                GlobalData["Rustitute_disappearList"] = disappearListInit;
                GlobalData["Rustitute_disappearUnique"] = disappearUniqueInit;

                foreach (var state in disappearListInit)
                {
                    try
                    {
                        BaseEntity baseEntity = state.Block.gameObject.ToBaseEntity();
                        if (baseEntity != null)
                        {
                            if (!baseEntity.isDestroyed)
                            {
                                baseEntity.SendMessage("PreDie", SendMessageOptions.DontRequireReceiver);
                                baseEntity.Kill(BaseNetworkable.DestroyMode.None);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("[ServerInit] 1 " + ex.ToString());
                    }
                    try
                    {

                        BaseEntity ent = GameManager.server.CreateEntity(state.Prefab, state.Location, state.Rotation);
                        ent.SpawnAsMapEntity();

                        state.Block = (BuildingBlock) ent;
                        state.Block.grade = state.Grade;
                        try
                        {
                            state.Block.Heal(100000f);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("[ServerInit] 2 " + ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("[ServerInit] 3 " + ex.ToString());
            }
        }
    }
}
