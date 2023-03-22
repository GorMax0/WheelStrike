using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Data;
using GameAnalyticsSDK;
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
        GameAnalytics.Initialize();
#if !UNITY_WEBGL || UNITY_EDITOR
        yield return new WaitForSeconds(0.1f);
        yield return GetLevelIndex();
        LoadScene();
#elif YANDEX_GAMES
        while (YandexGamesSdk.IsInitialized == false)
        {
            yield return YandexGamesSdk.Initialize();
        }

        Services.Localization.SetLanguage();
        yield return GetLevelIndex();
        InterstitialAd.Show(OnOpenCallback, OnCloseCallback, OnErrorCallback, OnOfflineCallback);
        //  YandexGamesSdk.CallbackLogging = true;
#endif
    }

    private IEnumerator GetLevelIndex()
    {
        GameData gameData = null;

#if !UNITY_WEBGL || UNITY_EDITOR
#elif YANDEX_GAMES
        if (PlayerAccount.IsAuthorized == true)
        {
            PlayerAccount.GetPlayerData((string data) =>
            {
                gameData = ConvertJsonToGameData(data);
            });
        }
        else
#endif
        if (PlayerPrefs.HasKey(DataKey))
        {
            string data = PlayerPrefs.GetString(DataKey);

            gameData = JsonUtility.FromJson<GameData>(data);
        }

        yield return new WaitForSeconds(1f);

        _levelIndex = gameData == null ? SceneManager.GetActiveScene().buildIndex + 1 : gameData.IndexScene;
    }

    private GameData ConvertJsonToGameData(string data) => string.IsNullOrEmpty(data) ? null : JsonUtility.FromJson<GameData>(data);

    private void LoadScene() => SceneManager.LoadScene(_levelIndex);

    private void OnOpenCallback() => GameAnalytics.NewDesignEvent("AdClick:InterstitialAds:InitializeSDK");

    private void OnCloseCallback(bool isClose) => LoadScene();

    private void OnErrorCallback(string error)
    {
        Debug.LogWarning(error);
        LoadScene();
    }

    private void OnOfflineCallback() => LoadScene();
}
