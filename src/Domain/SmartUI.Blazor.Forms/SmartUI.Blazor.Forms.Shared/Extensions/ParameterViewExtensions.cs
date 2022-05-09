namespace Microsoft.AspNetCore.Components
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ParameterViewExtensions
    {
        public static ParameterView EnsureOnlyOneParameterIsSet(this ParameterView parameters, string[] parametersNames)
        {
            foreach (string parameterName in parametersNames)
            {
                parameters.TryGetValue(parameterName, out object parameterValue);

                if (parameterValue != default)
                {
                    parameters = parameters.Remove(parametersNames.Where(n => n != parameterName).ToArray());
                    break;
                }
            }

            return parameters;
        }

        public static ParameterView Remove(this ParameterView parameters, string parameterName) => parameters.Remove(new[] { parameterName });

        public static ParameterView Remove(this ParameterView parameters, string[] parameterNames)
        {
            Dictionary<string, object> parametersDictionary = parameters.ToDictionary().ToDictionary(x => x.Key, x => x.Value);

            foreach (string name in parameterNames)
                parametersDictionary.Remove(name);

            return ParameterView.FromDictionary(parametersDictionary);
        }
    }
}