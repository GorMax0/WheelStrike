using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Core;
using Core.Wheel;
using Parameters;
using UI.Views;
using Services.GameStates;

namespace Services.Level
{
    public class LevelService : MonoBehaviour
    {
        [SerializeField] private string _nameForAnalytic;
        [SerializeField] private TMP_Text _nameView;
        [SerializeField] private Wall _finishWall;
        [SerializeField] private TopPanel _topPanel;
        [SerializeField] private WorldPanel _worldPanel;

        private const float DistanceCoefficient = 5f;

        private GameStateService _gameStateService;
        private LevelInfinity _levelInfinity;
        private ITravelable _travelable;
        private int _indexCurrentScene;
        private bool _isInfinity;
        private bool _isInitialize;

        public string NameForAnalytic => _nameForAnalytic;
        public int LengthRoad => (int)(_finishWall.transform.position.z * DistanceCoefficient);
        public int IndexNextScene => _indexCurrentScene;
        public bool IsInfinity => _isInfinity;
        public LevelScore Score { get; private set; }

        public void Initialize(GameStateService gameStateService, ITravelable travelable, InteractionHandler interactionHandler, Parameter income)
        {
            if (_isInitialize == true)
                throw new System.InvalidOperationException($"{GetType()}: Initialize(ITravelable travelable, Parameter income): Already initialized.");

            Score = new LevelScore(travelable, income);
            _gameStateService = gameStateService;
            _indexCurrentScene = SceneManager.GetActiveScene().buildIndex;
            _travelable = travelable;

            if (TryGetLevelInfinity(out _levelInfinity))
                _levelInfinity.Initialize(interactionHandler);

            _isInitialize = true;
        }

        public void LoadLevel(int indexScene)
        {
            if (indexScene <= _indexCurrentScene)
                return;

            SceneManager.LoadScene(indexScene);
        }

        public void ShowWorldPanel()
        {
            if (_indexCurrentScene != SceneManager.GetActiveScene().buildIndex)
            {
                _worldPanel.gameObject.SetActive(true);
                _worldPanel.DisplayProgress();
                _gameStateService.ChangeState(GameState.Save);
                return;
            }

            _topPanel.Restart();
        }

        public void SetNextScene()
        {
            if (_travelable.DistanceTraveled >= LengthRoad && _indexCurrentScene < SceneManager.sceneCountInBuildSettings - 1)
                _indexCurrentScene++;
        }

        public void RestartLevel() => SceneManager.LoadScene(_indexCurrentScene);

        private bool TryGetLevelInfinity(out LevelInfinity levelInfinity)
        {
            if (TryGetComponent(out LevelInfinity component))
            {             
                levelInfinity = component;
                _isInfinity = true;
                return true;
            }
           
            levelInfinity = null;
            return false;
        }
    }
}