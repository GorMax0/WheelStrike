using UnityEngine.SceneManagement;

namespace Services
{
    public class LevelService
    {
        private string _currentSceneName;

        public LevelService()
        {
            _currentSceneName = SceneManager.GetActiveScene().name;
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(_currentSceneName);
        }
    }
}