using System.Collections.Generic;
using Core;
using Parameters;
using Services.Level;

namespace Data
{
    public class DataOperator
    {
        private GameData _gameData;
        private ISaveSystem _saveSystem;
        private LevelService _levelService;
        private LevelScore _levelScore;
        private Wallet _wallet;
        private Dictionary<ParameterType, Parameter> _parameters;
        private List<Skin> _openSkins;
        private List<TrailFX> _openTrails;

        public DataOperator(LevelService levelService, Wallet wallet, Dictionary<ParameterType, Parameter> parameters)
        {
            _levelService = levelService;
            _levelScore = _levelService.Score;
            _wallet = wallet;
            _parameters = parameters;
        }

        public void Save()
        {
            _gameData.SetNumberCurrentLeve(_levelService);
            _gameData.SetHighscore(_levelScore);
            _gameData.SetMoney(_wallet);
            _gameData.SetParameters(_parameters);

            if (_saveSystem == null)
                _saveSystem = new PlayerPrefsSystem();

            _saveSystem.Save(_gameData);
        }

        public void Load()
        {
            _gameData = _saveSystem.Load();

            if (_gameData == null)
                return;

            _levelService.LoadLevel(_gameData.GetNumberCurrentLeve());
        }
    }
}
