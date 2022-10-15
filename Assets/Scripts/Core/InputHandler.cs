using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Core
{
    public class InputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool _handleInput = true;

        public event UnityAction PointerDown;
        public event UnityAction PointerUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_handleInput == true)
                PointerDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_handleInput == true)
                PointerUp?.Invoke();

            _handleInput = false;
        }
    }
}