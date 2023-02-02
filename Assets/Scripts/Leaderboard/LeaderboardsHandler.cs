using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using UI.Views;
using Agava.YandexGames;

namespace Leaderboards
{
    public class LeaderboardsHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private LeaderboardTabMenu _tabMenu;
        [SerializeField] private LeaderboardView _distanceTraveledLeaderboardView;
        [SerializeField] private LeaderboardView _collisionsLeaderboardView;
        [SerializeField] private int _numberTopPlayers = 10;
        [SerializeField] private Button _close;

        [Header("Authorization request")]
        [SerializeField] private AuthorizationView _authorizationView;

        private const string DistanceTraveledBoard = "DistanceTraveledBoard";
        private const string CollisionsBoard = "CollisionsBoard";
        private const float WaitBetweenSaveScore = 1.15f;

        private LeaderboardYandex _distanceTraveledLeaderboard;
        private LeaderboardYandex _collisionsLeaderboard;
        private GamePlayService _gamePlayService;
        private bool _isInitialized;
        private bool _isCollisionsLeaderboardCached;
        private bool _isDistanceTraveledLeaderboardCached;

        private void OnEnable()
        {
            _tabMenu.CollisionTabSelected += OnCollisionTabSelected;
            _close.onClick.AddListener(Disable);

        }

        private void OnDisable()
        {
            _tabMenu.CollisionTabSelected -= OnCollisionTabSelected;
            _close.onClick.RemoveListener(Disable);

        }

        private void OnDestroy()
        {
            _distanceTraveledLeaderboard.GetEntriesCompleted -= OnGetEntriesCompleted;
            _distanceTraveledLeaderboard.GetPlayerEntryCompleted -= OnGetPlayerEntryCompleted;
            _collisionsLeaderboard.GetEntriesCompleted -= OnGetEntriesCompleted;
            _collisionsLeaderboard.GetPlayerEntryCompleted -= OnGetPlayerEntryCompleted;
        }

        public void Initialize(GamePlayService gamePlayService)
        {
            if (_isInitialized == true)
                return;

            _gamePlayService = gamePlayService;

            _distanceTraveledLeaderboard = new LeaderboardYandex(DistanceTraveledBoard, _numberTopPlayers);
            _distanceTraveledLeaderboard.GetPlayerEntryCompleted += OnGetPlayerEntryCompleted;
            _distanceTraveledLeaderboard.GetEntriesCompleted += OnGetEntriesCompleted;
            InstantiateLeaderboardView(_distanceTraveledLeaderboardView);

            _collisionsLeaderboard = new LeaderboardYandex(CollisionsBoard, _numberTopPlayers);
            _collisionsLeaderboard.GetPlayerEntryCompleted += OnGetPlayerEntryCompleted;
            _collisionsLeaderboard.GetEntriesCompleted += OnGetEntriesCompleted;
            InstantiateLeaderboardView(_collisionsLeaderboardView);

            _isInitialized = true;
        }

        public void Enable()
        {
            _container.SetActive(true);
            GameAnalyticsSDK.GameAnalytics.NewDesignEvent($"guiClick:Leaderboard:Open");

            if (PlayerAccount.IsAuthorized == true)
            {
                OnCollisionTabSelected(false);
                _tabMenu.gameObject.SetActive(true);
                _tabMenu.SelectCollisionTab(false);
            }
            else
            {
                _distanceTraveledLeaderboardView.gameObject.SetActive(false);
                _collisionsLeaderboardView.gameObject.SetActive(false);
                _authorizationView.gameObject.SetActive(true);
            }
        }

        private void Disable()
        {
            _tabMenu.gameObject.SetActive(false);
            _distanceTraveledLeaderboardView.gameObject.SetActive(false);
            _collisionsLeaderboardView.gameObject.SetActive(false);
            _authorizationView.gameObject.SetActive(false);
            _container.SetActive(false);
        }

        public void SaveScore() => StartCoroutine(SaveScoreToLeaderboard());

        private IEnumerator SaveScoreToLeaderboard()
        {
            SavePlayerScoreToLeaderboard(_distanceTraveledLeaderboard, _gamePlayService.DistanceTraveledOverAllTime);

            yield return new WaitForSeconds(WaitBetweenSaveScore);

            SavePlayerScoreToLeaderboard(_collisionsLeaderboard, _gamePlayService.CountCollisionObstacles);
        }

        private void SavePlayerScoreToLeaderboard(LeaderboardYandex leaderboard, int score) => leaderboard.SetScore(score);

        private void InstantiateLeaderboardView(LeaderboardView leaderboardView) => leaderboardView.InstantiateLeaderboardElements(_numberTopPlayers);

        private void OnCollisionTabSelected(bool isCollisionTab)
        {
            if (isCollisionTab == true)
            {
                if (_isCollisionsLeaderboardCached == false)
                {
                    _collisionsLeaderboard.GetCurrentPlayer();
                    _collisionsLeaderboard.GetEntries();
                    _isCollisionsLeaderboardCached = true;
                }
            }
            else
            {
                if (_isDistanceTraveledLeaderboardCached == false)
                {
                    _distanceTraveledLeaderboard.GetCurrentPlayer();
                    _distanceTraveledLeaderboard.GetEntries();
                    _isDistanceTraveledLeaderboardCached = true;
                }
            }
        }

        private void OnGetEntriesCompleted(string nameLeaderboard, List<PlayerInfoLeaderboard> topPlayers)
        {
            switch (nameLeaderboard)
            {
                case DistanceTraveledBoard:
                    _distanceTraveledLeaderboardView.RenderTop(topPlayers);
                    break;
                case CollisionsBoard:
                    _collisionsLeaderboardView.RenderTop(topPlayers);
                    break;
            }

            //     Debug.Log($"Invoke OnGetEntriesCompleted for {nameLeaderboard}");
        }

        private void OnGetPlayerEntryCompleted(string nameLeaderboard, PlayerInfoLeaderboard currentPlayer)
        {
            switch (nameLeaderboard)
            {
                case DistanceTraveledBoard:
                    _distanceTraveledLeaderboardView.RenderCurrentPlayer(currentPlayer);
                    break;
                case CollisionsBoard:
                    _collisionsLeaderboardView.RenderCurrentPlayer(currentPlayer);
                    break;
            }

            //    Debug.Log($"Invoke OnGetPlayerEntryCompleted for {nameLeaderboard}");
        }
    }
}