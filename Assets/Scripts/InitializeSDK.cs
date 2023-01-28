using System.Collections;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.SceneManagement;
using Data;
using Agava.YandexGames;

public class InitializeSDK : MonoBehaviour
{
    private const string DataKey = "GameData";

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

        GameAnalytics.Initialize();
        SceneManager.LoadScene(GetLevelIndex());
    }

    private int GetLevelIndex()
    {
        if (PlayerPrefs.HasKey(DataKey))
        {
            string data = PlayerPrefs.GetString(DataKey);

            GameData gameData = JsonUtility.FromJson<GameData>(data);
            return gameData.IndexScene;
        }

        return SceneManager.GetActiveScene().buildIndex + 1;
    }
}
