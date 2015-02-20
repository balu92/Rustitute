using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Pluton;
using ProtoBuf;
using UnityEngine;

namespace Rustitute
{
    partial class Rustitute
    {
        private void SendMessage(Player to, Player from, string message)
        {
            ulong userID = 0;
            string fromName = botName;
            string arg2 = "#5af"; // blue
            if (from == null)
            {
                arg2 = "#af5"; // green
            }
            else if (DeveloperList.IsDeveloper(from.basePlayer))
            {
                userID = from.basePlayer.userID;
                fromName = from.basePlayer.displayName;
                arg2 = "#fa5"; // orange
            }
            else if (from.basePlayer.IsAdmin())
            {
                userID = from.basePlayer.userID;
                fromName = from.basePlayer.displayName;
                arg2 = "#af5"; // green
            }
            else
            {
                userID = from.basePlayer.userID;
                fromName = from.basePlayer.displayName;
            }

            string text2 = string.Format("<color={2}>{0}</color>  {1}", fromName.Replace('<', ' ').Replace('>', ' '), message.Replace('<', ' ').Replace('>', ' '), arg2);

            if (to == null)
            {
                ConsoleSystem.Broadcast("chat.add", userID, text2);
                Debug.Log("[CHAT] " + fromName + ": " + message);
            }
            else
                to.basePlayer.SendConsoleCommand("chat.add", userID, text2);
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
            MaxGrade(part.buildingBlock);
            part.buildingBlock.startHealth = 100000f;
            part.buildingBlock.Heal(100000f);
        }

        private void MaxGrade(BuildingBlock block)
        {
            int grade = ((int)block.blockDefinition.grades.Length) - 1;

            if (grade == block.grade)
                return;

            block.grade = grade;
            block.baseProtection = block.blockDefinition.grades[block.grade].damageProtecton;
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

        private void DestroyLanterns(Vector3 location, float radius)
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
                    if ((collider.name == "items/lantern_deployed") || (collider.name == "Lantern (world)"))
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
