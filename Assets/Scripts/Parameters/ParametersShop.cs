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
        private Parameter _parameterForRewardAds;
        private int _adsRewardMultiplier = 3;

        private Action _refreshView;

        private void OnDestroy()
        {
            foreach (var view in _views.Values)
            {
                view.LevelUpForMoneyButtonClicked -= OnLevelUpForMoneyButtonClicked;
                view.LevelUpForAdsButtonClicked -= OnLevelUpForAdsButtonClicked;
            }

            _wallet.MoneyLoaded -= ChangeInteractableLevelUpButtons;
            _wallet.MoneyChanged -= ChangeInteractableLevelUpButtons;
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
            _wallet.MoneyLoaded += ChangeInteractableLevelUpButtons;
            _wallet.MoneyChanged += ChangeInteractableLevelUpButtons;
        }

        public void ChangeInteractableLevelUpButtons(int moneyInWallet)
        {
            foreach (var view in _views)
            {
                var moneyToBuy = HasMoneyToBuy(view.Value.Parameter, moneyInWallet);
                view.Value.ChangeStateButton(moneyToBuy);
            }
        }

        private bool TryParameterLevelUp(Parameter parameter) => _wallet.TrySpandMoney(parameter.Cost);

        private bool HasMoneyToBuy(Parameter parameter, int moneyInWallet) => moneyInWallet >= parameter.Cost;

        private void OnLevelUpForMoneyButtonClicked(Parameter parameter, Action onRefresh)
        {
            if (TryParameterLevelUp(parameter) == false)
                return;

            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "Money", parameter.Cost, "ParameterShop", $"{parameter.Type}");
            parameter.LevelUp();
            _animationWheel.ParameterUp();
            ChangeInteractableLevelUpButtons(_wallet.Money);
            onRefresh();
            GameAnalytics.NewDesignEvent($"ParameterUp:{parameter.Type}", parameter.Level);
        }

        private void OnLevelUpForAdsButtonClicked(Parameter parameter, Action onRefresh)
        {
            _refreshView = onRefresh;
            ShowAds(parameter);
        }

        private void ShowAds(Parameter parameter)
        {
            _parameterForRewardAds = parameter;

#if !UNITY_WEBGL || UNITY_EDITOR
            Debug.Log("Parameter level up for ads!");
            _adsRewards.EnrollParameterLevelUpReward(_parameterForRewardAds, _adsRewardMultiplier);
            _refreshView();

#elif YANDEX_GAMES
            Agava.YandexGames.VideoAd.Show(OnOpenCallback, OnRewardedCallback, OnCloseCallback, OnErrorCallback);
#endif     
        }

        private void PauseOn()
        {
            SoundController.ChangeWhenAd(true);
            Time.timeScale = 0f;
        }

        private static void PauseOff()
        {
            SoundController.ChangeWhenAd(false);
            Time.timeScale = 1f;
        }

        private void OnOpenCallback()
        {
            PauseOn();
            GameAnalytics.NewDesignEvent("AdClick:ParameterLevelUp");
        }

        private void OnRewardedCallback()
        {
            _adsRewards.EnrollParameterLevelUpReward(_parameterForRewardAds, _adsRewardMultiplier);
            _parameterForRewardAds = null;
            _refreshView();
        }

        private void OnCloseCallback() => PauseOff();

        private void OnErrorCallback(string message)
        {
            Debug.LogWarning(message);
            PauseOff();
        }
    }
}