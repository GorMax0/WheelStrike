using UnityEngine;
using UnityEngine.SceneManagement;
using Core.Wheel;
using Parameters;

namespace Services.Level
{
    public class LevelService : MonoBehaviour
    {
        [SerializeField] private Scene _nextScene;

        private LevelScore _score;
        private string _currentSceneName;

        public LevelScore Score => _score;

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