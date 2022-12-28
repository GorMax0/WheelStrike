using System;
using System.Collections.Generic;
using Core;
using Parameters;
using Services.Level;

namespace Data
{
    public class DataOperator : IDisposable
    {
        private GameData _gameData;
        private ISaveSystem _saveSystem;
        private LevelService _levelService;
        private LevelScore _levelScore;
        private Wallet _wallet;
        private Dictionary<ParameterType, Parameter> _parameters;
        //private List<Skin> _openSkins;
        //private List<TrailFX> _openTrails;

        public DataOperator(LevelService levelService, Wallet wallet, Dictionary<ParameterType, Parameter> parameters)
        {
            _levelService = levelService;
            _levelScore = _levelService.Score;
            _wallet = wallet;
            _parameters = parameters;
            _saveSystem = new PlayerPrefsSystem();

            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        public void Save()
        {
            if (_saveSystem == null)
                throw new NullReferenceException($"{GetType()}: Save(): _saveSystem is null");

            SaveIndexScene();
            SaveHighscore(_levelScore.Highscore);
            SaveMoney(_wallet.Money);

            _saveSystem.Save(_gameData);
        }

        public void Load()
        {
            if (_saveSystem == null)
                throw new NullReferenceException($"{GetType()}: Load(): _saveSystem is null");

            _gameData = _saveSystem.Load();

            if (_gameData == null)
                return;

            LoadIndexScene();
            LoadHighscore();
            LoadMoney();
            LoadParameters();
        }

        private void SaveIndexScene() => _gameData.IndexScene = _levelService.IndexNextScene;

        private void SaveHighscore(int highscore) => _gameData.Highscore = highscore;

        private void SaveMoney(int money) => _gameData.Money = money;

        private void SaveParameter(Parameter parameter)
        {
            switch (parameter.Type)
            {
                case ParameterType.Speed:
                    _gameData.SpeedParameter = parameter.Level;
                    break;
                case ParameterType.Size:
                    _gameData.SizeParameter = parameter.Level;
                    break;
                case ParameterType.Income:
                    _gameData.IncomeParameter = parameter.Level;
                    break;
                default:
                    throw new InvalidOperationException($"{GetType()}: SaveParameter(Parameter parameter): Invalid parameter");
            }
        }

        private void LoadMoney() => _wallet.LoadMoney(_gameData.Money);

        private void LoadHighscore() => _levelScore.LoadHighscore(_gameData.Highscore);

        private void LoadIndexScene() => _levelService.LoadLevel(_gameData.IndexScene);

        private void LoadParameters()
        {
            foreach (KeyValuePair<ParameterType, Parameter> parameter in _parameters)
            {
                if (parameter.Key == ParameterType.Speed)
                {
                    parameter.Value.LoadLevel(_gameData.SpeedParameter);
                    continue;
                }

                if (parameter.Key == ParameterType.Size)
                {
                    parameter.Value.LoadLevel(_gameData.SizeParameter);
                    continue;
                }

                if (parameter.Key == ParameterType.Income)
                {
                    parameter.Value.LoadLevel(_gameData.IncomeParameter);
                    continue;
                }
            }
        }

        private void Subscribe()
        {
            _levelScore.HighscoreChanged += SaveHighscore;
            _wallet.MoneyChanged += SaveMoney;

            foreach (var parameter in _parameters)
            {
                parameter.Value.LevelChanged += SaveParameter;
            }
        }

        private void Unsubscribe()
        {
            _levelScore.HighscoreChanged -= SaveHighscore;
            _wallet.MoneyChanged -= SaveMoney;

            foreach (var parameter in _parameters)
            {
                parameter.Value.LevelChanged -= SaveParameter;
            }
        }
    }
}
