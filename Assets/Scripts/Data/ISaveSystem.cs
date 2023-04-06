using System.Threading.Tasks;

namespace Data
{
    public interface ISaveSystem
    {
        public void Save(GameData gameData);
        
        public Task<GameData> Load();
    }
}