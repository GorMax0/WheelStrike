using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Core.Wheel;
using UI.Views;

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

        private void OnLevelUpForMoneyButtonClicked(Parameter parameter, Action Refresh)
        {
            if (TryParameterLevelUp(parameter) == false)
                return;

            parameter.LevelUp();
            _animationWheel.ParameterUp();
            Refresh();
            ChangeInteractableLevelUpButtons();
        }

        private void OnLevelUpForAdsButtonClicked(Parameter parameter, Action Refresh)
        {            
            ShowAds(parameter);
            Refresh();
        }

        private void ShowAds(Parameter parameter)
        {
            _tempParameterForRewardAds = parameter;

#if !UNITY_WEBGL || UNITY_EDITOR
            Debug.Log("Parameter level up for ads!");
#elif YANDEX_GAMES
            Agava.YandexGames.VideoAd.Show(OnOpenCallback, OnRewardedCallback, OnCloseCallback, ErrorCallback);
#endif     
            // -------------> if(Agava.WebUtility.AdBlock.Enabled)
        }

        private void OnOpenCallback()
        {
            AudioListener.pause = true;
            AudioListener.volume = 0f;
            Time.timeScale = 0f;
        }

        private void OnRewardedCallback()
        {
            _adsRewards.EnrollParameterLevelUpReward(_tempParameterForRewardAds, _adsRewardMultiplier);
            _tempParameterForRewardAds = null;
        }    

        private void OnCloseCallback()
        {
            AudioListener.pause = false;
            AudioListener.volume = 1f;
            Time.timeScale = 1f;
        }

        private void OnErrorCallback(string message)
        {
            Debug.LogWarning(message);
            OnCloseCallback();
        }
    }
}