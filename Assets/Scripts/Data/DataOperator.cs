using System;
using System.Collections.Generic;
using Core;
using Parameters;
using Services;
using Services.Level;

namespace Data
{
    public class DataOperator : IDisposable
    {
        private GameData _gameData;
        private ISaveSystem _saveSystem;
        private GamePlayService _gamePlayService;
        private LevelService _levelService;
        private LevelScore _levelScore;
        private SoundController _soundController;
        private Wallet _wallet;
        private Dictionary<ParameterType, Parameter> _parameters;

        public DataOperator(GamePlayService gamePlayService, LevelService levelService, SoundController soundController, Wallet wallet, Dictionary<ParameterType, Parameter> parameters)
        {
            _gamePlayService = gamePlayService;
            _gamePlayService.SetDataOperator(this);
            _levelService = levelService;
            _levelScore = _levelService.Score;
            _soundController = soundController;
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
            SaveMoney(_wallet.Money);
            SaveTime(_gamePlayService.ElapsedTime);

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
            LoadMoney();
            LoadParameters();
            LoadHighscore();
            LoadTime();
            LoadMutedState();
        }

        private void SaveIndexScene() => _gameData.IndexScene = _levelService.IndexNextScene;

        private void SaveHighscore(int highscore)
        {
            _gameData.Highscore = highscore;
            Save();
        }

        private void SaveMuted(bool isMuted)
        {
            _gameData.IsMuted = isMuted;
            Save();
        }

        private void SaveMoney(int money) => _gameData.Money = money;

        private void SaveTime(float elapsedTime) => _gameData.ElapsedTime = elapsedTime;

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

            Save();
        }

        private void LoadTime() => _gamePlayService.SetElapsedTime(_gameData.ElapsedTime);

        private void LoadMoney() => _wallet.LoadMoney(_gameData.Money);

        private void LoadHighscore() => _levelScore.LoadHighscore(_gameData.Highscore);

        private void LoadIndexScene() => _levelService.LoadLevel(_gameData.IndexScene);

        private void LoadMutedState() => _soundController.LoadMutedState(_gameData.IsMuted);

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
            _wallet.MoneyChanged += SaveMoney;
            _levelScore.HighscoreChanged += SaveHighscore;
            _soundController.MutedChanged += SaveMuted;

            foreach (var parameter in _parameters)
            {
                parameter.Value.LevelChanged += SaveParameter;
            }
        }

        private void Unsubscribe()
        {
            _wallet.MoneyChanged -= SaveMoney;
            _levelScore.HighscoreChanged -= SaveHighscore;
            _soundController.MutedChanged -= SaveMuted;

            foreach (var parameter in _parameters)
            {
                parameter.Value.LevelChanged -= SaveParameter;
            }
        }
    }
}
