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
        [SerializeField] private string _nameForAnalytic;
        [SerializeField] private string _name;
        [SerializeField] private TMP_Text _nameView;
        [SerializeField] private Wall _finishWall;

        private const float DistanceCoefficient = 5f;

        private int _indexCurrentScene;
        private ITravelable _travelable;
        private bool _isInitialize;

        public string NameForAnalytic => _nameForAnalytic;
        public int LengthRoad => (int)(_finishWall.transform.position.z * DistanceCoefficient);
        public int IndexNextScene => _indexCurrentScene; // + 1;
        public LevelScore Score { get; private set; }

        public void Initialize(ITravelable travelable, Parameter income)
        {
            if (_isInitialize == true)
                throw new System.InvalidOperationException($"{GetType()}: Initialize(ITravelable travelable, Parameter income): Already initialized.");

            Score = new LevelScore(travelable, income);
            _indexCurrentScene = SceneManager.GetActiveScene().buildIndex;
            _nameView.text = _name;
            _travelable = travelable;
            _isInitialize = true;
        }

        public void LoadLevel(int indexScene)
        {
            if (indexScene <= _indexCurrentScene)
                return;

            SceneManager.LoadScene(indexScene);
        }

        public void SetNextLevel()
        {
            if (_travelable.DistanceTraveled >= LengthRoad && _indexCurrentScene < SceneManager.sceneCountInBuildSettings - 1)
                _indexCurrentScene++;
        }

        public void RestartLevel() => SceneManager.LoadScene(_indexCurrentScene);
    }
}