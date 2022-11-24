using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI.Views
{
    public class FinishView : MonoBehaviour
    {
        private Material _uiMaterial;

        private void Awake()
        {
            _uiMaterial = GetComponent<Image>().material;

            Color transparentColor = new Color(_uiMaterial.color.r,_uiMaterial.color.g,_uiMaterial.color.b, 0f);
            _uiMaterial.color = transparentColor;
        }

        private void OnEnable()
        {
            _uiMaterial.DOFade(1f, 0.5f).SetEase(Ease.InOutSine);
        }
    }
}