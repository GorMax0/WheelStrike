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
        public float ElapsedTime;

        //public IEnumerable<Skin> OpenSkins;
        //public IEnumerable<TrailFX> OpenTrails;
    }
}