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

        public LevelScore(ITravelable travelable, Parameter income)
        {
            _travelable = travelable;
            _income = income;
        }
        
        public int Score => _score + (int)_travelable.DistanceTraveled;
        public int BonusScore => (int)(Score * _income.Value);
        public int ResultScore => Score + BonusScore;

        public void AddScore(int score)
        {
            if (score <= 0)
                throw new InvalidOperationException($"{GetType()}: AddScore(int score): Amount money {score} is invalid.");

            _score += score;
        }
    }
}