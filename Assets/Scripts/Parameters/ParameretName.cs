using System;
using System.Collections.Generic;

public static class ParameretName
{
    private static Dictionary<ParametrType, string> _parameterName = new Dictionary<ParametrType, string>() {
        { ParametrType.Power, "Сила" },
        { ParametrType.Size, "Размер" },
        { ParametrType.Income, "Доход"}
    };

    public static string GetName(ParametrType type)
    {
        switch (type)
        {
            case ParametrType.Power:
                return _parameterName[ParametrType.Power];
            case ParametrType.Size:
                return _parameterName[ParametrType.Size];
            case ParametrType.Income:
                return _parameterName[ParametrType.Income];                
        }

        throw new ArgumentException($"{typeof(ParameretName)}: GetName(ParametrType type): Not available ParameterType {type}.");
    }
}
