using Agava.YandexGames;
using GameAnalyticsSDK;
using Lean.Localization;

namespace Services
{
    public static class Localization
    {
        private static string _localizationName;

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

            GameAnalytics.NewDesignEvent($"Language:{_localizationName}");
        }

        private static string GetLanguageEnvironment() => string.IsNullOrEmpty(_localizationName) ? _localizationName = YandexGamesSdk.Environment.i18n.lang : _localizationName;
    }
}