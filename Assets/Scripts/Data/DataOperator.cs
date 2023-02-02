using System;
using System.Collections.Generic;
using Core;
using Parameters;
using Services;
using Services.Level;
using Authorization;
using Agava.YandexGames;
using UnityEngine;

namespace Data
{
    public class DataOperator : IDisposable
    {
        private const int DefaultScene = 1;

        private GameData _gameData;
        private ISaveSystem _saveSystem;
        private GamePlayService _gamePlayService;
        private LevelService _levelService;
        private LevelScore _levelScore;
        private SoundController _soundController;
        private Wallet _wallet;
        private Dictionary<ParameterType, Parameter> _parameters;
        private YandexAuthorization _yandexAuthorization;

        private int _countSave;

        public DataOperator(GamePlayService gamePlayService, LevelService levelService, SoundController soundController,
            Wallet wallet, Dictionary<ParameterType, Parameter> parameters, YandexAuthorization yandexAuthorization)
        {
            _gamePlayService = gamePlayService;
            _gamePlayService.SetDataOperator(this);
            _levelService = levelService;
            _levelScore = _levelService.Score;
            _soundController = soundController;
            _wallet = wallet;
            _parameters = parameters;
            _yandexAuthorization = yandexAuthorization;
#if UNITY_EDITOR
            _saveSystem = new PlayerPrefsSystem();
#elif YANDEX_GAMES
            _saveSystem = PlayerAccount.IsAuthorized == true ? new YandexSaveSystem() : new PlayerPrefsSystem();
#endif
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        public void ClearSave()
        {
            _gameData = new GameData();
            _saveSystem.Save(_gameData);
        }

        public void Save()
        {
            if (_saveSystem == null)
                throw new NullReferenceException($"{GetType()}: Save(): _saveSystem is null");

            if (_gameData == null)
                throw new NullReferenceException($"{GetType()}: Save(): GameData is null");

            SaveIndexScene();         
            SaveTime(_gamePlayService.ElapsedTime);
            SaveCountCollisionObstacles(_gamePlayService.CountCollisionObstacles);
            SaveAllDistanceTraveled(_gamePlayService.DistanceTraveledOverAllTime);

            _saveSystem.Save(_gameData);
            Debug.Log($"Save #{++_countSave}");
        }

        public async void Load()
        {
            if (_saveSystem == null)
                throw new NullReferenceException($"{GetType()}: Load(): _saveSystem is null");

            _gameData = await _saveSystem.Load();

            if (_gameData == null)
                throw new NullReferenceException($"{GetType()}: Load(): _gameData is null");

            LoadIndexScene();
            LoadParameters();
            LoadMoney();
            LoadHighscore();
            LoadTime();
            LoadCountCollisionObstacles();
            LoadAllDistanceTraveled();
            LoadMutedState();
        }

        private void SaveIndexScene() => _gameData.IndexScene = _levelService.IndexNextScene;

        private void SaveHighscore(int highscore)
        {
            _gameData.Highscore = highscore;
        }

        private void SaveAllDistanceTraveled(int distanceTraveledOverAllTime) => _gameData.DistanceTraveledOverAllTime = distanceTraveledOverAllTime;

        private void SaveMoney(int money) => _gameData.Money = money;

        private void SaveTime(float elapsedTime) => _gameData.ElapsedTime = elapsedTime;

        private void SaveCountCollisionObstacles(int countCollisionObstacles) => _gameData.CountCollisionObstacles = countCollisionObstacles;

        private void SaveMuted(bool isMuted) => _gameData.IsMuted = isMuted;

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

        private void LoadIndexScene() => _levelService.LoadLevel(_gameData.IndexScene);

        private void LoadTime() => _gamePlayService.SetElapsedTime(_gameData.ElapsedTime);

        private void LoadCountCollisionObstacles() => _gamePlayService.LoadCountCollisionObstacles(_gameData.CountCollisionObstacles);
        private void LoadAllDistanceTraveled() => _gamePlayService.LoadDistanceTraveledOverAllTime(_gameData.DistanceTraveledOverAllTime);

        private void LoadMoney() => _wallet.LoadMoney(_gameData.Money);

        private void LoadHighscore() => _levelScore.LoadHighscore(_gameData.Highscore);

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
            _yandexAuthorization.Authorized += OnAuthorized;

            foreach (var parameter in _parameters)
            {
                parameter.Value.LevelChanged += SaveParameter;
            }
        }

        private async void OnAuthorized()
        {
            _saveSystem = new YandexSaveSystem();
            GameData gameDate = await _saveSystem.Load();

            if (gameDate.IndexScene == DefaultScene)
                _saveSystem.Save(_gameData);
        }

        private void Unsubscribe()
        {
            _wallet.MoneyChanged -= SaveMoney;
            _levelScore.HighscoreChanged -= SaveHighscore;
            _soundController.MutedChanged -= SaveMuted;
            _yandexAuthorization.Authorized -= OnAuthorized;

            foreach (var parameter in _parameters)
            {
                parameter.Value.LevelChanged -= SaveParameter;
            }
        }
    }
}
