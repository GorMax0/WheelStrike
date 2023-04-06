namespace Data
{
    [System.Serializable]
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
        public int CountDailyEntry;
        public int CountLaunch;
        public int Playtime;
        public float ElapsedTime;
        public bool IsMuted;
        public bool IsNormalQuality = true;

        public GameData(string dataVersion)
        {
            DataVersion = dataVersion;
        }
    }
}