using UnityEngine.SceneManagement;
using Zenject;

namespace Services
{
    public class LevelService : IInitializable
    {
        private int _currentSceneIndex;

        public void Initialize()
        {
            _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(_currentSceneIndex);
        }
    }
}