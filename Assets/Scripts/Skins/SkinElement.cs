using System;
using UnityEngine;
using UnityEngine.UI;

namespace Skins
{
    public class SkinElement : MonoBehaviour
    {
        [SerializeField] private Image _board;
        [SerializeField] private Sprite _spriteOn;
        [SerializeField] private Sprite _spriteOff;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _skin;

        public event Action<SkinElement> OnActivated;

        public GameObject Skin => _skin;
        
        private void OnEnable()
        {
            _button.onClick.AddListener(Activate);
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveListener(Activate);
        }

        public void Activate()
        {
            _board.sprite = _spriteOn;
            _button.interactable = false;
            _skin.SetActive(true);
            OnActivated?.Invoke(this);
        }
        
        public void Deactivate()
        {
            _board.sprite = _spriteOff;
            _button.interactable = true;
            _skin.SetActive(false);
        }
    }
}