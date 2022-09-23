using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(TMP_Text))]
public class TextEffect : MonoBehaviour
{
    [SerializeField] private bool _isLooping;

    private const int InfinityLoops = -1;
    
    private float _transparency = 0f;
    private float _duration = 1f;
    private int _countLoops = 0;
    private TMP_Text _text;

    public TMP_Text Text => _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        PlayEffect();
    }

    protected virtual void PlayEffect()
    {
        if (_isLooping == true)
            _countLoops = InfinityLoops;

        Text.DOFade(_transparency, _duration).SetLoops(_countLoops, LoopType.Yoyo);
    }
}
