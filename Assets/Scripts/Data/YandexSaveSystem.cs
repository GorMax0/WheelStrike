using System.Threading.Tasks;
using UnityEngine;
using Agava.YandexGames;

namespace Data
{
    public class YandexSaveSystem : ISaveSystem
    {
        private string _dataVersion;
        private GameData _gameData;
        private bool _isSaveDataReceived;

        public YandexSaveSystem(string dataVersion)
        {
            _dataVersion = dataVersion;
        }

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

        private GameData ConvertJsonToGameData(string data) => string.IsNullOrEmpty(data) ? new GameData(_dataVersion) : JsonUtility.FromJson<GameData>(data);

        private void CheckVersion()
        {
            if (_gameData.DataVersion == _dataVersion)
                return;

            _gameData = new GameData(_dataVersion);
        }

        private void OnSuccessCallback(string data)
        {
            _gameData = ConvertJsonToGameData(data);
            CheckVersion();
            _isSaveDataReceived = true;
        }
    }
}