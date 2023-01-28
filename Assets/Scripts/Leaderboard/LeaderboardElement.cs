using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Leaderboards
{
    public class LeaderboardElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playerRank;
        [SerializeField] private TMP_Text _playerNick;
        [SerializeField] private TMP_Text _playerResult;
        [SerializeField] private Image _medalIcon;
        [SerializeField] private Color _playerColor;

        private const string AnonymousName = "Anonymous";
        private const string NoInfo = "---";
        private const int MaximumDisplayedRank = 999;

        public void Render(PlayerInfoLeaderboard playerInfo, bool isPlayer, Sprite medal = null)
        {
            if (playerInfo == null)
                throw new System.NullReferenceException($"{GetType()}: Render(PlayerInfoLeaderboard playerInfo, bool isPlayer, Sprite medal = null): playerInfo nullable.");

            if (medal != null)
            {
                _medalIcon.sprite = medal;
                _medalIcon.gameObject.SetActive(true);
            }
            else
            {
                _playerRank.text = playerInfo.Rank > MaximumDisplayedRank ? ">1000" : playerInfo.Rank.ToString();
            }

            _playerNick.text = playerInfo.Name;
            _playerResult.text = playerInfo.Score.ToString();

            if (isPlayer == true)
            {
                _playerRank.color = _playerColor;
                _playerNick.color = _playerColor;
                _playerResult.color = _playerColor;
            }
        }
    }
}