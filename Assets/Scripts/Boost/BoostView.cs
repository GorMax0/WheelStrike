using System.Collections.Generic;
using GameAnalyticsSDK;
using Lean.Localization;
using Parameters;
using Services.GameStates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Boost
{
    public class BoostView : MonoBehaviour
    {
        private const int Percent = 100;

        [SerializeField] private TMP_Text _descriptionDefault;
        [SerializeField] private TMP_Text _descriptionMaximumBoost;
        [SerializeField] private TMP_Text _buttonDefaultText;
        [SerializeField] private TMP_Text _buttonTextMaximumBoost;
        [SerializeField] private LeanToken _levelToken;
        [SerializeField] private LeanToken _speedToken;
        [SerializeField] private LeanToken _incomeToken;
        [SerializeField] private Button _button;

        private GameStateService _gameStateService;
        private BoostParameter _boost;
        private Dictionary<ParameterType, Parameter> _parameters;
        private Dictionary<ParameterType, bool> _maximumParameters;
        private bool _isInitialized;

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _button.onClick.AddListener(ApplyBoost);
            _levelToken.SetValue(_boost.Level);

            if (_boost.Level < BoostParameter.MaxLevel)
            {
                _speedToken.SetValue((_boost.SpeedMultiplier + BoostParameter.BaseSpeedValue) * Percent);
                _incomeToken.SetValue((_boost.IncomeMultiplier + BoostParameter.BaseIncomeValue) * Percent);
            }
            else
            {
                _speedToken.SetValue(_boost.SpeedMultiplier * Percent);
                _incomeToken.SetValue(_boost.IncomeMultiplier * Percent);
                _descriptionDefault.gameObject.SetActive(false);
                _descriptionMaximumBoost.gameObject.SetActive(true);
                _buttonDefaultText.gameObject.SetActive(false);
                _buttonTextMaximumBoost.gameObject.SetActive(true);
                _button.interactable = false;
            }
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ApplyBoost);
        }

        public void Initialize(
            GameStateService gameStateService,
            BoostParameter boost,
            Dictionary<ParameterType, Parameter> parameters)
        {
            _gameStateService = gameStateService;
            _boost = boost;
            _parameters = parameters;
            _maximumParameters = new Dictionary<ParameterType, bool>(_parameters.Count);

            foreach (KeyValuePair<ParameterType, Parameter> parameter in _parameters)
            {
                _maximumParameters.Add(parameter.Key, false);
                parameter.Value.Loaded += CheckMaximumLevelParameters;
            }

            _isInitialized = true;
            OnEnable();
        }

        public void Open()
        {
            gameObject.SetActive(true);
            GameAnalytics.NewDesignEvent("guiClick:Boost");
        }

        public void HasMaximumLevelParameter(Parameter parameter)
        {
            _maximumParameters[parameter.Type] = parameter.Level >= parameter.MaximumLevel;
            EnableButton();
        }

        private void EnableButton()
        {
            if (_boost.Level >= BoostParameter.MaxLevel)
                return;

            if (_maximumParameters[ParameterType.Speed] && _maximumParameters[ParameterType.Size] &&
                _maximumParameters[ParameterType.Income])
            {
                _buttonDefaultText.alpha = 1;
                _button.interactable = true;
            }
        }

        private void CheckMaximumLevelParameters()
        {
            foreach (KeyValuePair<ParameterType, Parameter> parameter in _parameters)
            {
                _maximumParameters[parameter.Value.Type] = parameter.Value.Level >= parameter.Value.MaximumLevel;
            }

            EnableButton();
        }

        private void ApplyBoost()
        {
            _boost.LevelUp();

            foreach (KeyValuePair<ParameterType, Parameter> parameter in _parameters)
            {
                parameter.Value.Reset();
            }

            _gameStateService.ChangeState(GameState.ApplyBoost);
        }
    }
}