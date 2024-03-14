using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class Wrap : MonoBehaviour
    {
        [SerializeField] private Vector2 _wrapOffset;
        [SerializeField] private float _duration;
        [SerializeField] private bool _isDisableAfterApply;

        private Vector3 _startPositoin;
        private Canvas _canvas;

        private void Start()
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        private void OnDisable()
        {
            DOTween.Kill(transform);
        }

        public void ApplyOffsetTransform()
        {
            _wrapOffset *= _canvas.transform.localScale;
            _startPositoin = transform.position;

            DOTween.Sequence()
                .Append(transform.DOMove(_startPositoin + new Vector3(_wrapOffset.x, _wrapOffset.y, 0f), _duration))
                .AppendCallback(Disable);
        }

        public void CancelOffsetTransform()
        {
            if (transform.position != _startPositoin)
                transform.DOMove(_startPositoin, _duration);
        }

        private void Disable()
        {
            if (_isDisableAfterApply)
                gameObject.SetActive(false);
        }
    }
}