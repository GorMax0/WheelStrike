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

        public LeaderboardYandex(string leaderboardName, int countEntries)
        {
            _leaderboardName = leaderboardName;
            _numberTopPlayers = countEntries;
        }

        public List<PlayerInfoLeaderboard> GetEntries()
        {
            List<PlayerInfoLeaderboard> topPlayers = new List<PlayerInfoLeaderboard>();

#if !UNITY_WEBGL || UNITY_EDITOR
            for (int i = 1; i <= _numberTopPlayers; i++)
            {
                topPlayers.Add(new PlayerInfoLeaderboard(i, "name", i));
            }

            return topPlayers;
#elif YANDEX_GAMES
            PlayerAccount.Authorize();

            if (PlayerAccount.IsAuthorized == true)
                PlayerAccount.RequestPersonalProfileDataPermission();

            Leaderboard.GetEntries(_leaderboardName, (result) =>
            {
                int resultsCount = result.entries.Length;

                resultsCount = Mathf.Clamp(resultsCount, 1, _numberTopPlayers);

                for (int i = 0; i < resultsCount; i++)
                {
                    string name = CheckNameForNull(result.entries[i].player.publicName);

                    int score = result.entries[i].score;

                    topPlayers.Add(new PlayerInfoLeaderboard(i, name, score));
                }
            });

            return topPlayers;
#endif
        }

        public PlayerInfoLeaderboard GetCurrentPlayer()
        {
            PlayerInfoLeaderboard player;

            player = new PlayerInfoLeaderboard(AnonymousRank, AnonymousName, AnonymousScore);
#if !UNITY_WEBGL || UNITY_EDITOR
            return player;
#elif YANDEX_GAMES
            Leaderboard.GetPlayerEntry(_leaderboardName, (result) =>
            {
                if (result == null)
                    return;

                Debug.Log($"My rank = {result.rank}");
                string name = CheckNameForNull(result.player.publicName);

                player = new PlayerInfoLeaderboard(result.rank, result.player.publicName, result.score);
            });

            return player;
#endif
        }

        public void SetPlayerScoreToLeaderboard(int score)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            return;
#elif YANDEX_GAMES
            if (PlayerAccount.IsAuthorized == false)
                return;

            Leaderboard.GetPlayerEntry(_leaderboardName, (result) =>
             {
                 if (result.score < score)
                     Leaderboard.SetScore(_leaderboardName, score);
             });
#endif
        }

        private string CheckNameForNull(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = AnonymousName;

            return name;
        }
    }
}
