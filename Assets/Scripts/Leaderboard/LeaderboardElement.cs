using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard
{
    public class LeaderboardElement : MonoBehaviour
    {
        private const int MaximumDisplayedRank = 999;
        [SerializeField] private TMP_Text _playerRank;
        [SerializeField] private TMP_Text _playerNick;
        [SerializeField] private TMP_Text _playerResult;
        [SerializeField] private Image _medalIcon;
        [SerializeField] private Color _playerColor;

        public void Render(PlayerInfoLeaderboard playerInfo, bool isPlayer, Sprite medal = null)
        {
            if (playerInfo == null)
            {
                throw new NullReferenceException(
                    $"{GetType()}: Render(PlayerInfoLeaderboard playerInfo,"
                    + " bool isPlayer, Sprite medal = null): playerInfo nullable.");
            }

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

            if (isPlayer)
            {
                _playerRank.color = _playerColor;
                _playerNick.color = _playerColor;
                _playerResult.color = _playerColor;
            }
        }
    }
}