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
        private readonly AchievementSystem _achievementSystem;

        private readonly DateTimeService _dateTimeService;
        private readonly GameStateService _gameStateService;
        private readonly Parameter _income;
        private readonly Wallet _wallet;

        public DailyReward(
            GameStateService gameStateService,
            Wallet wallet,
            Parameter income,
            AchievementSystem achievementSystem)
        {
            _dateTimeService = new DateTimeService();
            _gameStateService = gameStateService;
            _wallet = wallet;
            _income = income;
            _achievementSystem = achievementSystem;
        }

        public int Reward => CalculateReward();

        public int CountDayEntry { get; private set; }

        public void LoadDailyData(string loadDate, int countDayEntry)
        {
            _dateTimeService.LoadDate(loadDate);
            CountDayEntry = countDayEntry;
        }

        public DateTime GetSavedDate() => _dateTimeService.PreviousDate;

        public bool HasNextDaily()
        {
            if (_dateTimeService.CurrentDatetime.Day - _dateTimeService.PreviousDate.Day > 0
                && _dateTimeService.CurrentDatetime.Month - _dateTimeService.PreviousDate.Month >= 0)
            {
                return true;
            }

            if (_dateTimeService.CurrentDatetime.Day - _dateTimeService.PreviousDate.Day <= 0
                && _dateTimeService.CurrentDatetime.Month - _dateTimeService.PreviousDate.Month > 0)
            {
                return true;
            }

            return false;
        }

        public void EnrollDaily()
        {
            EnrollReward();
            _achievementSystem.PassValue(AchievementType.Daily, CountDayEntry);
            CountDayEntry++;
            _gameStateService.ChangeState(GameState.Save);
        }

        private int CalculateReward() =>
            (int)(_baseReward * _multipleReward * CountDayEntry * (1 + _income.Value));

        private void EnrollReward()
        {
            _wallet.EnrollMoney(Reward);
            _dateTimeService.SaveDate();

            if (CountDayEntry >= Week)
                CountDayEntry = Week;
        }
    }
}