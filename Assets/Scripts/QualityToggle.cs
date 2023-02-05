using System;
using GameAnalyticsSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Services
{
    [RequireComponent(typeof(Toggle))]
    public class QualityToggle : MonoBehaviour
    {
        [SerializeField] private Image _lowFPS;
        [SerializeField] private TMP_Text _lowFPSText;

        private Toggle _qualityToggle;
        private Color _lowQuality = new Color(0.745f, 0f, 0f, 1f);
        private Color _highQuality = Color.white;

        public event Action<bool> QualityChanged;

        private void Awake()
        {
            _qualityToggle = GetComponent<Toggle>();
        }

        private void OnEnable()
        {
            _qualityToggle.onValueChanged.AddListener(SwitchQuality);
        }

        private void OnDisable()
        {
            _qualityToggle.onValueChanged.RemoveListener(SwitchQuality);
        }

        public void LoadSelectedQuality(bool isNormalQuality)
        {
            _qualityToggle.isOn = isNormalQuality;
            SwitchQuality(isNormalQuality);
        }

        private void SwitchQuality(bool isOn)
        {
            _lowFPS.color = isOn == true ? _highQuality : _lowQuality;
            _lowFPSText.color = isOn == true ? _highQuality : _lowQuality;
            QualityChanged?.Invoke(isOn);
            GameAnalytics.NewDesignEvent($"guiClick:FPSToggle:{isOn}");
        }
    }
}