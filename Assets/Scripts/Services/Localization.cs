using System;
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
        private static string _currentLanguage;

        public static event Action<string> LanguageChanged;

        public static string CurrentLanguage => _currentLanguage;

        public static void GetCurrentLanguage()
        {
            _currentLanguage = LeanLocalization.GetFirstCurrentLanguage();
            LanguageChanged?.Invoke(_currentLanguage);
        }

        public static void SetLanguage()
        {
            switch ("en")
            {
                case "en":
                    _currentLanguage = EnglishLanguage;
                    LeanLocalization.SetCurrentLanguageAll(_currentLanguage);
                    break;
            }

            GameAnalytics.NewDesignEvent($"Language:{_language}");

        }

        public static void SwitchOnEnglish()
        {
            _currentLanguage = EnglishLanguage;
            LeanLocalization.SetCurrentLanguageAll(_currentLanguage);
            GameAnalytics.NewDesignEvent($"guiClick:Language:{_currentLanguage}");
            LanguageChanged?.Invoke(_currentLanguage);
        }

        public static void SwitchOnRussian()
        {
            _currentLanguage = RussianLanguage;
            LeanLocalization.SetCurrentLanguageAll(_currentLanguage);
            GameAnalytics.NewDesignEvent($"guiClick:Language:{_currentLanguage}");
            LanguageChanged?.Invoke(_currentLanguage);
        }

        public static void SwitchOnTurkish()
        {
            _currentLanguage = TurkishLanguage;
            LeanLocalization.SetCurrentLanguageAll(_currentLanguage);
            GameAnalytics.NewDesignEvent($"guiClick:Language:{_currentLanguage}");
            LanguageChanged?.Invoke(_currentLanguage);
        }
    }
}