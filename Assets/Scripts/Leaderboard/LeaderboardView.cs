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

        public void Render(IEnumerable<PlayerInfoLeaderboard> topPlayers, PlayerInfoLeaderboard currentPlayer)
        {
            int i = 0;

            foreach (var player in topPlayers)
            {  
                _createdElements[i].Render(player, player == currentPlayer ? true : false,
                   i < _medals.Length ? _medals[i] : null);
                i++;
            }

            _playerElement.Render(currentPlayer, true);
        }
    }
}