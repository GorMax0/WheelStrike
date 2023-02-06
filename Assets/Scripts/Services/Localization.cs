using Agava.YandexGames;
using GameAnalyticsSDK;
using Lean.Localization;

namespace Services
{
    public static class Localization
    {
        private static string _language;

        public static void SetLanguage()
        {
            switch (GetLanguageEnvironment())
            {
                case "en":
                    LeanLocalization.SetCurrentLanguageAll("English");
                    break;
                case "tr":
                    LeanLocalization.SetCurrentLanguageAll("Turkish");
                    break;
                case "ru":
                    LeanLocalization.SetCurrentLanguageAll("Russian");
                    break;
                default:
                    LeanLocalization.SetCurrentLanguageAll("English");
                    break;
            }

            GameAnalytics.NewDesignEvent($"Language:{_language}");
        }

        private static string GetLanguageEnvironment() => string.IsNullOrEmpty(_language) ? _language = YandexGamesSdk.Environment.i18n.lang : _language;
    }
}