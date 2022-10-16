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

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _startPositoin = _transform.position;
        }

        private void OnDisable()
        {
            DOTween.Kill(_transform);
        }

        public void Roll()
        {
            _transform.DOMove(_startPositoin + _wrapOffset, _duration);
        }

        public void Unroll()
        {
            if (_transform.position != (Vector3)_startPositoin)
                _transform.DOMove(_startPositoin, _duration);
        }
    }
}