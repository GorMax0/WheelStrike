using System.Collections.Generic;
using Parameters;
using Services.Level;
using Core;

namespace Data
{
    [System.Serializable]
    public class GameData
    {
        private int _numberCurrentLevel;
        private int _highscore;
        private int _money;
        private Dictionary<ParameterType, Parameter> _parameters;
        private IEnumerable<Skin> _openSkins;
        private IEnumerable<TrailFX> _openTrails;

        public void SetNumberCurrentLeve(LevelService levelService) => _numberCurrentLevel = levelService.NumberCurrentLevel;
        public void SetHighscore(LevelScore levelScore) => _highscore = levelScore.Highscore;
        public void SetMoney(Wallet wallet) => _money = wallet.Money;
        public void SetParameters(Dictionary<ParameterType, Parameter> parameters) => _parameters = parameters;
        public void SetOpenSkins(List<Skin> openSkins) => _openSkins = openSkins;
        public void SetOpenTrails(List<TrailFX> openTrails) => _openTrails = openTrails;
        public int GetNumberCurrentLeve() => _numberCurrentLevel;
        public int GetHighscore() => _highscore;
        public int GetMoney() => _money;
        public Dictionary<ParameterType, Parameter> GetParameters() => _parameters;
        public IEnumerable<Skin> GetOpenSkins() => _openSkins;
        public IEnumerable<TrailFX> GetOpenTrails() => _openTrails;
    }
}