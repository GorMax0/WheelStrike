using UnityEngine;

namespace Leaderboards
{
    public class LeaderboardTabMenu : MonoBehaviour
    {
        [SerializeField] private LeaderboardTab _distanceTab;
        [SerializeField] private LeaderboardTab _highscoreTab;
        [SerializeField] private LeaderboardTab _collisionTab;
        [SerializeField] private Color _unselectedColor;

        private LeaderboardTab _selectedTab;
        private Color _selectedColor = Color.white;

        public event System.Action<LeaderboardType> TabSwitched;

        private void OnEnable()
        {
            _distanceTab.ButtonClicked += SwitchTab;
            _highscoreTab.ButtonClicked += SwitchTab;
            _collisionTab.ButtonClicked += SwitchTab;
        }

        private void OnDisable()
        {
            _distanceTab.ButtonClicked -= SwitchTab;
            _highscoreTab.ButtonClicked -= SwitchTab;
            _collisionTab.ButtonClicked -= SwitchTab;
        }

        public void SwitchTab(LeaderboardType leaderboard)
        {
            _selectedTab?.Disable(_unselectedColor);

            switch (leaderboard)
            {
                case LeaderboardType.Traveled:
                    _selectedTab = _distanceTab;
                    GameAnalyticsSDK.GameAnalytics.NewDesignEvent($"guiClick:Leaderboard:Traveled");
                    break;
                case LeaderboardType.Highscore:
                    _selectedTab = _highscoreTab;
                    GameAnalyticsSDK.GameAnalytics.NewDesignEvent($"guiClick:Leaderboard:Highscore");
                    break;
                case LeaderboardType.Collisions:
                    _selectedTab = _collisionTab;
                    GameAnalyticsSDK.GameAnalytics.NewDesignEvent($"guiClick:Leaderboard:Collisions");
                    break;
            }

            _selectedTab.Enable(_selectedColor);

            TabSwitched?.Invoke(leaderboard);
        }
    }
}