using System;
using System.Collections.Generic;
using Achievements;
using Core;
using Parameters;
using Boost;
using Services;
using Services.Level;
using Authorization;
using Agava.YandexGames;
using Services.GameStates;
using UI.Manual.Tutorial;
using UnityEngine;

namespace Data
{
    public class DataOperator : IDisposable
    {
        private const string DataVersion = "v0.4.13";
        private const int DefaultScene = 1;

        private GameData _gameData;
        private ISaveSystem _saveSystem;
        private readonly GamePlayService _gamePlayService;
        private readonly GameStateService _gameStateService;
        private readonly LevelService _levelService;
        private readonly LevelScore _levelScore;
        private readonly SoundController _soundController;
        private readonly QualityToggle _qualityToggle;
        private readonly Wallet _wallet;
        private readonly Dictionary<ParameterType, Parameter> _parameters;
        private readonly CounterParameterLevel _counterParameterLevel;
        private readonly BoostParameter _boost;
        private readonly YandexAuthorization _yandexAuthorization;
        private readonly DailyReward _dailyReward;
        private readonly AchievementSystem _achievementSystem;
        private readonly TutorialManager _tutorialManager;

        public DataOperator(GamePlayService gamePlayService, GameStateService gameStateService, LevelService levelService, SoundController soundController,
            QualityToggle qualityToggle, Wallet wallet, Dictionary<ParameterType, Parameter> parameters, CounterParameterLevel counterParameterLevel,
            BoostParameter boost,
            YandexAuthorization yandexAuthorization, DailyReward dailyReward, AchievementSystem achievementSystem, TutorialManager tutorialManager)
        {
            _gamePlayService = gamePlayService;
            _gameStateService = gameStateService;
            _gamePlayService.SetDataOperator(this);
            _levelService = levelService;
            _levelScore = _levelService.Score;
            _soundController = soundController;
            _qualityToggle = qualityToggle;
            _wallet = wallet;
            _parameters = parameters;
            _counterParameterLevel = counterParameterLevel;
            _boost = boost;
            _yandexAuthorization = yandexAuthorization;
            _dailyReward = dailyReward;
            _achievementSystem = achievementSystem;
            _tutorialManager = tutorialManager;
#if UNITY_EDITOR
            _saveSystem = new PlayerPrefsSystem(DataVersion);
#elif YANDEX_GAMES
            _saveSystem =
 PlayerAccount.IsAuthorized == true ? new YandexSaveSystem(DataVersion) : new PlayerPrefsSystem(DataVersion);
#endif
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        public void ClearSave()
        {
            _gameData = new GameData(DataVersion);
            _saveSystem.Save(_gameData);
            PlayerPrefs.DeleteAll();
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }

        public void Save()
        {
            if (_saveSystem == null)
                throw new NullReferenceException($"{GetType()}: Save(): _saveSystem is null");

            if (_gameData == null)
                throw new NullReferenceException($"{GetType()}: Save(): GameData is null");

            SaveIndexScene();
            SaveTime(_gamePlayService.ElapsedTime);
            SavePlaytime(_gamePlayService.Playtime);
            SaveCountCollisionObstacles(_gamePlayService.CountCollisionObstacles);
            SaveAllDistanceTraveled(_gamePlayService.DistanceTraveledOverAllTime);
            SaveCountLaunch(_gamePlayService.CountLaunch);
            SaveCounterParameterLevels();
            SaveDailyInfo();
            SaveTutorialState();
            SaveTutorialState();
            SaveAchievements();

            _saveSystem.Save(_gameData);
        }


        public async void Load()
        {
            if (_saveSystem == null)
                throw new NullReferenceException($"{GetType()}: Load(): _saveSystem is null");

            _gameData = await _saveSystem.Load();
            Debug.Log($"async void Load() {_gameData.DataVersion} complete");

            if (_gameData == null)
                throw new NullReferenceException($"{GetType()}: Load(): _gameData is null");

            LoadIndexScene();
            LoadParameters();
            LoadMoney();
            LoadHighscore();
            LoadTime();
            LoadPlaytime();
            LoadCountCollisionObstacles();
            LoadAllDistanceTraveled();
            LoadCountLaunch();
            LoadMutedState();
            LoadSelectedQuality();
            LoadBoostLevel();
            LoadDailyInfo();
            LoadAchievements();
            _gameStateService.ChangeState(GameState.Load);
        }

        private void SaveIndexScene() => _gameData.IndexScene = _levelService.IndexNextScene;

        private void SaveHighscore(int highscore) => _gameData.Highscore = highscore;

        private void SaveAllDistanceTraveled(int distanceTraveledOverAllTime) =>
            _gameData.DistanceTraveledOverAllTime = distanceTraveledOverAllTime;

        private void SaveMoney(int money)
        {
            int difference = money - _gameData.Money;

            if (difference < 0)
            {
                _gameData.SpentMoney += -difference;
                _achievementSystem.PassValue(AchievementType.SpentMoney, _gameData.SpentMoney);
            }

            _gameData.Money = money;
        }

        private void SaveTime(float elapsedTime) => _gameData.ElapsedTime = elapsedTime;

        private void SavePlaytime(float playtime) => _gameData.Playtime = (int)playtime;

        private void SaveCountCollisionObstacles(int countCollisionObstacles) => _gameData.CountCollisionObstacles = countCollisionObstacles;

        private void SaveCountLaunch(int countLaunch) => _gameData.CountLaunch = countLaunch;

        private void SaveMuted(bool isMuted) => _gameData.IsMuted = isMuted;

        private void SaveSelectedQuality(bool isNormalQuality) => _gameData.IsNormalQuality = isNormalQuality;

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

        private void SaveCounterParameterLevels()
        {
            _gameData.SpeedAchievement = _counterParameterLevel.CountSpeedLevel;
            _gameData.SizeAchievement = _counterParameterLevel.CountSizeLevel;
            _gameData.IncomeAchievement = _counterParameterLevel.CountIncomeLevel;
        }

        private void SaveBoostLevel() => _gameData.BoostLevel = _boost.Level;

        private void SaveDailyInfo()
        {
            _gameData.DailyDate = _dailyReward.GetSavedDate().ToShortDateString();
            _gameData.CountDailyEntry = _dailyReward.CountDayEntry;
        }

        private void SaveTutorialState()
        {
            if (_tutorialManager == null)
                return;

            _gameData.TutorialComplete = _tutorialManager.TutorialComplete;
        }

        private void SaveAchievements()
        {
          //  _gameData.AchievementsData = _achievementSystem.Save();
            _gameData.CountAchievement = _achievementSystem.CountAchievement;
        }

        private void LoadIndexScene() => _levelService.LoadLevel(_gameData.IndexScene);

        private void LoadTime() => _gamePlayService.LoadElapsedTime(_gameData.ElapsedTime);

        private void LoadPlaytime() => _gamePlayService.LoadPlaytime(_gameData.Playtime);

        private void LoadCountCollisionObstacles() => _gamePlayService.LoadCountCollisionObstacles(_gameData.CountCollisionObstacles);

        private void LoadAllDistanceTraveled() => _gamePlayService.LoadDistanceTraveledOverAllTime(_gameData.DistanceTraveledOverAllTime);

        private void LoadCountLaunch() => _gamePlayService.LoadCountLaunch(_gameData.CountLaunch);

        private void LoadCounterParameterLevels() =>
            _counterParameterLevel.Load(_gameData.SpeedAchievement, _gameData.SizeAchievement, _gameData.IncomeAchievement);

        private void LoadMoney() => _wallet.LoadMoney(_gameData.Money);

        private void LoadHighscore() => _levelScore.LoadHighscore(_gameData.Highscore);

        private void LoadMutedState() => _soundController.LoadMutedState(_gameData.IsMuted);

        private void LoadSelectedQuality() => _qualityToggle.LoadSelectedQuality(_gameData.IsNormalQuality);

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
                }
            }
        }

        private void LoadBoostLevel() => _boost.LoadLevel(_gameData.BoostLevel);

        private void LoadDailyInfo() => _dailyReward.LoadDailyData(_gameData.DailyDate, _gameData.CountDailyEntry);

        private void LoadAchievements()
        {
            //   _achievementSystem.LoadAchievementValue(_gameData.AchievementsData);
            LoadCounterParameterLevels();
            _achievementSystem.PassValue(AchievementType.Boost, _boost.Level);
            int playtimePerMinutes = _gameData.Playtime / 60;
            _achievementSystem.PassValue(AchievementType.Playtime, playtimePerMinutes);
            _achievementSystem.PassValue(AchievementType.Highscore, _gameData.Highscore);
            _achievementSystem.PassValue(AchievementType.Obstacle, _gameData.CountCollisionObstacles);
            _achievementSystem.PassValue(AchievementType.Travel, _gameData.DistanceTraveledOverAllTime);
            _achievementSystem.PassValue(AchievementType.Daily, _gameData.CountDailyEntry - 1);
            _achievementSystem.PassValue(AchievementType.Launch, _gameData.CountLaunch);
            _achievementSystem.PassValue(AchievementType.SpentMoney, _gameData.SpentMoney);
            _achievementSystem.PassValue(AchievementType.Training, _gameData.TutorialComplete);
        }


        private void Subscribe()
        {
            _wallet.MoneyChanged += SaveMoney;
            _levelScore.HighscoreChanged += SaveHighscore;
            _soundController.MutedChanged += SaveMuted;
            _qualityToggle.QualityChanged += SaveSelectedQuality;
            _yandexAuthorization.Authorized += OnAuthorized;

            foreach (var parameter in _parameters)
            {
                parameter.Value.LevelChanged += SaveParameter;
            }

            _boost.LevelChanged += SaveBoostLevel;
        }


        private async void OnAuthorized()
        {
            _saveSystem = new YandexSaveSystem(DataVersion);
            GameData gameData = await _saveSystem.Load();

            if (gameData == null || gameData.IndexScene == DefaultScene)
                _saveSystem.Save(_gameData);
        }

        private void Unsubscribe()
        {
            _wallet.MoneyChanged -= SaveMoney;
            _levelScore.HighscoreChanged -= SaveHighscore;
            _soundController.MutedChanged -= SaveMuted;
            _qualityToggle.QualityChanged -= SaveSelectedQuality;
            _yandexAuthorization.Authorized -= OnAuthorized;

            foreach (var parameter in _parameters)
            {
                parameter.Value.LevelChanged -= SaveParameter;
            }

            _boost.LevelChanged -= SaveBoostLevel;
        }
    }
}