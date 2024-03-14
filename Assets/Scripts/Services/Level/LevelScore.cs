using System;
using Boost;
using Core.Wheel;
using Parameters;

namespace Services.Level
{
    public class LevelScore
    {
        private const int HundredPercent = 1;
        private int _adsRewardRate = 1;
        private readonly BoostParameter _boost;
        private readonly Parameter _income;
        private int _reward;

        private readonly ITravelable _travelable;

        public LevelScore(ITravelable travelable, Parameter income, BoostParameter boost)
        {
            _travelable = travelable;
            _income = income;
            _boost = boost;
        }

        public int Reward => _reward + _travelable.DistanceTraveled;

        public int BonusReward => (int)(Reward * (_income.Value * (HundredPercent + _boost.IncomeMultiplier)));

        public int ResultReward => (Reward + BonusReward) * _adsRewardRate;

        public int Highscore { get; private set; }

        public event Action<int> HighscoreChanged;

        public event Action<int> HighscoreLoaded;

        public void LoadHighscore(int highscore)
        {
            Highscore = highscore;
            HighscoreLoaded?.Invoke(Highscore);
        }

        public void AddReward(int reward)
        {
            if (reward <= 0)
                throw new InvalidOperationException(
                    $"{GetType()}: AddScore(int reward): Amount money {reward} is invalid.");

            _reward += reward;
        }

        public void SetHighscore(int distance)
        {
            if (Highscore >= distance)
                return;

            Highscore = distance;
            HighscoreChanged?.Invoke(Highscore);
        }

        public void SetAdsRewardRate(int rate) => _adsRewardRate = rate;
    }
}