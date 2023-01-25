using System;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
using Core;
using Core.Wheel;
using UI.Views;
using Services;

namespace Parameters
{
    public class ParametersShop : MonoBehaviour
    {
        [SerializeField] private ParameterView _template;
        [SerializeField] private AnimationWheel _animationWheel;
        [SerializeField] private AdsReward.AdsRewards _adsRewards;

        private Dictionary<ParameterType, ParameterView> _views = new Dictionary<ParameterType, ParameterView>();
        private Wallet _wallet;
        private Parameter _tempParameterForRewardAds;
        private int _adsRewardMultiplier = 3;

        private event Action<string> ErrorCallback;

        private void OnDestroy()
        {
            foreach (var view in _views.Values)
            {
                view.LevelUpForMoneyButtonClicked -= OnLevelUpForMoneyButtonClicked;
                view.LevelUpForAdsButtonClicked -= OnLevelUpForAdsButtonClicked;
            }
        }

        public void Initialize(Dictionary<ParameterType, Parameter> parametrs, Wallet wallet)
        {
            foreach (KeyValuePair<ParameterType, Parameter> parametr in parametrs)
            {
                ParameterView view = Instantiate(_template, transform);
                view.LevelUpForMoneyButtonClicked += OnLevelUpForMoneyButtonClicked;
                view.LevelUpForAdsButtonClicked += OnLevelUpForAdsButtonClicked;
                view.Renger(parametr.Value, _adsRewardMultiplier);
                view.SubscribeToLevelChange();
                _views.Add(parametr.Key, view);
            }

            _wallet = wallet;
            ErrorCallback += OnErrorCallback;
        }

        public void ChangeInteractableLevelUpButtons()
        {
            foreach (var view in _views)
            {
                view.Value.ChangeStateButton(HasMoneyToBuy(view.Value.Parameter));
            }
        }

        private bool TryParameterLevelUp(Parameter parameter) => _wallet.TrySpandMoney(parameter.Cost);

        private bool HasMoneyToBuy(Parameter parameter) => _wallet.Money >= parameter.Cost;

        private void OnLevelUpForMoneyButtonClicked(Parameter parameter, Action onRefresh)
        {
            if (TryParameterLevelUp(parameter) == false)
                return;

            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "Money", parameter.Cost, "ParameterShop", $"{parameter.Type}");
            parameter.LevelUp();
            _animationWheel.ParameterUp();
            onRefresh();
            ChangeInteractableLevelUpButtons();
            GameAnalytics.NewDesignEvent($"ParameterUp:{parameter.Type}", parameter.Level);
        }

        private void OnLevelUpForAdsButtonClicked(Parameter parameter, Action onRefresh)
        {            
            ShowAds(parameter, onRefresh);
        }

        private void ShowAds(Parameter parameter, Action onRefresh)
        {
            _tempParameterForRewardAds = parameter;

#if !UNITY_WEBGL || UNITY_EDITOR
            Debug.Log("Parameter level up for ads!");
#elif YANDEX_GAMES
            Agava.YandexGames.VideoAd.Show(OnOpenCallback, OnRewardedCallback(onRefresh), OnCloseCallback, ErrorCallback);
#endif     
            // -------------> if(Agava.WebUtility.AdBlock.Enabled
        }

        private void OnOpenCallback()
        {
            SoundController.ChangeWhenAd(true);
            Time.timeScale = 0f;
            GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.RewardedVideo, "YandexSDK", $"Yandex");
            GameAnalytics.NewDesignEvent("AdClick:ParameterLevelUp");
        }

        private Action OnRewardedCallback(Action onRefresh)
        {
            _adsRewards.EnrollParameterLevelUpReward(_tempParameterForRewardAds, _adsRewardMultiplier);
            _tempParameterForRewardAds = null;
            GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, "YandexSDK", $"Yandex");
            onRefresh();

            return null;
        }    

        private void OnCloseCallback()
        {
            SoundController.ChangeWhenAd(false);
            Time.timeScale = 1f;            
        }

        private void OnErrorCallback(string message)
        {
            Debug.LogWarning(message);
            OnCloseCallback();
            GameAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.RewardedVideo, "YandexSDK", $"Yandex");
        }
    }
}