using System.Collections.Generic;
using Parameters;

public class ParameterCreater 
{
    private Dictionary<ParameterType, Parameter> _parameters;

    public Dictionary<ParameterType, Parameter> CreateParameters(ParameterObject[] _parameterObject)
    {
        _parameters = new Dictionary<ParameterType, Parameter>();

        for (int i = 0; i < _parameterObject.Length; i++)
        {
            _parameters.Add(_parameterObject[i].Type, new Parameter(_parameterObject[i]));
        }

        return _parameters;
    }
}
