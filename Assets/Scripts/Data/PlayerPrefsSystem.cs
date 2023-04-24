using System.Threading.Tasks;
using UnityEngine;

namespace Data
{
    public class PlayerPrefsSystem : ISaveSystem
    {
        private const string DataKey = "GameData";

        private readonly string _dataVersion;
        private GameData _gameData;

        public PlayerPrefsSystem(string dataVersion)
        {
            _dataVersion = dataVersion;
        }

        public void Save(GameData gameData)
        {
            string data = JsonUtility.ToJson(gameData);

            PlayerPrefs.SetString(DataKey, data);
            PlayerPrefs.Save();
        }

        public async Task<GameData> Load()
        {
            if (PlayerPrefs.HasKey(DataKey))
            {
                string data = PlayerPrefs.GetString(DataKey);
                _gameData = JsonUtility.FromJson<GameData>(data);
                CheckVersion();
                return _gameData;
            }

            await Task.Yield();

            return new GameData(_dataVersion);
        }

        private void CheckVersion()
        {
            if (_gameData.DataVersion == _dataVersion)
                return;
            
            PlayerPrefs.DeleteAll();
            _gameData = new GameData(_dataVersion);
        }
    }
}