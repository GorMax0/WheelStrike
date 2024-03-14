using System;
using Boost;
using Core;
using Core.Wall;
using Core.Wheel;
using Parameters;
using Services.GameStates;
using TMPro;
using UI.Views;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.Level
{
    public class LevelService : MonoBehaviour
    {
        private const int IndexLevelOne = 2;
        private const float DistanceCoefficient = 5f;
        [SerializeField] private string _nameForAnalytic;
        [SerializeField] private TMP_Text _nameView;
        [SerializeField] private Wall _finishWall;
        [SerializeField] private TopPanel _topPanel;
        [SerializeField] private WorldPanel _worldPanel;

        private GameStateService _gameStateService;
        private LevelGenerator _levelGenerator;
        private ITravelable _travelable;
        private bool _isInitialize;

        public string NameForAnalytic => _nameForAnalytic;

        public float LengthRoad => _finishWall.transform.position.z * DistanceCoefficient;

        public int IndexNextScene { get; private set; }

        public bool IsInfinity { get; private set; }

        public LevelScore Score { get; private set; }

        public void Initialize(
            GameStateService gameStateService,
            ITravelable travelable,
            InteractionHandler interactionHandler,
            Parameter income,
            BoostParameter boost)
        {
            if (_isInitialize)
            {
                throw new InvalidOperationException(
                    $"{GetType()}: Initialize(ITravelable travelable, Parameter income): Already initialized.");
            }

            Score = new LevelScore(travelable, income, boost);
            _gameStateService = gameStateService;
            IndexNextScene = SceneManager.GetActiveScene().buildIndex;
            _travelable = travelable;

            if (TryGetLevelInfinity(out _levelGenerator))
                _levelGenerator.Initialize(interactionHandler);

            _isInitialize = true;
        }

        public void LoadLevel(int indexScene)
        {
            if (indexScene <= IndexNextScene)
                return;

            SceneManager.LoadScene(indexScene);
        }

        public void ShowWorldPanel()
        {
            if (IndexNextScene != SceneManager.GetActiveScene().buildIndex)
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
            if (_travelable.DistanceTraveled >= LengthRoad
                && IndexNextScene < SceneManager.sceneCountInBuildSettings - 1)
                IndexNextScene++;
        }

        public void ResetLevelProgress() => IndexNextScene = IndexLevelOne;

        public void RestartLevel() => SceneManager.LoadScene(IndexNextScene);

        private bool TryGetLevelInfinity(out LevelGenerator levelGenerator)
        {
            if (TryGetComponent(out LevelGenerator component))
            {
                levelGenerator = component;
                IsInfinity = true;

                return true;
            }

            levelGenerator = null;

            return false;
        }
    }
}