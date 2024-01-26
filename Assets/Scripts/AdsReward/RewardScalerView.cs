using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

namespace AdsReward
{
    [RequireComponent(typeof(Slider))]
    public class RewardScalerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _texts;
        [SerializeField] private Color _rewardSelect;
        [SerializeField] private Color _rewardDeselect = Color.white;

        private const float SelectDuration = 0.15f;
        private const float DeselectDuration = 0.65f;
        private const float StartScale = 1f;
        private const float EndScale = 1.3f;

        private Slider _slider;
        private RewardScaler _rewardScaler;
        private Dictionary<RewardZone, TMP_Text> _rates;
        private bool _isInitialized = false;

        public event Action<string> RewardZoneChanged;

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _rewardScaler.ZoneTransmitted += OnSetZone;
            _rewardScaler.CurrentValueChanged += OnChangeSliderValue;
        }

        private void OnDisable()
        {
            _rewardScaler.ZoneTransmitted -= OnSetZone;
            _rewardScaler.CurrentValueChanged -= OnChangeSliderValue;
        }

        public void Initialize(RewardScaler rewardScaler)
        {
            if (_isInitialized == true)
                return;

            _slider = GetComponent<Slider>();
            _rewardScaler = rewardScaler;
            _rates = FillInRates();

            _isInitialized = true;
            OnEnable();
        }

        private Dictionary<RewardZone, TMP_Text> FillInRates()
        {
            Dictionary<RewardZone, TMP_Text> rates = new Dictionary<RewardZone, TMP_Text>();

            if (_texts.Length == 0)
                throw new ArgumentOutOfRangeException($"{GetType()}: Dictionary<RewardZone, TMP_Text> FillInRates(): _texts does not contain elements.");

            for (int i = 0; i < _texts.Length; i++)
            {
                rates.Add((RewardZone)i, _texts[i]);
            }

            return rates;
        }

        private void OnSetZone(RewardZone currentZone)
        {
            if (gameObject.activeInHierarchy == false)
                return;

            AnimationTextReward(_rates[currentZone]);

            RewardZoneChanged?.Invoke(_rates[currentZone].text);
        }

        private void AnimationTextReward(TMP_Text currentReward)
        {
            DOTween.Kill(currentReward);
            DOTween.Kill(currentReward.transform);

            DOTween.Sequence()
                .Append(currentReward.DOColor(_rewardSelect, SelectDuration))
                .Join(currentReward.transform.DOScale(EndScale, SelectDuration))
                .Append(currentReward.DOColor(_rewardDeselect, DeselectDuration))
                .Join(currentReward.transform.DOScale(StartScale, DeselectDuration));
        }

        private void OnChangeSliderValue(float value) => _slider.value = value;
    }
}