namespace Leaderboard
{
    public class PlayerInfoLeaderboard
    {
        public PlayerInfoLeaderboard(int rank, string name, int score)
        {
            Rank = rank;
            Name = name;
            Score = score;
        }

        public int Rank { get; }

        public string Name { get; }

        public int Score { get; }
    }
}