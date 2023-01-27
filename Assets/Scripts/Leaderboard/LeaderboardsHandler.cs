using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboards
{
    public class LeaderboardsHandler : MonoBehaviour
    {
        [SerializeField] private LeaderboardTabMenu _tabMenu;
        [SerializeField] private LeaderboardView _distanceTraveledLeaderboardView;
        [SerializeField] private LeaderboardView _collisionsLeaderboardView;
        [SerializeField] private int _numberTopPlayers = 10;
        [SerializeField] private Button _close;

        private const string DistanceTraveledBoard = "DistanceTraveledBoard";
        private const string CollisionsBoard = "CollisionsBoard";

        private LeaderboardYandex _distanceTraveledLeaderboard;
        private LeaderboardYandex _collisionsLeaderboard;
        private GamePlayService _gamePlayService;
        private bool _isInitialized;

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

        public void Initialize(GamePlayService gamePlayService)
        {
            if (_isInitialized == true)
                return;

            _gamePlayService = gamePlayService;
            _distanceTraveledLeaderboard = new LeaderboardYandex(DistanceTraveledBoard, _numberTopPlayers);
            InstantiateLeaderboardView(_distanceTraveledLeaderboardView);
            _collisionsLeaderboard = new LeaderboardYandex(CollisionsBoard, _numberTopPlayers);
            InstantiateLeaderboardView(_collisionsLeaderboardView);

            _isInitialized = true;
        }

        public void Enable()
        {
            GameAnalyticsSDK.GameAnalytics.NewDesignEvent($"guiClick:Leaderboard:Open");
            SavePlayerScoreToLeaderboard(_distanceTraveledLeaderboard, _gamePlayService.DistanceTraveledOverAllTime);
            SavePlayerScoreToLeaderboard(_collisionsLeaderboard, _gamePlayService.CountCollisionObstacles);
            PrepareLeaderboard(_distanceTraveledLeaderboard, _distanceTraveledLeaderboardView);
            PrepareLeaderboard(_collisionsLeaderboard, _collisionsLeaderboardView);
            gameObject.SetActive(true);
        }

        private void Disable()
        {
            gameObject.SetActive(false);
            _distanceTraveledLeaderboardView.gameObject.SetActive(true);
            _collisionsLeaderboardView.gameObject.SetActive(false);
        }

        private void SavePlayerScoreToLeaderboard(LeaderboardYandex leaderboard, int score) => leaderboard.SetPlayerScoreToLeaderboard(score);

        private void OnCollisionTabSelected(bool isCollisionTab)
        {
            _distanceTraveledLeaderboardView.gameObject.SetActive(!isCollisionTab);
            _collisionsLeaderboardView.gameObject.SetActive(isCollisionTab);
        }

        private void InstantiateLeaderboardView(LeaderboardView leaderboardView) => leaderboardView.InstantiateLeaderboardElements(_numberTopPlayers);

        private IEnumerable<PlayerInfoLeaderboard> GetEntries(LeaderboardYandex leaderboard, out PlayerInfoLeaderboard currentPlayer)
        {
            IEnumerable<PlayerInfoLeaderboard> topPlayers = leaderboard.GetEntries();
            currentPlayer = leaderboard.GetCurrentPlayer();

            return topPlayers;
        }

        private void RenderLeaderboardView(LeaderboardView leaderboardView, IEnumerable<PlayerInfoLeaderboard> topPlayers, PlayerInfoLeaderboard currentPlayer)
            => leaderboardView.Render(topPlayers, currentPlayer);

        private void PrepareLeaderboard(LeaderboardYandex leaderboard, LeaderboardView leaderboardView)
        {
            IEnumerable<PlayerInfoLeaderboard> topPlayers = GetEntries(leaderboard, out PlayerInfoLeaderboard currentPlayer);
            RenderLeaderboardView(leaderboardView, topPlayers, currentPlayer);
        }
    }
}