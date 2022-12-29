using UnityEngine;
using DG.Tweening;

namespace UI
{
    public class Wrap : MonoBehaviour
    {
        [SerializeField] private Vector2 _wrapOffset;
        [SerializeField] private float _duration;

        private Vector2 _startPositoin;
        private RectTransform _transform;
        private Canvas _canvas;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _startPositoin = _transform.position;
        }

        private void OnDisable()
        {
            DOTween.Kill(_transform);
        }

        private void Start()
        {
            _canvas = GetComponentInParent<Canvas>();
            _wrapOffset *= _canvas.transform.localScale;
        }

        public void ApplyOffsetTransform()
        {
            _startPositoin = _transform.position;
            _transform.DOMove(_startPositoin + _wrapOffset, _duration);
        }

        public void CancelOffsetTransform()
        {
            if (_transform.position != (Vector3)_startPositoin)
                _transform.DOMove(_startPositoin, _duration);
        }
    }
}