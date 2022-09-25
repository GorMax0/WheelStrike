using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(TMP_Text))]
public class TextEffect : MonoBehaviour
{
    [SerializeField] private bool _isLooping;

    private const int InfinityLoops = -1;

    private float _endTransparency = 0f;
    private float _duration = 1f;
    private int _countLoops = 0;
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        PlayEffect();
    }

    private void OnDisable()
    {
        DOTween.Kill(this); //Как отключить ТВИН при выключиении объекта???
    }

    protected virtual void PlayEffect()
    {
        if (_isLooping == true)
            _countLoops = InfinityLoops;

        _text.DOFade(_endTransparency, _duration).SetLoops(_countLoops, LoopType.Yoyo);
    }
}
