using UnityEngine;
using UnityEngine.SceneManagement;
using Core.Wheel;
using Parameters;
using Core;

namespace Services.Level
{
    public class LevelService : MonoBehaviour
    {
        [SerializeField] private Wall _finishWall;

        private const float DistanceCoefficient = 5f;

        private int _numberCurrentLevel;

        public int LengthRoad => (int)(_finishWall.transform.position.z * DistanceCoefficient);
        public int NumberNextLevel => _numberCurrentLevel; // + 1;
        public LevelScore Score { get; private set; }

        public void Initialize(ITravelable travelable, Parameter income)
        {
            Score = new LevelScore(travelable, income);
            _numberCurrentLevel = SceneManager.GetActiveScene().buildIndex;
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(_numberCurrentLevel);
        }

        public void LoadLevel(int indexScene)
        {
            if (indexScene <= _numberCurrentLevel)
                return;

            SceneManager.LoadScene(indexScene);
        }
    }
}