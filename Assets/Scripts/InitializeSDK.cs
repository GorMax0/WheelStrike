using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeSDK : MonoBehaviour
{
    public event Action Initialized;

    private void Awake()
    {        
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield return new WaitForSeconds(0.1f);

#elif YANDEX_GAMES
        while(Agava.YandexGames.YandexGamesSdk.IsInitialized == false)
        {
            yield return Agava.YandexGames.YandexGamesSdk.Initialize();
        }
#endif
        SceneManager.LoadScene(1);
    }
}
