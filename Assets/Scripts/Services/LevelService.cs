using UnityEngine.SceneManagement;
using Zenject;

namespace Services
{
    public class LevelService : IInitializable
    {
        private string _currentSceneName;

        public void Initialize()
        {
            _currentSceneName = SceneManager.GetActiveScene().name;
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(_currentSceneName);
        }
    }
}