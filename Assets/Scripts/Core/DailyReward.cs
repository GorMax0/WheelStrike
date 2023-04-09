using System;
using Parameters;
using Services;
using Services.GameStates;
using UnityEngine;

namespace Core
{
    public class DailyReward
    {
        private const int TwoWeeks = 14;
        private readonly int _baseReward = 200;
        private readonly float _multipleReward = 1.5f;

        private int _countDayEntry;
        private GameStateService _gameStateService;
        private DateTimeService _dateTimeService;
        private Wallet _wallet;
        private Parameter _income;

        public int Reward => CalculateReward();
        public int CountDayEntry => _countDayEntry;

        public DailyReward(GameStateService gameStateService, Wallet wallet, Parameter income)
        {
            _dateTimeService = new DateTimeService();
            _gameStateService = gameStateService;
            _wallet = wallet;
            _income = income;
        }

        public void LoadDailyData(string loadDate, int countDayEntry)
        {
            _dateTimeService.LoadDate(loadDate);
            _countDayEntry = countDayEntry;
            Debug.Log($"Count day entry {_countDayEntry}, Load data {loadDate}, Previous data: {_dateTimeService.PreviousDate}");
            Debug.Log($"Reward daily {Reward}");
        }

        public DateTime GetSavedDate() => _dateTimeService.PreviousDate;

        public bool HasNextDaily() => _dateTimeService.CurrentDatetime.Day - _dateTimeService.PreviousDate.Day > 0;

        public void EnrollDaily()
        {
            int intervalDays = _dateTimeService.CurrentDatetime.Day - _dateTimeService.PreviousDate.Day;

            switch (intervalDays)
            {
                case 1:
                    EnrollReward();
                    _countDayEntry++;
                    break;
                case > 1:
                    EnrollReward();
                    _countDayEntry = 1;
                    break;
            }

            _gameStateService.ChangeState(GameState.Save);
        }

        private int CalculateReward() =>
            (int)(_baseReward * _multipleReward * (1 + _countDayEntry) * (1 + _income.Value));

        private void EnrollReward()
        {
            _wallet.EnrollMoney(Reward);
            _dateTimeService.SaveDate();

            if (_countDayEntry >= TwoWeeks)
                _countDayEntry = 1;
        }
    }
}