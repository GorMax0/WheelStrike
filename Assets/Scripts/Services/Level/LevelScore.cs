using System;
using Core.Wheel;
using Parameters;

namespace Services.Level
{
    public class LevelScore
    {
        private ITravelable _travelable;
        private Parameter _income;
        private int _reward;
        private int _adsRewardRate = 1;
        private int _highscore;

        public LevelScore(ITravelable travelable, Parameter income)
        {
            _travelable = travelable;
            _income = income;
        }

        public event Action<int> HighscoreChanged;
        public event Action<int> HighscoreLoaded;

        public int Reward => _reward + _travelable.DistanceTraveled;
        public int BonusReward => (int)(Reward * _income.Value);
        public int ResultReward => (Reward + BonusReward) * _adsRewardRate;

        public void LoadHighscore(int highscore)
        {
            _highscore = highscore;
            HighscoreLoaded?.Invoke(_highscore);
        }

        public void AddReward(int reward)
        {
            if (reward <= 0)
                throw new InvalidOperationException($"{GetType()}: AddScore(int reward): Amount money {reward} is invalid.");

            _reward += reward;
        }
        
        public void SetHighscore(int distance)
        {
            if (_highscore >= distance)
                return;

            _highscore = distance;
            HighscoreChanged?.Invoke(_highscore);
        }

        public void SetAdsRewardRate(int rate) => _adsRewardRate = rate;
    }
}