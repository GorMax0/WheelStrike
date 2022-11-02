using System;
using System.Collections.Generic;

namespace Parameters
{
    public static class ParameretName
    {
        private static Dictionary<ParametrType, string> _parameterName = new Dictionary<ParametrType, string>() {
        { ParametrType.Speed, "Скорость" },
        { ParametrType.Size, "Размер" },
        { ParametrType.Income, "Доход"}
    };

        public static string GetName(ParametrType type)
        {
            switch (type)
            {
                case ParametrType.Speed:
                    return _parameterName[ParametrType.Speed];
                case ParametrType.Size:
                    return _parameterName[ParametrType.Size];
                case ParametrType.Income:
                    return _parameterName[ParametrType.Income];
            }

            throw new ArgumentException($"{typeof(ParameretName)}: GetName(ParametrType type): Not available ParameterType {type}.");
        }
    }
}