using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonGames.VKGames.Samples
{
    public class PlaytestingCanvas : MonoBehaviour
    {
        [SerializeField] private Text _coinsAmountText;

        private int _coinsAmount;

        public void InitializeSdkButton()
        {
            StartCoroutine(InitializeSDK());
        }

        public void ShowInterstitialButton()
        {
            Interstitial.Show();
        }

        public void ShowRewardedAdsButton()
        {
            VideoAd.Show(OnRewardedCallback);
        }

        public void InviteFriendsButton()
        {
            SocialInteraction.InviteFriends(OnRewardedCallback);
        }

        public void AddPlayerToCommunity()
        {
            Community.InviteToDungeonGamesGroup(OnRewardedCallback);
        }

        public void ShowLeaderboardButton()
        {
            Leaderboard.ShowLeaderboard(100);
        }

        public void IapTest()
        {
            InAppPurchase.BuyItem("item_id_123456", OnRewardedCallback, () => Debug.Log("alalala"));
        }

        private IEnumerator InitializeSDK()
        {
            yield return VKGamesSdk.Initialize(OnSDKInitilized);
        }

        private void OnSDKInitilized()
        {
            Debug.Log(VKGamesSdk.Initialized);
        }

        private void OnRewardedCallback()
        {
            _coinsAmount += 40;
            _coinsAmountText.text = $"{_coinsAmount} coins";
        }
    }
}