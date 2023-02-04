using System;
using UnityEngine;
using Agava.YandexGames;

namespace Authorization
{
    public class YandexAuthorization
    {
        public event Action Authorized;

        public void TryAuthorize()
        {
            if (PlayerAccount.IsAuthorized == true)
                return;

            PlayerAccount.Authorize(OnSuccessCallback, (string massage) => Debug.LogWarning(massage));
        }

        private void OnSuccessCallback() => Authorized?.Invoke();
    }
}