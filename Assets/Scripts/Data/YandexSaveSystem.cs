using UnityEngine;
using Agava.YandexGames;
using System.Threading.Tasks;

namespace Data
{
    public class YandexSaveSystem : ISaveSystem
    {
        private GameData _gameData;
        private bool _isSaveDataReceived;

        public void Save(GameData gameData)
        {
            string data = JsonUtility.ToJson(gameData);

            PlayerAccount.SetPlayerData(data);
        }

        public async Task<GameData> Load()
        {
            PlayerAccount.GetPlayerData(OnSuccessCallback);

            while (_isSaveDataReceived == false)
            {
                await Task.Yield();
            }

            return _gameData;
        }

        private GameData ConvertJsonToGameData(string data) => string.IsNullOrEmpty(data) ? new GameData() : JsonUtility.FromJson<GameData>(data);

        private void OnSuccessCallback(string data)
        {
            _gameData = ConvertJsonToGameData(data);
            _isSaveDataReceived = true;
        }
    }
}