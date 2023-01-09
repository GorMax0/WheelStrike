using UnityEngine;
using UnityEngine.SceneManagement;
using Core.Wheel;
using Parameters;
using Core;
using TMPro;

namespace Services.Level
{
    public class LevelService : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private TMP_Text _nameView;
        [SerializeField] private Wall _finishWall;

        private const float DistanceCoefficient = 5f;

        private int _indexCurrentScene;

        public int LengthRoad => (int)(_finishWall.transform.position.z * DistanceCoefficient);
        public int IndexNextScene => _indexCurrentScene; // + 1;
        public LevelScore Score { get; private set; }

        public void Initialize(ITravelable travelable, Parameter income)
        {
            Score = new LevelScore(travelable, income);
            _indexCurrentScene = SceneManager.GetActiveScene().buildIndex;
            _nameView.text = _name;
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(_indexCurrentScene);
        }

        public void LoadLevel(int indexScene)
        {
            if (indexScene <= _indexCurrentScene)
                return;

            SceneManager.LoadScene(indexScene);
        }
    }
}