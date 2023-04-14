using System;
using System.Collections.Generic;
using Achievements;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public string DataVersion;
        public int IndexScene = 1;
        public int Highscore;
        public int Money = -1;
        public int SpeedParameter;
        public int SizeParameter;
        public int IncomeParameter;
        public int BoostLevel;
        public int DistanceTraveledOverAllTime;
        public int CountCollisionObstacles;
        public string DailyDate;
        public int CountDailyEntry = 1;
        public int Playtime;
        public float ElapsedTime;
        public bool IsMuted;
        public bool IsNormalQuality = true;
        public int SpeedAchievement;
        public int SizeAchievement;
        public int IncomeAchievement;
        public int CountLaunch;
        public int SpentMoney;
        public int CountAchievement;
        public List<AchievementData> AchievementsData;

        public GameData(string dataVersion)
        {
            DataVersion = dataVersion;
        }
    }
}