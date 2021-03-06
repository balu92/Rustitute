﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Pluton;
using Pluton.Events;
using UnityEngine;

namespace Rustitute
{
    partial class Rustitute
    {
        private void Arena(CommandEvent cmd)
        {
            if (!GetSettingBool("Settings", "arenaEnabled"))
            {
                SendMessage(cmd.User, null, GetText("Arena_NotEnabled"));
                return;
            }

            float ax = float.Parse(GetSetting("Arena", "locationX"));
            float ay = float.Parse(GetSetting("Arena", "locationY"));
            float az = float.Parse(GetSetting("Arena", "locationZ"));

            if (ax == 0 && ay == 0 && az == 0)
            {
                SendMessage(cmd.User, null, GetText("Arena_NotSetup"));
                return;
            }

            if (GetSettingBool("user_" + cmd.User.SteamID, "inArena"))
            {
                SetSettingBool("user_" + cmd.User.SteamID, "inArena", false);
                SetSettingBool("user_" + cmd.User.SteamID, "godArena", false);
                cmd.User.Inventory._inv.Strip();

                float x = float.Parse(GetSetting("user_" + cmd.User.SteamID, "locationBeforeArenaX"));
                float y = float.Parse(GetSetting("user_" + cmd.User.SteamID, "locationBeforeArenaY"));
                float z = float.Parse(GetSetting("user_" + cmd.User.SteamID, "locationBeforeArenaZ"));

                cmd.User.Teleport(x, y, z);

                SendMessage(null, null, String.Format(GetText("Arena_UserLeft"), cmd.User.Name));
            }
            else
            {
                if (GetSettingBool("user_" + cmd.User.SteamID, "god"))
                {
                    SetSettingBool("user_" + cmd.User.SteamID, "god", false);
                    cmd.User.basePlayer.InitializeHealth(100, 100);
                    SendMessage(cmd.User, null, GetText("Words_GodDisabled"));
                }

                Achievement("JoinedArena", cmd.User);

                SetSetting("user_" + cmd.User.SteamID, "locationBeforeArenaX", cmd.User.Location.x.ToString());
                SetSetting("user_" + cmd.User.SteamID, "locationBeforeArenaY", cmd.User.Location.y.ToString());
                SetSetting("user_" + cmd.User.SteamID, "locationBeforeArenaZ", cmd.User.Location.z.ToString());

                SetSetting("user_" + cmd.User.SteamID, "campingCounter", "0");

                var belt = InvItem.ContainerPreference.Belt;
                var main = InvItem.ContainerPreference.Main;

                cmd.User.Inventory._inv.Strip();

                var loadout = Server.LoadOuts["arena"];

                for (int i = 0; i < loadout.items.Count; i++)
                {
                    if (i <= 5)
                        cmd.User.Inventory._inv.GiveItem(loadout.items[i].invItem._item, cmd.User.Inventory.InnerBelt);
                    else
                        cmd.User.Inventory._inv.GiveItem(loadout.items[i].invItem._item, cmd.User.Inventory.InnerMain);
                }

                //loadout.ToInv(cmd.User.Inventory, false);

                if (GetSettingBool("user_" + cmd.User.SteamID, "arenaClothes") == false)
                {
                    List<string> possibleHelmets = new List<string>();
                    //possibleHelmets.Add("bucket_helmet");
                    //possibleHelmets.Add("coffeecan_helmet");
                    //possibleHelmets.Add("metal_facemask");
                    //possibleHelmets.Add("hazmat_helmet");
                    possibleHelmets.Add("none");

                    List<string> possibleShirts = new List<string>();
                    possibleShirts.Add("burlap_shirt");
                    //possibleShirts.Add("hazmat_jacket");
                    possibleShirts.Add("jacket_snow");
                    possibleShirts.Add("jacket_snow2");
                    possibleShirts.Add("jacket_snow3");
                    //possibleShirts.Add("metal_plate_torso");
                    possibleShirts.Add("urban_jacket");
                    possibleShirts.Add("urban_shirt");
                    possibleShirts.Add("vagabond_jacket");
                    possibleShirts.Add("none");

                    List<string> possibleGloves = new List<string>();
                    possibleGloves.Add("burlap_gloves");
                    //possibleGloves.Add("hazmat_gloves");
                    possibleGloves.Add("none");

                    List<string> possiblePants = new List<string>();
                    possiblePants.Add("burlap_trousers");
                    //possiblePants.Add("hazmat_pants");
                    possiblePants.Add("urban_pants");

                    // Always have at least 1 item of clothing so no 'none' on pants.
                    //possiblePants.Add("none");

                    List<string> possibleBoots = new List<string>();
                    possibleBoots.Add("burlap_shoes");
                    //possibleBoots.Add("hazmat_boots");
                    possibleBoots.Add("urban_boots");
                    possibleBoots.Add("none");

                    SetSetting("user_" + cmd.User.SteamID, "arenaClothes_helmet", possibleHelmets.PickRandom());
                    SetSetting("user_" + cmd.User.SteamID, "arenaClothes_shirt", possibleShirts.PickRandom());
                    SetSetting("user_" + cmd.User.SteamID, "arenaClothes_gloves", possibleGloves.PickRandom());
                    SetSetting("user_" + cmd.User.SteamID, "arenaClothes_pants", possiblePants.PickRandom());
                    SetSetting("user_" + cmd.User.SteamID, "arenaClothes_shoes", possibleBoots.PickRandom());
                    
                    SetSettingBool("user_" + cmd.User.SteamID, "arenaClothes", true);
                }

                SetSettingBool("user_" + cmd.User.SteamID, "inArena", true);

                SendToArena(cmd.User);

                SendMessage(null, null, string.Format(GetText("Arena_UserJoined"), cmd.User.Name));
                SendMessage(cmd.User, null, GetText("Arena_JoinMessage"));
            }
        }

        private void LogArena(CommandEvent cmd)
        {
            SavingArena = true;

            SendMessageToAdmins(GetText("Arena_LoggingArena"));

            Dictionary<string, int> list = new Dictionary<string, int>();

            var arenaParts = iniArena.EnumSection("Arena_1_Parts");

            foreach (var arenaPart in arenaParts)
            {
                iniArena.DeleteSetting("Arena_1_Parts", arenaPart);
            }

            float x = float.Parse(GetSetting("Arena", "locationX"));
            float y = float.Parse(GetSetting("Arena", "locationY"));
            float z = float.Parse(GetSetting("Arena", "locationZ"));

            Vector3 arenaLocation = new Vector3(x, y, z);

            int totalHits = 0;

            int cubed = 100;

            for (float xx = -cubed; xx <= cubed; xx += 3f)
            {
                //SendMessageToAdmins(xx.ToString());

                for (float zz = -cubed; zz <= cubed; zz += 3f)
                {
                    for (float yy = -cubed; yy <= cubed; yy += 3f)
                    {
                        Vector3 origin = new Vector3(x + xx, y + yy, z + zz);
                        Collider[] castHits = Physics.OverlapSphere(origin, 5f);

                        if (castHits.Count() >= 128)
                        {
                            SendMessageToAdmins("WARNING: Hits >= 128! Some items may not have saved below position " + origin);
                        }

                        totalHits += castHits.Count();

                        foreach (Collider collider in castHits)
                        {
                            //Collider collider = castHit.collider;

                            try
                            {
                                if ((collider.gameObject.GetComponentsInParent<BuildingBlock>().Any()) ||
                                    (collider.gameObject.GetComponentsInParent<Deployable>().Any()) ||
                                    (collider.gameObject.GetComponentsInParent<StorageContainer>().Any()) ||
                                    (collider.gameObject.GetComponentsInParent<FakePhysics>().Any()) ||
                                    (collider.gameObject.GetComponentsInParent<WorldItem_EnableDisable>().Any()) ||
                                    (collider.name.StartsWith("items/"))
                                    )
                                {
                                    //Debug.Log(collider.gameObject.name);
                                    //BuildingBlock buildingBlock = collider.gameObject.GetComponentInParent<BuildingBlock>();

                                    JSON.Object arena = ArenaPart(collider);
                                    if (arena.Any())
                                    {
                                        string unique = Unique(arena["prefabName"].Str, collider.transform.position, collider.transform.rotation);

                                        if (!iniArena.ContainsSetting("Arena_1_Parts", unique))
                                        {
                                            iniArena.AddSetting("Arena_1_Parts", unique, arena.ToString());
                                            if (list.ContainsKey(arena["name"].Str))
                                                list[arena["name"].Str]++;
                                            else
                                                list.Add(arena["name"].Str, 1);
                                        }
                                        else
                                            iniArena.SetSetting("Arena_1_Parts", unique, arena.ToString());
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.Log("Log Arena Exception: " + ex.ToString());
                            }
                        }
                    }
                }
            }

            foreach (var item in disappearList)
            {
                JSON.Object arena = null;

                try
                {
                    arena = ArenaPart(null, item);
                }
                catch (Exception ex)
                {
                    //SendMessageToAdmins("logarena disappearlist exception: " + ex.ToString());
                }

                if (arena.Any())
                {
                    string unique = Unique(item.Prefab, item.Location, item.Rotation);

                    if (!iniArena.ContainsSetting("Arena_1_Parts", unique))
                    {
                        iniArena.AddSetting("Arena_1_Parts", unique, arena.ToString());
                        if (list.ContainsKey(arena["name"].Str))
                            list[arena["name"].Str]++;
                        else
                            list.Add(arena["name"].Str, 1);
                    }
                    else
                        iniArena.SetSetting("Arena_1_Parts", unique, arena.ToString());
                }
            }

            foreach (var item in list)
            {
                //SendMessageToAdmins(item.Value + "x " + item.Key);
                Debug.Log(item.Value + "x " + item.Key);
            }

            SendMessageToAdmins(GetText("Arena_SavingToFile"));
            iniArena.Save();
            iniArena.SaveSettings(Plugin.ValidateRelativePath("arenas/" + "Arena-" + DateTime.Now.Ticks + ".ini"));

            SendMessageToAdmins(GetText("Arena_LoggedArena"));

            SavingArena = false;
        }

        private string Unique(string prefab, Vector3 location, Quaternion rotation)
        {
            return md5(prefab + "--" + location + "--" + rotation);
        }

        private void SpawnArena(CommandEvent cmd = null)
        {
            var arenaParts = iniArena.EnumSection("Arena_1_Parts");

            foreach (var arenaPart in arenaParts)
            {
                string json = iniArena.GetSetting("Arena_1_Parts", arenaPart);
                JSON.Object part = JSON.Object.Parse(json);

                Quaternion rot = new Quaternion(float.Parse(part["rotationX"].Str), float.Parse(part["rotationY"].Str), float.Parse(part["rotationZ"].Str), float.Parse(part["rotationW"].Str));
                Vector3 pos = new Vector3(float.Parse(part["positionX"].Str), float.Parse(part["positionY"].Str), float.Parse(part["positionZ"].Str));

                if (part["type"].Str == "BuildingBlock")
                {
                    try
                    {
                        BaseEntity ent = GameManager.server.CreateEntity(part["prefabName"].Str, pos, rot);
                        ent.SpawnAsMapEntity();

                        var block = (BuildingBlock)ent;

                        if (cmd != null && cmd.quotedArgs.Any())
                        {
                            int newGrade = Convert.ToInt32(cmd.quotedArgs[0]);
                            int grade = ((int) block.blockDefinition.grades.Length) - 1;

                            if (newGrade <= grade && grade >= 0)
                                block.grade = newGrade;
                            else
                                block.grade = grade;
                        }
                        else
                        {
                            block.grade = Int32.Parse(part["grade"].Number.ToString());
                        }

                        try
                        {
                            block.Heal(100000f);
                        }
                        catch (Exception ex) { }

                        if (part["disappearItem"].Boolean)
                        {
                            string unique = Unique(part["prefabName"].Str, block.transform.position, block.transform.rotation);

                            DisappearItem state = new DisappearItem();

                            state.Block = block;
                            state.Prefab = state.Block.LookupPrefabName();
                            state.Location = state.Block.transform.position;
                            state.Rotation = state.Block.transform.rotation;
                            state.Grade = state.Block.grade;

                            disappearBlocks.Add(block.gameObject);
                            disappearList.Add(state);
                            disappearUnique.Add(unique);
                            block.Kill(BaseNetworkable.DestroyMode.None);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Debug.Log("d1: " + ex.ToString());
                    }
                }
                else if ((part["name"].Str == "items/lantern_deployed") || (part["name"].Str == "Lantern (world)"))
                {
                    try
                    {
                        BaseEntity ent = GameManager.server.CreateEntity("items/lantern_deployed", pos, rot);

                        var deployedItem = ent.GetComponent<Deployable>();
                        var newItem = ItemManager.CreateByName("lantern");

                        deployedItem.SendMessage("SetDeployedBy", cmd.User.basePlayer, UnityEngine.SendMessageOptions.DontRequireReceiver);
                        deployedItem.SendMessage("InitializeItem", newItem, UnityEngine.SendMessageOptions.DontRequireReceiver);

                        ent.Spawn(true);
                        newItem.SetWorldEntity(ent);

                        lanternList.Add(deployedItem);
                    }
                    catch (Exception ex)
                    {
                        //Debug.Log("[spawnarena] " + ex.ToString());
                    }
                }
            }

            if (cmd != null)
                SendMessage(cmd.User, null, GetText("Arena_SpawnedArena"));
        }

        private void DestroyArena(CommandEvent cmd = null)
        {
            lanternList.Clear();
            disappearBlocks.Clear();
            disappearList.Clear();
            disappearUnique.Clear();

            float x = float.Parse(GetSetting("Arena", "locationX"));
            float y = float.Parse(GetSetting("Arena", "locationY"));
            float z = float.Parse(GetSetting("Arena", "locationZ"));

            Vector3 arenaLocation = new Vector3(x, y, z);

            int totalHits = 0;

            for (float xx = -100; xx <= 100; xx += 3f)
            {
                //SendMessageToAdmins(xx.ToString());

                for (float zz = -100; zz <= 100; zz += 3f)
                {
                    for (float yy = -100; yy <= 100; yy += 3f)
                    {
                        Vector3 origin = new Vector3(x + xx, y + yy, z + zz);
                        DestroyEverything(origin, 6f);
                        /*
                        Vector3 origin = new Vector3(x + xx, y, z + zz);
                        Collider[] castHits = Physics.OverlapSphere(origin, 6f);

                        if (castHits.Count() >= 128)
                        {
                            SendMessageToAdmins("WARNING: Hits >= 128! Some items may not have been destroyed below position " + origin);
                        }

                        totalHits += castHits.Count();

                        foreach (Collider collider in castHits)
                        {
                            //Collider collider = castHit.collider;

                            try
                            {
                                BuildingBlock buildingBlock = collider.gameObject.GetComponentInParent<BuildingBlock>();
                                buildingBlock.ChangeHealth(-100000f);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        */
                    }
                }
            }

            if(cmd != null)
                SendMessage(cmd.User, null, GetText("Arena_DestroyedArena"));
        }

        private void AddSpawn(CommandEvent cmd)
        {
            var location = cmd.User.Location.x + " " + cmd.User.Location.y + " " + cmd.User.Location.z;
            SetSetting("ArenaSpawn", location, location);
            SendMessage(cmd.User, null, GetText("Arena_AddSpawn"));
        }

        private void ArenaHere(CommandEvent cmd)
        {
            SetSetting("Arena", "locationX", cmd.User.Location.x.ToString());
            SetSetting("Arena", "locationY", cmd.User.Location.y.ToString());
            SetSetting("Arena", "locationZ", cmd.User.Location.z.ToString());

            SendMessage(cmd.User, null, GetText("Arena_SetCenter"));
        }

        private class ArenaTimer
        {
            public string SteamID { get; set; }
            public int Counter { get; set; }
            public System.Threading.Timer Timer { get; set; }
        }

        private void CreateArenaSpawnTimer(string SteamID, int Interval, int Counter)
        {
            ArenaTimer state = new ArenaTimer();
            state.SteamID = SteamID;
            state.Counter = Counter;
            state.Timer = null;

            state.Timer = new System.Threading.Timer(ArenaSpawnTimer, state, Interval, Timeout.Infinite);
        }

        private void ArenaSpawnTimer(object stateInfo)
        {
            ArenaTimer arenaState = (ArenaTimer)stateInfo;
            arenaState.Timer.Dispose();

            Player player = Player.FindBySteamID(arenaState.SteamID);

            var belt = player.Inventory._inv.containerBelt;
            var wear = player.Inventory._inv.containerWear;
            var main = player.Inventory._inv.containerMain;

            if (!GetSettingBool("user_" + arenaState.SteamID, "inArena"))
            {
                SetSettingBool("user_" + arenaState.SteamID, "godArena", false);
                return;
            }

            if (arenaState.Counter == 1)
                SetSettingBool("user_" + player.SteamID, "godArena", true);

            if (arenaState.Counter <= 10)
            {
                var plural = ((11 - arenaState.Counter) == 1 ? "" : GetText("Words_plural"));
                SendMessage(player, null, "[" + GetText("Arena_ARENA") + "] " + String.Format(GetText("Arena_InvincibleFor"), (11 - arenaState.Counter), plural));
                CreateArenaSpawnTimer(arenaState.SteamID, 1000, ++arenaState.Counter);
            }
            else if (arenaState.Counter == 11)
            {
                SetSettingBool("user_" + arenaState.SteamID, "godArena", false);
                SendMessage(player, null, "[" + GetText("Arena_ARENA") + "] " + GetText("Arena_NoLongerInvincible"));

                var arenaClothes_helmet = GetSetting("user_" + player.SteamID, "arenaClothes_helmet");
                var arenaClothes_shirt = GetSetting("user_" + player.SteamID, "arenaClothes_shirt");
                var arenaClothes_gloves = GetSetting("user_" + player.SteamID, "arenaClothes_gloves");
                var arenaClothes_pants = GetSetting("user_" + player.SteamID, "arenaClothes_pants");
                var arenaClothes_shoes = GetSetting("user_" + player.SteamID, "arenaClothes_shoes");

                if (arenaClothes_helmet != "none")
                    player.Inventory.Add(new InvItem(arenaClothes_helmet, 1), wear);

                if (arenaClothes_shirt != "none")
                    player.Inventory.Add(new InvItem(arenaClothes_shirt, 1), wear);

                if (arenaClothes_gloves != "none")
                    player.Inventory.Add(new InvItem(arenaClothes_gloves, 1), wear);

                if (arenaClothes_pants != "none")
                    player.Inventory.Add(new InvItem(arenaClothes_pants, 1), wear);

                if (arenaClothes_shoes != "none")
                    player.Inventory.Add(new InvItem(arenaClothes_shoes, 1), wear);
            }
        }

        private void item_OnDirty(Item item)
        {
            //CheckLanterns();
            //item.SwitchOnOff(true, new BasePlayer());
        }

        private void CheckLanterns()
        {
            try
            {
                bool lightsOn = (World.Time >= 17.5 || World.Time <= 5.5f);

                foreach (var lantern in lanternList)
                {
                    var baseOven = lantern.gameObject.GetComponent<BaseOven>();
                    foreach (var item in baseOven.inventory.itemList)
                    {
                        item.amount = 1000;
                    }
                    baseOven.isLootable = false;

                    var baseEntity = lantern.gameObject.GetComponent<BaseEntity>();
                    var entityFlagToggle = lantern.gameObject.GetComponent<EntityFlag_Toggle>();
                    baseEntity.SetFlag(BaseEntity.Flags.On, lightsOn);
                    entityFlagToggle.DoUpdate(baseEntity);
                }
            }
            catch (Exception ex)
            {
                //Debug.Log(ex.ToString());
            }
        }

        private void SendToArena(Player player)
        {
            player.Teleport(GetRandomArenaSpawn());
            Heal(player);
            CreateArenaSpawnTimer(player.SteamID, 1000, 1);
        }

        private List<Deployable> GetArenaLanterns()
        {
            List<Deployable> lanterns = new List<Deployable>();

            float x = float.Parse(GetSetting("Arena", "locationX"));
            float y = float.Parse(GetSetting("Arena", "locationY"));
            float z = float.Parse(GetSetting("Arena", "locationZ"));

            var location = new Vector3(x, y, z);

            Collider[] castHits = Physics.OverlapSphere(location, 300f, 1 << 8);

            if (castHits.Count() >= 128)
            {
                SendMessageToAdmins("WARNING: Hits >= 128! Some lanterns may not have been discovered at position " + location);
            }

            foreach (Collider collider in castHits)
            {
                try
                {
                    if ((collider.name == "items/lantern_deployed") || (collider.name == "Lantern (world)"))
                    {
                        lanterns.Add(collider.GetComponent<Deployable>());
                    }
                }
                catch (Exception ex) { }
            }

            return lanterns;
        }

        private IDictionary<string, string> PlayersInArena()
        {
            IDictionary<string, string> list = new Dictionary<string, string>();
            ;
            for (var i = 0; i < Server.ActivePlayers.Count; i++)
            {
                if (GetSettingBool("user_" + Server.ActivePlayers[i].SteamID, "inArena"))
                    list[Server.ActivePlayers[i].SteamID] = Server.ActivePlayers[i].Name;
            }
            return list;
        }

        private Vector3 GetRandomArenaSpawn()
        {
            String[] spawnLocations = ini.EnumSection("ArenaSpawn");

            int index = new System.Random().Next(spawnLocations.Count());
            string spawnLocation = spawnLocations[index];
            var spawn = spawnLocation.Split(' ');

            return new Vector3(float.Parse(spawn[0]), float.Parse(spawn[1]), float.Parse(spawn[2]));
        }

        private bool IsInArena(Vector3 location)
        {
            float x = float.Parse(GetSetting("Arena", "locationX"));
            //float y = float.Parse(GetSetting("Arena", "locationY"));
            float z = float.Parse(GetSetting("Arena", "locationZ"));

            Vector2 arenaLocation = new Vector2(x, z);

            return (Vector2.Distance(arenaLocation, new Vector2(location.x, location.z)) <= arenaBuildRestrictionSpace);
        }

        private JSON.Object ArenaPart(Collider collider = null, DisappearItem  disappearItem = null)
        {
            JSON.Object arena = new JSON.Object();

            Vector3 location = new Vector3();
            Quaternion rotation = new Quaternion();

            if (collider == null)
            {
                arena.Add("type", "BuildingBlock");
                arena.Add("grade", disappearItem.Grade);
                arena.Add("name", disappearItem.Prefab);
                arena.Add("prefabName", disappearItem.Prefab);
                location = disappearItem.Location;
                rotation = disappearItem.Rotation;
            }
            else if (collider.GetComponentInParent<BuildingBlock>())
            {
                BuildingBlock item = collider.GetComponentInParent<BuildingBlock>();
                arena.Add("type", "BuildingBlock");
                arena.Add("grade", item.grade);
                arena.Add("name", item.name);
                arena.Add("prefabName", item.LookupPrefabName());
                location = item.transform.position;
                rotation = item.transform.rotation;
            }
            else if (collider.gameObject.GetComponentsInParent<Deployable>().Any())
            {
                Deployable item = collider.GetComponentInParent<Deployable>();
                arena.Add("type", "Deployable");
                arena.Add("name", collider.name);
                arena.Add("prefabName", collider.name);
                location = item.transform.position;
                rotation = item.transform.rotation;
            }

            arena.Add("positionX", location.x.ToString("F99").TrimEnd('0'));
            arena.Add("positionY", location.y.ToString("F99").TrimEnd('0'));
            arena.Add("positionZ", location.z.ToString("F99").TrimEnd('0'));

            arena.Add("rotationX", rotation.x.ToString("F99").TrimEnd('0'));
            arena.Add("rotationY", rotation.y.ToString("F99").TrimEnd('0'));
            arena.Add("rotationZ", rotation.z.ToString("F99").TrimEnd('0'));
            arena.Add("rotationW", rotation.w.ToString("F99").TrimEnd('0'));

            string unique;
            if(collider == null)
                unique = Unique(arena["prefabName"].Str, disappearItem.Location, disappearItem.Rotation);
            else
                unique = Unique(arena["prefabName"].Str, collider.transform.position, collider.transform.rotation);

            if (disappearUnique.Contains(unique))
                arena.Add("disappearItem", true);

            return arena;
        }
    }
}
