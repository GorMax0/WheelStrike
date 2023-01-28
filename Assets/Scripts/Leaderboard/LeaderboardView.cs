using System.Collections.Generic;
using UnityEngine;

namespace Leaderboards
{
    public class LeaderboardView : MonoBehaviour
    {
        [SerializeField] private Transform _parentObject;
        [SerializeField] private LeaderboardElement _leaderboardElementPrefab;
        [SerializeField] private LeaderboardElement _playerElement;
        [SerializeField] private Sprite[] _medals;

        private List<LeaderboardElement> _createdElements = new List<LeaderboardElement>();
        private PlayerInfoLeaderboard _currentPlayer;
        private bool _isCreated;

        public void InstantiateLeaderboardElements(int numberTopPlayers)
        {
            if (_isCreated == true)
                return;

            for (int i = 0; i < numberTopPlayers; i++)
            {
                LeaderboardElement leaderboardElement = Instantiate(_leaderboardElementPrefab, _parentObject);
                _createdElements.Add(leaderboardElement);
            }

            _isCreated = true;
        }

        public void RenderTop(List<PlayerInfoLeaderboard> topPlayers)
        {
            for (int i = 0; i < topPlayers.Count; i++)
            {
                _createdElements[i].Render(topPlayers[i], topPlayers[i].Rank == _currentPlayer?.Rank ? true : false,
                   i < _medals.Length ? _medals[i] : null);
            }
        }

        public void RenderCurrentPlayer(PlayerInfoLeaderboard currentPlayer)
        {
            Sprite medal = null;

            if (currentPlayer.Rank > 0)
                medal = currentPlayer.Rank - 1 < _medals.Length ? _medals[currentPlayer.Rank - 1] : null;

            _currentPlayer = currentPlayer;
            _playerElement.Render(currentPlayer, true, medal);
        }
    }
}