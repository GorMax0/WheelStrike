namespace Data
{
    public interface ISaveSystem
    {
        public void Save(GameData gameData);
        public GameData Load();
    }
}