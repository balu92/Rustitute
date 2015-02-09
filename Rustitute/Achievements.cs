using System;
using System.Collections.Generic;
using Pluton;

namespace Rustitute
{
    partial class Rustitute
    {
        private IDictionary<string, string> Achievements()
        {
            IDictionary<string, string> achievements = new Dictionary<string, string>();

            achievements["5HealthKill"] = "Get a kill while you have 5 or less health";

            achievements["Kills10"] = "Get 10 Kills";
            achievements["Kills100"] = "Get 100 Kills";
            achievements["Kills1000"] = "Get 1,000 Kills";
            achievements["Kills10000"] = "Get 10,000 Kills";

            achievements["Headshots10"] = "Get 10 Headshots";
            achievements["Headshots100"] = "Get 100 Headshots";
            achievements["Headshots1000"] = "Get 1,000 Headshots";
            achievements["Headshots10000"] = "Get 10,000 Headshots";

            achievements["Deaths10"] = "Died 10 Times";
            achievements["Deaths100"] = "Died 100 Times";
            achievements["Deaths1000"] = "Died 1,000 Times";
            achievements["Deaths10000"] = "Died 10,000 Times";

            achievements["SleeperKills10"] = "Killed 10 sleepers";
            achievements["SleeperKills100"] = "Killed 100 sleepers";
            achievements["SleeperKills1000"] = "Killed 1,000 sleepers";
            achievements["SleeperKills10000"] = "Killed 10,000 sleepers";

            achievements["HitRunning50"] = "Hit someone over 50m away while they are running";
            achievements["HitRunning100"] = "Hit someone over 100m away while they are running";
            achievements["HitRunning200"] = "Hit someone over 200m away while they are running";

            achievements["HitAir50"] = "Hit someone over 50m away while they are in the air";
            achievements["HitAir100"] = "Hit someone over 100m away while they are in the air";
            achievements["HitAir200"] = "Hit someone over 200m away while they are in the air";

            achievements["Hit100"] = "Hit someone over 100m away";
            achievements["Hit200"] = "Hit someone over 200m away";
            achievements["Hit300"] = "Hit someone over 300m away";
            achievements["Hit500"] = "Hit someone over 500m away";

            achievements["KillDistance50"] = "Killed someone over 50m away";
            achievements["KillDistance100"] = "Killed someone over 100m away";
            achievements["KillDistance200"] = "Killed someone over 200m away";
            achievements["KillDistance300"] = "Killed someone over 300m away";
            achievements["KillDistance400"] = "Killed someone over 400m away";
            achievements["KillDistance500"] = "Killed someone over 500m away";

            achievements["JoinedArena"] = "Joined the Arena";

            return achievements;
        }

        private void Achievement(String achievement, Player player)
        {
            var achievements = Achievements();

            if (!achievements.ContainsKey(achievement))
                return;

            if (!GetSettingBool("user_" + player.SteamID, "achievement_" + achievement))
            {
                SendMessage(null, null, "[Achievement Unlocked] Congrats " + player.Name + ": " + achievements[achievement]);
                SetSettingBool("user_" + player.SteamID, "achievement_" + achievement, true);
            }
        }
    }
}
