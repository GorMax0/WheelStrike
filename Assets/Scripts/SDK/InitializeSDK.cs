using System.Collections;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.SceneManagement;
using Data;
using Agava.YandexGames;

public class InitializeSDK : MonoBehaviour
{
    private const string DataKey = "GameData";

    private int _levelIndex;

    private void Awake()
    {

        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield return new WaitForSeconds(0.1f);
#elif YANDEX_GAMES
        while(YandexGamesSdk.IsInitialized == false)
        {
            yield return YandexGamesSdk.Initialize();
        }

      //  YandexGamesSdk.CallbackLogging = true;
#endif
        yield return GetLevelIndex();

        GameAnalytics.Initialize();
        SceneManager.LoadScene(_levelIndex);
    }

    private IEnumerator GetLevelIndex()
    {
        GameData gameData = null;

        if (PlayerPrefs.HasKey(DataKey))
        {
            string data = PlayerPrefs.GetString(DataKey);

            gameData = JsonUtility.FromJson<GameData>(data);
        }
#if !UNITY_WEBGL || UNITY_EDITOR
#elif YANDEX_GAMES
        else
        {
            PlayerAccount.GetPlayerData((string data) =>
            {
                gameData = ConvertJsonToGameData(data);
            });
        }
#endif
        yield return new WaitForSeconds(1f);
        _levelIndex = gameData == null ? SceneManager.GetActiveScene().buildIndex + 1 : gameData.IndexScene;
    }

    private GameData ConvertJsonToGameData(string data) => string.IsNullOrEmpty(data) ? null : JsonUtility.FromJson<GameData>(data);
}
