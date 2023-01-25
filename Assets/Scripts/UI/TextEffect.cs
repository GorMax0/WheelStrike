using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(TMP_Text))]
    public class TextEffect : MonoBehaviour
    {
        [SerializeField] private float _offsetY = 45f;
        [SerializeField] private bool _isInfinityLooping;

        private const int InfinityLoops = int.MaxValue;

        private RectTransform _transform;
        private Vector3 _startPosition;
        private float _alphaZero = 0f;
        private float _duration = 0.5f;
        private int _countLoops = 0;
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _transform = GetComponent<RectTransform>();
            _startPosition = transform.position;
        }

        private void OnEnable()
        {
            PlayEffect();
        }

        private void OnDisable()
        {
            KillTween();
        }

        public void Fade()
        {
            KillTween();
            _text.DOFade(_alphaZero, _duration);
        }

        public void Prepare()
        {
            _transform.position = _startPosition;
            _text.color = Color.white;
        }

        private void PlayEffect()
        {
            if (_isInfinityLooping == true)
                _countLoops = InfinityLoops;

            _transform.DOAnchorPosY(_offsetY, _duration).SetLoops(_countLoops, LoopType.Yoyo);
        }

        private void KillTween()
        {
            DOTween.Kill(_transform);
            DOTween.Kill(_text);
        }
    }
}