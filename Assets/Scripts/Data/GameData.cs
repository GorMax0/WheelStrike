namespace Data
{
    [System.Serializable]
    public class GameData
    {
        public int IndexScene;
        public int Highscore;
        public int Money = -1;
        public int SpeedParameter;
        public int SizeParameter;
        public int IncomeParameter;
        public int DistanceTraveledOverAllTime;
        public int CountCollisionObstacles;
        public float ElapsedTime;
        public bool IsMuted;
    }
}