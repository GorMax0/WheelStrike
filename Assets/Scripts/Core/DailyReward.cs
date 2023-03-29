using Parameters;
using Services;
using Services.GameStates;
using UnityEngine;

namespace Core
{
    public class DailyReward
    {
        private readonly int _baseReward = 200;
        private readonly float _multipleReward = 1.5f;

        private int _countDayEntry;
        private GameStateService _gameStateService;
        private DateTimeService _dateTimeService;
        private Wallet _wallet;
        private Parameter _income;

        public int Reward => CalculateReward();
        public int CountDayEntry => _countDayEntry;

        public DailyReward(GameStateService gameStateService, DateTimeService dateTimeService, Wallet wallet,
            Parameter income)
        {
            _gameStateService = gameStateService;
            _dateTimeService = dateTimeService;
            _wallet = wallet;
            _income = income;
        }

        public bool HasNextDaily()
        {
            Debug.Log(_dateTimeService.CurrentDatetime.Day - _dateTimeService.PreviousDate.Day);
            return _dateTimeService.CurrentDatetime.Day - _dateTimeService.PreviousDate.Day > 0;
        }

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
        }

        private int CalculateReward() =>
            (int)(_baseReward * _multipleReward * (1 + _countDayEntry) * (1 + _income.Value));

        private void EnrollReward()
        {
            _wallet.EnrollMoney(Reward);
            _dateTimeService.SaveDate();
            _gameStateService.ChangeState(GameState.Save);
        }
    }
}