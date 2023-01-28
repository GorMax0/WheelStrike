using System;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;

namespace Leaderboards
{
    public class LeaderboardYandex
    {
        private const int AnonymousRank = 0;
        private const int AnonymousScore = 0;
        private const string AnonymousName = "Anonymous";

        private int _numberTopPlayers;
        private string _leaderboardName;

        public Action<string, List<PlayerInfoLeaderboard>> GetEntriesComplited;
        public Action<string, PlayerInfoLeaderboard> GetPlayerEntryComplited;

        public LeaderboardYandex(string leaderboardName, int countEntries)
        {
            _leaderboardName = leaderboardName;
            _numberTopPlayers = countEntries;
        }

        public void GetEntries()
        {
            List<PlayerInfoLeaderboard> topPlayers = new List<PlayerInfoLeaderboard>(_numberTopPlayers);

#if !UNITY_WEBGL || UNITY_EDITOR
            for (int i = 1; i <= _numberTopPlayers; i++)
            {
                topPlayers.Add(new PlayerInfoLeaderboard(i, "name", i));
            }

            GetEntriesComplited?.Invoke(_leaderboardName, topPlayers);
            Debug.Log($"{_leaderboardName} - GetEntriesComplited (UNITY_EDITOR)");

#elif YANDEX_GAMES
            Leaderboard.GetEntries(_leaderboardName, (result) =>
            {
                int resultsCount = result.entries.Length;

                resultsCount = Mathf.Clamp(resultsCount, 1, _numberTopPlayers);

                for (int i = 0; i < resultsCount; i++)
                {
                    LeaderboardEntryResponse entry = result.entries[i];
                    string name = CheckNameForNull(entry.player.publicName);

                    topPlayers.Add(new PlayerInfoLeaderboard(entry.rank, name, entry.score));
                }

                Debug.Log($"{_leaderboardName} - GetEntriesComplited (Yandex)");
                GetEntriesComplited?.Invoke(_leaderboardName, topPlayers);
            }, (massage) => Debug.Log(massage), _numberTopPlayers, 0, true);
#endif
        }

        public void GetCurrentPlayer()
        {
            PlayerInfoLeaderboard player;
#if !UNITY_WEBGL || UNITY_EDITOR
            player = new PlayerInfoLeaderboard(AnonymousRank, AnonymousName, AnonymousScore);
            GetPlayerEntryComplited?.Invoke(_leaderboardName, player);
            Debug.Log($"{_leaderboardName} - GetCurrentPlayer (UNITY_EDITOR)");

#elif YANDEX_GAMES           
            TryAuthorize();
            TryGetPersonalData();

            Leaderboard.GetPlayerEntry(_leaderboardName, (result) =>
            {
                string name = CheckNameForNull(result.player.publicName);

                player = new PlayerInfoLeaderboard(result.rank, result.player.publicName, result.score);
                Debug.Log($"{_leaderboardName} - GetPlayerEntryComplited (Yandex)");
                GetPlayerEntryComplited?.Invoke(_leaderboardName, player);
            });
#endif
        }

        public void SetScore(int score)
        {
#if YANDEX_GAMES   
            if (PlayerAccount.IsAuthorized == false)
                return;

            Leaderboard.GetPlayerEntry(_leaderboardName, result =>
            {
                Debug.Log($"SetScore {result.score} new score {score}, name {result.player.publicName}");

                if (result.score < score)
                    Leaderboard.SetScore(_leaderboardName, score);
            });
#endif
        }

        private void TryAuthorize()
        {
            if (PlayerAccount.IsAuthorized == true)
                return;

            PlayerAccount.Authorize();
        }

        private void TryGetPersonalData()
        {
            if (PlayerAccount.IsAuthorized == true && PlayerAccount.HasPersonalProfileDataPermission == false)
                PlayerAccount.RequestPersonalProfileDataPermission();
        }

        private string CheckNameForNull(string name) => string.IsNullOrEmpty(name) ? AnonymousName : name;
    }
}