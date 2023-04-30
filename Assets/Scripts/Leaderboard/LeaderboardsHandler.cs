using System.Collections;
using System.Collections.Generic;
using Achievements;
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
        [SerializeField] private LeaderboardView _highscoreDistanceLeaderboardView;
        [SerializeField] private LeaderboardView _collisionsLeaderboardView;
        [SerializeField] private int _numberTopPlayers = 10;
        [SerializeField] private Button _close;

        [Header("Authorization request")]
        [SerializeField] private AuthorizationView _authorizationView;

        private const LeaderboardType DefaultLeaderboard = LeaderboardType.Traveled;
        private const string DistanceTraveledBoard = "DistanceTraveledBoardv04";
        private const string HighscoreDistanceBoard = "HighscoreDistanceBoard04";
        private const string CollisionsBoard = "CollisionsBoard04";
        private const float WaitBetweenSaveScore = 1.15f;

        private LeaderboardYandex _distanceTraveledLeaderboard;
        private LeaderboardYandex _highscoreDistanceLeaderboard;
        private LeaderboardYandex _collisionsLeaderboard;
        private GamePlayService _gamePlayService;
        private AchievementSystem _achievementSystem;
        private bool _isInitialized;
        private bool _isDistanceTraveledLeaderboardCached;
        private bool _isHighscoreDistanceLeaderboardCached;
        private bool _isCollisionsLeaderboardCached;

        private void OnEnable()
        {
            _tabMenu.TabSwitched += OnTabSwitched;
            _close.onClick.AddListener(Disable);
        }

        private void OnDisable()
        {
            _tabMenu.TabSwitched -= OnTabSwitched;
            _close.onClick.RemoveListener(Disable);
        }

        private void OnDestroy()
        {
            _distanceTraveledLeaderboard.GetEntriesCompleted -= OnGetEntriesCompleted;
            _distanceTraveledLeaderboard.GetPlayerEntryCompleted -= OnGetPlayerEntryCompleted;
            _highscoreDistanceLeaderboard.GetEntriesCompleted -= OnGetEntriesCompleted;
            _highscoreDistanceLeaderboard.GetPlayerEntryCompleted -= OnGetPlayerEntryCompleted;
            _collisionsLeaderboard.GetEntriesCompleted -= OnGetEntriesCompleted;
            _collisionsLeaderboard.GetPlayerEntryCompleted -= OnGetPlayerEntryCompleted;
        }

        public void Initialize(GamePlayService gamePlayService, AchievementSystem achievementSystem)
        {
            if (_isInitialized == true)
                return;

            _gamePlayService = gamePlayService;
            _achievementSystem = achievementSystem;

            _distanceTraveledLeaderboard = CreateLeaderboard(_distanceTraveledLeaderboardView, DistanceTraveledBoard);
            _highscoreDistanceLeaderboard = CreateLeaderboard(_highscoreDistanceLeaderboardView, HighscoreDistanceBoard);
            _collisionsLeaderboard = CreateLeaderboard(_collisionsLeaderboardView, CollisionsBoard);

            _isInitialized = true;
        }

        public void Enable()
        {
            _container.SetActive(true);

            if (PlayerAccount.IsAuthorized == true)
            {
                _tabMenu.gameObject.SetActive(true);
                _tabMenu.SwitchTab(DefaultLeaderboard);
            }
            else
            {
                _distanceTraveledLeaderboardView.gameObject.SetActive(false);
                _highscoreDistanceLeaderboardView.gameObject.SetActive(false);
                _collisionsLeaderboardView.gameObject.SetActive(false);
                _authorizationView.gameObject.SetActive(true);
                GameAnalyticsSDK.GameAnalytics.NewDesignEvent($"guiClick:Leaderboard:NotAuthorized");
            }
        }

        public void SaveScore() => StartCoroutine(SaveScoreToLeaderboard());
        
        private void Disable()
        {
            _tabMenu.gameObject.SetActive(false);
            _distanceTraveledLeaderboardView.gameObject.SetActive(false);
            _highscoreDistanceLeaderboardView.gameObject.SetActive(false);
            _collisionsLeaderboardView.gameObject.SetActive(false);
            _authorizationView.gameObject.SetActive(false);
            _container.SetActive(false);
        }

        private LeaderboardYandex CreateLeaderboard(LeaderboardView leaderboardView, string leaderboardName)
        {
            LeaderboardYandex leaderboard = new LeaderboardYandex(leaderboardName, _numberTopPlayers);
            leaderboard.GetPlayerEntryCompleted += OnGetPlayerEntryCompleted;
            leaderboard.GetEntriesCompleted += OnGetEntriesCompleted;
            InstantiateLeaderboardView(leaderboardView);

            return leaderboard;
        }

        private IEnumerator SaveScoreToLeaderboard()
        {
            SavePlayerScoreToLeaderboard(_distanceTraveledLeaderboard, _gamePlayService.DistanceTraveledOverAllTime);

            yield return new WaitForSeconds(WaitBetweenSaveScore);

            SavePlayerScoreToLeaderboard(_collisionsLeaderboard, _gamePlayService.CountCollisionObstacles);

            yield return new WaitForSeconds(WaitBetweenSaveScore);

            SavePlayerScoreToLeaderboard(_highscoreDistanceLeaderboard, _gamePlayService.Highscore);
        }

        private void SavePlayerScoreToLeaderboard(LeaderboardYandex leaderboard, int score) => leaderboard.SetScore(score);

        private void InstantiateLeaderboardView(LeaderboardView leaderboardView) => leaderboardView.InstantiateLeaderboardElements(_numberTopPlayers);

        private void GetLeaderboardInformation(LeaderboardYandex leaderboard, ref bool isCached)
        {
            if (isCached == true)
                return;

            leaderboard.GetCurrentPlayer();
            leaderboard.GetEntries();
            isCached = true;
        }

        private void OnTabSwitched(LeaderboardType leaderboardType)
        {
            switch (leaderboardType)
            {
                case LeaderboardType.Traveled:
                    GetLeaderboardInformation(_distanceTraveledLeaderboard, ref _isDistanceTraveledLeaderboardCached);
                    break;
                case LeaderboardType.Highscore:
                    GetLeaderboardInformation(_highscoreDistanceLeaderboard, ref _isHighscoreDistanceLeaderboardCached);
                    break;
                case LeaderboardType.Collisions:
                    GetLeaderboardInformation(_collisionsLeaderboard, ref _isCollisionsLeaderboardCached);
                    break;
            }
        }

        private void OnGetEntriesCompleted(string nameLeaderboard, List<PlayerInfoLeaderboard> topPlayers)
        {
            switch (nameLeaderboard)
            {
                case DistanceTraveledBoard:
                    _distanceTraveledLeaderboardView.RenderTop(topPlayers);
                    break;
                case HighscoreDistanceBoard:
                    _highscoreDistanceLeaderboardView.RenderTop(topPlayers);
                    break;
                case CollisionsBoard:
                    _collisionsLeaderboardView.RenderTop(topPlayers);
                    break;
            }
        }

        private void OnGetPlayerEntryCompleted(string nameLeaderboard, PlayerInfoLeaderboard currentPlayer)
        {
            switch (nameLeaderboard)
            {
                case DistanceTraveledBoard:
                    _distanceTraveledLeaderboardView.RenderCurrentPlayer(currentPlayer);
                    break;
                case HighscoreDistanceBoard:
                    _highscoreDistanceLeaderboardView.RenderCurrentPlayer(currentPlayer);
                    break;
                case CollisionsBoard:
                    _collisionsLeaderboardView.RenderCurrentPlayer(currentPlayer);
                    break;
            }
            
            _achievementSystem.PassValue(AchievementType.Top, _achievementSystem.SetTopRankValue(currentPlayer.Rank));
        }
    }
}