using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Pluton;
using ProtoBuf;
using UnityEngine;

namespace Rustitute
{
    partial class Rustitute
    {
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

                Debug.Log("[CHAT] " + fromName + ": " + message);
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
                        // :(
                        //(collider.gameObject.GetComponentInParent<DeployedItem>()) ||
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
                                baseEntity.Kill(BaseNetworkable.DestroyMode.Gib);
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
                        // :(
                        //(collider.gameObject.GetComponentInParent<DeployedItem>()) ||
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
                                baseEntity.Kill(BaseNetworkable.DestroyMode.Gib);

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
                return "north";
            if (angle < 67.5f)
                return "north east";
            if (angle < 112.5f)
                return "east";
            if (angle < 157.5f)
                return "south east";
            if (angle < 202.5f)
                return "south";
            if (angle < 247.5f)
                return "south west";
            if (angle < 292.5f)
                return "west";
            if (angle < 337.5f)
                return "north west";
            return "north";
        }
    }
}
