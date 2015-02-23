using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_ServerShutdown()
        {
            if (disappearTimer != null)
                disappearTimer.Close();

            foreach (var state in disappearList)
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
                    //Debug.Log("[DisappearTimer] " + ex.ToString());
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
                    catch (Exception ex) { }
                }
                catch (Exception ex)
                {
                    //Debug.Log("[DisappearTimer] " + ex.ToString());
                }
            }
        }
    }
}
