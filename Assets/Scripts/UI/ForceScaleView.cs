using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ForceScaleView : MonoBehaviour
{
    [SerializeField] private Image _sliderBackground;
    [SerializeField] private Image _sliderHandler;
    [SerializeField] private float _fadeTime;

    public void Fade()
    {
        _sliderBackground.DOFade(0, _fadeTime);
        _sliderHandler.DOFade(0, _fadeTime);
        Invoke(nameof(Disable), _fadeTime);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
