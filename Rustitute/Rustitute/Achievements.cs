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

            string[] cheevos =
            {
                "5HealthKill",
                "Kills10",
                "Kills100",
                "Kills1000",
                "Kills10000",
                "Headshots10",
                "Headshots100",
                "Headshots1000",
                "Headshots10000",
                "Deaths10",
                "Deaths100",
                "Deaths1000",
                "Deaths10000",
                "SleeperKills10",
                "SleeperKills100",
                "SleeperKills1000",
                "SleeperKills10000",
                "HitRunning50",
                "HitRunning100",
                "HitRunning200",
                "HitAir50",
                "HitAir100",
                "HitAir200",
                "Hit100",
                "Hit200",
                "Hit300",
                "Hit500",
                "KillDistance50",
                "KillDistance100",
                "KillDistance200",
                "KillDistance300",
                "KillDistance400",
                "KillDistance500",
                "JoinedArena"
            };

            foreach (var cheev in cheevos)
            {
                achievements[cheev] = GetText("Achievement_" + cheev);
            }

            return achievements;
        }

        private void Achievement(String achievement, Player player)
        {
            var achievements = Achievements();

            if (!achievements.ContainsKey(achievement))
                return;

            if (!GetSettingBool("user_" + player.SteamID, "achievement_" + achievement))
            {
                SendMessage(null, null, String.Format(GetText("Achievement_Unlocked"), player.Name, achievements[achievement]));
                SetSettingBool("user_" + player.SteamID, "achievement_" + achievement, true);
            }
        }
    }
}
