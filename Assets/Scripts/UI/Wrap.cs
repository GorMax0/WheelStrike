using UnityEngine;
using DG.Tweening;

namespace UI
{
    public class Wrap : MonoBehaviour
    {
        [SerializeField] private Vector2 _wrapOffset;
        [SerializeField] private float _duration;
        [SerializeField] private bool _isDisableAfterApply;

        private Vector2 _startPositoin;
        private Canvas _canvas;

        private void OnDisable()
        {
            DOTween.Kill(transform);
        }

        private void Start()
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        public void ApplyOffsetTransform()
        {
            _wrapOffset *= _canvas.transform.localScale;
            _startPositoin = transform.position;

            DOTween.Sequence()
                .Append(transform.DOMove(_startPositoin + _wrapOffset, _duration))
                .AppendCallback(Disable);
        }

        public void CancelOffsetTransform()
        {
            if (transform.position != (Vector3)_startPositoin)
                transform.DOMove(_startPositoin, _duration);
        }

        private void Disable()
        {
            if (_isDisableAfterApply == true)            
                gameObject.SetActive(false);   
        }
    }
}