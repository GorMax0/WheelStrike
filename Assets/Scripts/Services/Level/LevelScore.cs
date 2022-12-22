using System;
using Core.Wheel;
using Parameters;

namespace Services.Level
{
    public class LevelScore
    {
        private ITravelable _travelable;
        private Parameter _income;
        private int _score;
        private int _highscore;

        public LevelScore(ITravelable travelable, Parameter income)
        {
            _travelable = travelable;
            _income = income;
        }

        public event Action<int> HighscoreChanged;

        public int Score => _score + _travelable.DistanceTraveled;
        public int BonusScore => (int)(Score * _income.Value);
        public int ResultScore => Score + BonusScore;
        public int Highscore
        {
            get
            {
                return _highscore;
            }
            private set
            {
                if (_highscore < _travelable.DistanceTraveled)
                {
                    _highscore = _travelable.DistanceTraveled;
                    HighscoreChanged?.Invoke(_highscore);
                    return;
                }

                _highscore = value;
                HighscoreChanged?.Invoke(_highscore);
            }
        }

        public void LoadHighscore(int highscore)
        {
            Highscore = highscore;            
        }

        public void AddScore(int score)
        {
            if (score <= 0)
                throw new InvalidOperationException($"{GetType()}: AddScore(int score): Amount money {score} is invalid.");

            _score += score;
        }
    }
}