using Core.Wheel;
using GameAnalyticsSDK;
using Services.GameStates;
using UnityEngine;
using UnityEngine.UI;

namespace Skins
{
    public class SkinView : MonoBehaviour
    {
        [SerializeField] private SkinElement[] _elements;
        [SerializeField] private Button _closeButton;

        public int IndexOfActivatedElement;

        private GameStateService _stateService;
        private AnimationWheel _animationWheel;
        private SkinReward _skinReward;
        private SkinElement _currentSkinElement;

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Disable);
            _skinReward.OnRewarded -= SetActivatedIndex;

            for (int i = 0; i < _elements.Length - 1; i++)
            {
                _elements[i].OnActivated -= SetActivatedElement;
            }
        }

        public void Initialize(GameStateService stateService, SkinReward skinReward, AnimationWheel animationWheel)
        {
            _animationWheel = animationWheel;
            _stateService = stateService;
            _skinReward = skinReward;

            _closeButton.onClick.AddListener(Disable);
            _skinReward.OnRewarded += SetActivatedIndex;

            for (int i = 0; i < _elements.Length; i++)
            {
                _elements[i].OnActivated += SetActivatedElement;
            }
        }

        public void LoadIndex(int index)
        {
            IndexOfActivatedElement = index;

            for (int i = 0; i < _elements.Length; i++)
            {
                if (i == IndexOfActivatedElement)
                {
                    _elements[i].Activate();
                }
                else
                {
                    _elements[i].Deactivate();
                }
            }
        }

        private void Disable()
        {
            _stateService.ChangeState(GameState.Save);
            GameAnalytics.NewDesignEvent($"SelectSkin:{_currentSkinElement.Skin.name}");
            gameObject.SetActive(false);
        }

        private void SetActivatedElement(SkinElement skinElement)
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                if (_elements[i] == skinElement)
                {
                    IndexOfActivatedElement = i;
                    _animationWheel.SetMeshWheel(_elements[i].Skin);
                    _currentSkinElement = skinElement;
                }
                else
                {
                    _elements[i].Deactivate();
                }
            }
        }

        private void SetActivatedIndex(int index)
        {
            _elements[index].Activate();
        }
    }
}