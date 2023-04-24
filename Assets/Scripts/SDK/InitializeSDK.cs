using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Data;
using GameAnalyticsSDK;
using Agava.YandexGames;

public class InitializeSDK : MonoBehaviour
{
    private const string DataVersion = "v0.4.01";
    private const string DataKey = "GameData";

    private int _levelIndex;
    private GameData _gameData;

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
#elif YANDEX_GAMES
        while (YandexGamesSdk.IsInitialized == false)
        {
            yield return YandexGamesSdk.Initialize();
        }

        Services.Localization.SetLanguage();
        yield return GetLevelIndex();
  //      InterstitialAd.Show(OnOpenCallback, OnCloseCallback, OnErrorCallback, OnOfflineCallback);

       //  YandexGamesSdk.CallbackLogging = true;
#endif

        LoadScene();
    }


    private IEnumerator GetLevelIndex()
    {
        LoadGameData();

        yield return new WaitForSeconds(1f);

        if (_gameData == null || _gameData.DataVersion != DataVersion)
        {
            _gameData = new GameData(DataVersion);
            PlayerPrefs.DeleteAll();
        }

        _levelIndex = _gameData.IndexScene;

#if !UNITY_WEBGL || UNITY_EDITOR
        Save();
#elif YANDEX_GAMES
        SaveDataYandex();
#endif
    }

    private void LoadGameData()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
#elif YANDEX_GAMES
        if (PlayerAccount.IsAuthorized == true)
        {
            PlayerAccount.GetPlayerData((string data) =>
            {
                _gameData = ConvertJsonToGameData(data);
            });
        }
        else
#endif
        if (PlayerPrefs.HasKey(DataKey))
        {
            string data = PlayerPrefs.GetString(DataKey);
            _gameData = ConvertJsonToGameData(data);
        }
    }

    private GameData ConvertJsonToGameData(string data) =>
        string.IsNullOrEmpty(data) ? null : JsonUtility.FromJson<GameData>(data);

    private void SaveDataYandex()
    {
        string data = JsonUtility.ToJson(_gameData);
        PlayerAccount.SetPlayerData(data);
    }

    private void Save()
    {
        string data = JsonUtility.ToJson(_gameData);

        PlayerPrefs.SetString(DataKey, data);
        PlayerPrefs.Save();
    }

    private void LoadScene() => SceneManager.LoadScene(_levelIndex);

    private void OnOpenCallback() => GameAnalytics.NewDesignEvent("AdClick:InterstitialAds:InitializeSDK");

    private void OnCloseCallback(bool _) => LoadScene();

    private void OnErrorCallback(string error)
    {
        Debug.LogWarning(error);
        LoadScene();
    }

    private void OnOfflineCallback() => LoadScene();
}