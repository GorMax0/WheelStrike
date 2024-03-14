using System;
using Agava.YandexGames;
using GameAnalyticsSDK;
using Lean.Localization;

namespace Services
{
    public static class Localization
    {
        private const string TurkishLanguage = "Turkish";
        private const string RussianLanguage = "Russian";
        private const string EnglishLanguage = "English";

        private static string _language;

        public static string CurrentLanguage { get; private set; }

        public static event Action<string> LanguageChanged;

        public static void GetCurrentLanguage()
        {
            CurrentLanguage = LeanLocalization.GetFirstCurrentLanguage();
            LanguageChanged?.Invoke(CurrentLanguage);
        }

        public static void SetLanguage()
        {
#if !UNITY_WEBGL || UNITY_EDITOR

#elif YANDEX_GAMES
            switch (GetLanguageEnvironment())
            {
                case "en":
                    _currentLanguage = EnglishLanguage;
                    LeanLocalization.SetCurrentLanguageAll(_currentLanguage);
                    break;
                case "tr":
                    _currentLanguage = TurkishLanguage;
                    LeanLocalization.SetCurrentLanguageAll(_currentLanguage);
                    break;
                case "ru":
                    _currentLanguage = RussianLanguage;
                    LeanLocalization.SetCurrentLanguageAll(_currentLanguage);
                    break;
                default:
                    _currentLanguage = EnglishLanguage;
                    LeanLocalization.SetCurrentLanguageAll(_currentLanguage);
                    break;
            }

            GameAnalytics.NewDesignEvent($"Language:{_language}");
#endif
        }

        public static void SwitchOnEnglish()
        {
            CurrentLanguage = EnglishLanguage;
            LeanLocalization.SetCurrentLanguageAll(CurrentLanguage);
            GameAnalytics.NewDesignEvent($"guiClick:Language:{CurrentLanguage}");
            LanguageChanged?.Invoke(CurrentLanguage);
        }

        public static void SwitchOnRussian()
        {
            CurrentLanguage = RussianLanguage;
            LeanLocalization.SetCurrentLanguageAll(CurrentLanguage);
            GameAnalytics.NewDesignEvent($"guiClick:Language:{CurrentLanguage}");
            LanguageChanged?.Invoke(CurrentLanguage);
        }

        public static void SwitchOnTurkish()
        {
            CurrentLanguage = TurkishLanguage;
            LeanLocalization.SetCurrentLanguageAll(CurrentLanguage);
            GameAnalytics.NewDesignEvent($"guiClick:Language:{CurrentLanguage}");
            LanguageChanged?.Invoke(CurrentLanguage);
        }

        private static string GetLanguageEnvironment() => string.IsNullOrEmpty(_language)
            ? _language = YandexGamesSdk.Environment.i18n.lang
            : _language;
    }
}