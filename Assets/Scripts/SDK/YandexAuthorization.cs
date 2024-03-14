using System;
using Agava.YandexGames;
using UnityEngine;

namespace SDK
{
    public class YandexAuthorization
    {
        public event Action Authorized;

        public void TryAuthorize()
        {
            if (PlayerAccount.IsAuthorized)
                return;

            PlayerAccount.Authorize(OnSuccessCallback, massage => Debug.LogWarning(massage));
        }

        private void OnSuccessCallback() => Authorized?.Invoke();
    }
}