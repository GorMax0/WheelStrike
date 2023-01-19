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
        [SerializeField] private bool _isInfinityLooping;

        private const int InfinityLoops = int.MaxValue;

        private RectTransform _transform;
        private float _endTransparency = 0f;
        private float _duration = 0.5f;
        private int _countLoops = 0;
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _transform = GetComponent<RectTransform>();
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
            _text.DOFade(_endTransparency, _duration);
        }

        private void PlayEffect()
        {
            if (_isInfinityLooping == true)
                _countLoops = InfinityLoops;

            _transform.DOAnchorPosY(45f, _duration).SetLoops(_countLoops, LoopType.Yoyo);
        }

        private void KillTween()
        {
            DOTween.Kill(_transform);
            DOTween.Kill(_text);
        }
    }
}