using UnityEngine;
using UnityEngine.SceneManagement;
using Core.Wheel;
using Parameters;
using Core;

namespace Services.Level
{
    public class LevelService : MonoBehaviour
    {
        [SerializeField] private Scene _nextScene;
        [SerializeField] private Wall _finishWall;

        private const float DistanceCoefficient = 5f;

        private LevelScore _score;
        private string _currentSceneName;

        public LevelScore Score => _score;
        public int LengthRoad => (int)(_finishWall.transform.position.z * DistanceCoefficient);

        public void Initialize(ITravelable travelable, Parameter income)
        {   
            _score = new LevelScore(travelable, income);
            _currentSceneName = SceneManager.GetActiveScene().name;
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(_currentSceneName);
        }
    }
}