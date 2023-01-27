using System.Collections.Generic;
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
        [SerializeField] private TMP_Text _playerMedalRank;
        [SerializeField] private Image _medalIcon;
        [SerializeField] private Color _playerColor;

        private const string AnonymousName = "Anonymous";
        private const string NoInfo = "---";
        private const int MaximumDisplayedRank = 999;

        public void Render(PlayerInfoLeaderboard playerInfo, bool isPlayer, Sprite medal = null)
        {
            if (medal != null)
            {
                _playerMedalRank.text = playerInfo.Rank.ToString();
                _medalIcon.sprite = medal;
                _medalIcon.gameObject.SetActive(true);
            }
            else if (playerInfo.Name == AnonymousName)
            {
                _playerRank.text = NoInfo;
            }
            else
            {
                _playerRank.text = playerInfo.Rank > MaximumDisplayedRank ? ">1000" : playerInfo.Rank.ToString();
            }

            _playerNick.text = playerInfo.Name;
            _playerResult.text = playerInfo.Name == AnonymousName ? NoInfo : playerInfo.Score.ToString();

            if (isPlayer == true)
            {
                _playerRank.color = _playerColor;
                _playerNick.color = _playerColor;
                _playerResult.color = _playerColor;
            }
        }
    }
}