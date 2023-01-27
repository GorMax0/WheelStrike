namespace Leaderboards
{
    public class PlayerInfoLeaderboard
    {
        public string Name { get; private set; }
        public int Rank { get; private set; }
        public int Score { get; private set; }

        public PlayerInfoLeaderboard(int rank, string name, int score)
        {
            Rank = rank;
            Name = name;
            Score = score;
        }
    }
}