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

        public int LengthRoad => (int)(_finishWall.transform.position.z * DistanceCoefficient);
        public int NumberCurrentLevel => SceneManager.GetActiveScene().buildIndex;
        public LevelScore Score { get; private set; }

        public void Initialize(ITravelable travelable, Parameter income)
        {
            Score = new LevelScore(travelable, income);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(NumberCurrentLevel);
        }

        public void LoadLevel(int indexScene)
        {
            if (indexScene == NumberCurrentLevel)
                return;

            SceneManager.LoadScene(indexScene);
        }
    }
}