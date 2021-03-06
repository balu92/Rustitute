﻿using System;
using Pluton.Events;
using UnityEngine;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_Placement(BuildingEvent be)
        {
            if (!GetSettingBool("user_" + be.Builder.SteamID, "arenabuild"))
            {
                if (server.stability == false)
                {
                    //be.Destroy("Sorry, you cannot build on this server. Type /arena to join our arena!");
                    //return;
                }
            }

            float x = float.Parse(GetSetting("Arena", "locationX"));
            float z = float.Parse(GetSetting("Arena", "locationZ"));

            Vector2 arenaLocation = new Vector2(x, z);

            if (Vector2.Distance(arenaLocation, new Vector2(be.Builder.Location.x, be.Builder.Location.z)) <= arenaBuildRestrictionSpace)
            {
                if (!GetSettingBool("user_" + be.Builder.SteamID, "arenabuild"))
                {
                    be.Destroy(GetText("Arena_CantBuildNear"));
                    return;
                }
            }

            if (GetSettingBool("user_" + be.Builder.SteamID, "instamax"))
            {
                InstaMax(be.BuildingPart);
            }
        }
    }
}
