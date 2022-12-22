using System.Collections.Generic;
using Parameters;
using Services.Level;
using Core;

namespace Data
{
    [System.Serializable]
    public class GameData
    {
        public int PassedLevel;
        public int Highscore;
        public int Money;
        public int SpeedParameter;
        public int SizeParameter;
        public int IncomeParameter;
        //public IEnumerable<Skin> OpenSkins;
        //public IEnumerable<TrailFX> OpenTrails;
    }
}