using Authorization;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class AuthorizationView : MonoBehaviour
    {
        [SerializeField] private Button _authorizationButton;

        private YandexAuthorization _yandexAuthorization;

        private void OnEnable()
        {
            _authorizationButton.onClick.AddListener(_yandexAuthorization.TryAuthorize);
        }

        private void OnDisable()
        {
            _authorizationButton.onClick.RemoveListener(_yandexAuthorization.TryAuthorize);
        }

        public void Initialize(YandexAuthorization yandexAuthorization)
        {
            _yandexAuthorization = yandexAuthorization;
        }
    }
}