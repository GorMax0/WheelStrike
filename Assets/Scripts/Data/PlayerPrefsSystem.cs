using System.Threading.Tasks;
using UnityEngine;

namespace Data
{
    public class PlayerPrefsSystem : ISaveSystem
    {
        private const string DataKey = "GameData";

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
                return JsonUtility.FromJson<GameData>(data);
            }

            await Task.Yield();

            return new GameData();
        }
    }
}