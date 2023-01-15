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

        private const float Duration = 0.15f;
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

            _rewardScaler.ZoneTransmitted += SetZone;
            _rewardScaler.CurrentValueChanged += ChangeSliderValue;
        }

        private void OnDisable()
        {
            _rewardScaler.ZoneTransmitted -= SetZone;
            _rewardScaler.CurrentValueChanged -= ChangeSliderValue;
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

        //private void SetZone(RewardZone currentZone, RewardZone previousZone)
        //{
        //    if (gameObject.activeInHierarchy == false)
        //        return;

        //    AnimationTextReward(_rates[currentZone], _rates[previousZone]);

        //    RewardZoneChanged?.Invoke(_rates[currentZone].text);
        //}

        //private void AnimationTextReward(TMP_Text currentReward, TMP_Text previousReward)
        //{
        //    DOTween.Kill(currentReward);
        //    DOTween.Kill(previousReward);
        //    DOTween.Kill(currentReward.transform);
        //    DOTween.Kill(previousReward.transform);
        //    previousReward.DOColor(_rewardSelect, Duration);
        //    previousReward.transform.DOScale(EndScale, Duration);
        //    currentReward.DOColor(_rewardDeselect, Duration);
        //    currentReward.transform.DOScale(StartScale, Duration);
        //}

        private void SetZone(RewardZone currentZone)
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
                .Append(currentReward.DOColor(_rewardSelect, Duration))
                .Join(currentReward.transform.DOScale(EndScale, Duration))
                .Append(currentReward.DOColor(_rewardDeselect, Duration))
                .Join(currentReward.transform.DOScale(StartScale, Duration));
        }

        private void ChangeSliderValue(float value) => _slider.value = value;
    }
}