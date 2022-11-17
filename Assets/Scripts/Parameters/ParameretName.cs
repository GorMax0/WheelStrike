using System;
using System.Collections.Generic;

namespace Parameters
{
    public static class ParameretName
    {
        private static Dictionary<ParameterType, string> _parameterName = new Dictionary<ParameterType, string>() {
        { ParameterType.Speed, "Скорость" },
        { ParameterType.Size, "Размер" },
        { ParameterType.Income, "Доход"}
    };

        public static string GetName(ParameterType type)
        {
            switch (type)
            {
                case ParameterType.Speed:
                    return _parameterName[ParameterType.Speed];
                case ParameterType.Size:
                    return _parameterName[ParameterType.Size];
                case ParameterType.Income:
                    return _parameterName[ParameterType.Income];
            }

            throw new ArgumentException($"{typeof(ParameretName)}: GetName(ParametrType type): Not available ParameterType {type}.");
        }
    }
}