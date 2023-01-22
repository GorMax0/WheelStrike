using System;
using System.Collections.Generic;
using Lean.Localization;

namespace Parameters
{
    public static class ParameterName
    {
        private const string TurkishLanguage = "Turkish";
        private const string RussianLanguage = "Russian";
        private const string EnglishLanguage = "English";

        private static Dictionary<ParameterType, string> _parameterNameRu = new Dictionary<ParameterType, string>()
        {
            {ParameterType.Speed, "Скорость"},
            {ParameterType.Size, "Размер"},
            {ParameterType.Income, "Доход"}
        };
        private static Dictionary<ParameterType, string> _parameterNameTr = new Dictionary<ParameterType, string>()
        {
            {ParameterType.Speed, "Hız"},
            {ParameterType.Size, "Boyut"},
            {ParameterType.Income, "Gelir"}
        };
        private static Dictionary<ParameterType, string> _parameterNameEn = new Dictionary<ParameterType, string>()
        {
            {ParameterType.Speed, "Speed"},
            {ParameterType.Size, "Size"},
            {ParameterType.Income, "Income"}
        };

        public static string GetName(ParameterType type)
        {
            Dictionary<ParameterType, string> currentLanguageParameterName = GetCurrentLanguage();

            switch (type)
            {
                case ParameterType.Speed:
                    return currentLanguageParameterName[ParameterType.Speed];
                case ParameterType.Size:
                    return currentLanguageParameterName[ParameterType.Size];
                case ParameterType.Income:
                    return currentLanguageParameterName[ParameterType.Income];
            }

            throw new ArgumentException($"{typeof(ParameterName)}: GetName(ParameterType type): Not available ParameterType {type}.");
        }

        private static Dictionary<ParameterType, string> GetCurrentLanguage()
        {
            string language = LeanLocalization.GetFirstCurrentLanguage();

            switch (language)
            {
                case TurkishLanguage:
                    return _parameterNameTr;

                case RussianLanguage:
                    return _parameterNameRu;

                case EnglishLanguage:
                default:
                    return _parameterNameEn;
            }
        }
    }
}