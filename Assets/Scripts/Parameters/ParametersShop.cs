using System;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
using Core;
using Core.Wheel;
using UI.Views;
using Boost;
using CrazyGames;

namespace Parameters
{
    public class ParametersShop : MonoBehaviour
    {
        [SerializeField] private ParameterView _template;
        [SerializeField] private AnimationWheel _animationWheel;
        [SerializeField] private AdsReward.AdsRewards _adsRewards;
        [SerializeField] private BoostView _boostView;

        private const int AdsRewardMultiplier = 5;
        private readonly Dictionary<ParameterType, ParameterView> _views = new Dictionary<ParameterType, ParameterView>();

        private Wallet _wallet;
        private CounterParameterLevel _counterParameterLevel;
        private Parameter _parameterForRewardAds;

        private bool _hasOpenVideoAd;

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

        public void Initialize(Dictionary<ParameterType, Parameter> parameters, Wallet wallet, CounterParameterLevel counterParameterLevel)
        {
            foreach (KeyValuePair<ParameterType, Parameter> parameter in parameters)
            {
                ParameterView view = Instantiate(_template, transform);
                view.LevelUpForMoneyButtonClicked += OnLevelUpForMoneyButtonClicked;
                view.LevelUpForAdsButtonClicked += OnLevelUpForAdsButtonClicked;
                view.Render(parameter.Value, AdsRewardMultiplier);
                view.SubscribeToLevelChange();
                _views.Add(parameter.Key, view);
            }

            _wallet = wallet;
            _wallet.MoneyLoaded += ChangeInteractableLevelUpButtons;
            _wallet.MoneyChanged += ChangeInteractableLevelUpButtons;

            _counterParameterLevel = counterParameterLevel;
        }

        private void ChangeInteractableLevelUpButtons(int moneyInWallet)
        {
            foreach (var view in _views)
            {
                var moneyToBuy = HasMoneyToBuy(view.Value.Parameter, moneyInWallet);
                view.Value.ChangeStateButton(moneyToBuy);
            }
        }

        private bool TryParameterLevelUp(Parameter parameter) => _wallet.TrySpendMoney(parameter.Cost);

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
            _counterParameterLevel.CheckAchievement(parameter.Type);
            _boostView.HasMaximumLevelParameter(parameter);
            GameAnalytics.NewDesignEvent($"ParameterUp:{parameter.Type}");
        }

        private void OnLevelUpForAdsButtonClicked(Parameter parameter, Action onRefresh)
        {
            _refreshView = onRefresh;
            ShowAds(parameter);
        }

        private void ShowAds(Parameter parameter)
        {
            _parameterForRewardAds = parameter;
            CrazyAds.Instance.beginAdBreakRewarded(OnCompletedCallback, OnErrorCallback);
        }

        private void OnCompletedCallback()
        {
            GameAnalytics.NewDesignEvent($"AdClick:ParameterLevelUp:{_parameterForRewardAds.Type}:Complete");
            _counterParameterLevel.CheckAchievement(_parameterForRewardAds.Type, AdsRewardMultiplier);
            _adsRewards.EnrollParameterLevelUpReward(_parameterForRewardAds, AdsRewardMultiplier);
            _refreshView();
            _boostView.HasMaximumLevelParameter(_parameterForRewardAds);
            _parameterForRewardAds = null;
        }

        private void OnErrorCallback()
        {
            GameAnalytics.NewDesignEvent($"AdClick:ParameterLevelUp:{_parameterForRewardAds.Type}:Error");
            _adsRewards.ShowErrorAds();
        }
    }
}