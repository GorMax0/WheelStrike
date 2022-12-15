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

        public GameData Load()
        {
            if (PlayerPrefs.HasKey(DataKey))
            {
                string data = PlayerPrefs.GetString(DataKey);
                return JsonUtility.FromJson<GameData>(data);
            }
            
            return null;
        }
    }
}