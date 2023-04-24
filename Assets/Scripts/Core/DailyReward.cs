using System;
using Achievements;
using Parameters;
using Services;
using Services.GameStates;

namespace Core
{
    public class DailyReward
    {
        private const int OneDay = 1;
        private const int Week = 7;
        private readonly int _baseReward = 200;
        private readonly float _multipleReward = 1.5f;

        private int _countDayEntry;
        private GameStateService _gameStateService;
        private DateTimeService _dateTimeService;
        private Wallet _wallet;
        private Parameter _income;
        private AchievementSystem _achievementSystem;

        public int Reward => CalculateReward();
        public int CountDayEntry => _countDayEntry;

        public DailyReward(GameStateService gameStateService, Wallet wallet, Parameter income, AchievementSystem achievementSystem)
        {
            _dateTimeService = new DateTimeService();
            _gameStateService = gameStateService;
            _wallet = wallet;
            _income = income;
            _achievementSystem = achievementSystem;
        }

        public void LoadDailyData(string loadDate, int countDayEntry)
        {
            _dateTimeService.LoadDate(loadDate);
            _countDayEntry = countDayEntry;
        }

        public DateTime GetSavedDate() => _dateTimeService.PreviousDate;

        public bool HasNextDaily() => _dateTimeService.CurrentDatetime.Day - _dateTimeService.PreviousDate.Day > 0;

        public void EnrollDaily()
        {
            int intervalDays = _dateTimeService.CurrentDatetime.Day - _dateTimeService.PreviousDate.Day;

            if (intervalDays < OneDay)
                return;
            
            EnrollReward();
            _achievementSystem. PassValue(AchievementType.Daily, _countDayEntry);
            _countDayEntry++;
            _gameStateService.ChangeState(GameState.Save);
        }

        private int CalculateReward() =>
            (int)(_baseReward * _multipleReward * _countDayEntry * (1 + _income.Value));

        private void EnrollReward()
        {
            _wallet.EnrollMoney(Reward);
            _dateTimeService.SaveDate();

            if (_countDayEntry >= Week)
                _countDayEntry = Week;
        }
    }
}