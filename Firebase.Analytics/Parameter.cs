namespace Firebase.Analytics
{
    /// <summary>
    /// Event parameter.
    /// </summary>
    public sealed class Parameter
    {
        public string Name { get; }

        public object Value { get; }
        /// <summary>
        /// Construct a string parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter. <see cref="Parameter"/> names must be a combination of letters and digits (matching the regular expression [a-zA-Z0-9]) between 1 and 40 characters long starting with a letter [a-zA-Z] character.</param>
        /// <param name="parameterValue">String value for the parameter, can be up to 100 characters long.</param>
        public Parameter(string parameterName, string parameterValue)
        {
            Name = parameterName;
            Value = parameterValue;
        }
        /// <summary>
        /// Construct a 64-bit integer parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter. <see cref="Parameter"/> names must be a combination of letters and digits (matching the regular expression [a-zA-Z0-9]) between 1 and 40 characters long starting with a letter [a-zA-Z] character.</param>
        /// <param name="parameterValue">Integer value for the parameter.</param>
        public Parameter(string parameterName, long parameterValue)
        {
            Name = parameterName;
            Value = parameterValue;
        }
        /// <summary>
        /// Construct a floating point parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter. <see cref="Parameter"/> names must be a combination of letters and digits (matching the regular expression [a-zA-Z0-9]) between 1 and 40 characters long starting with a letter [a-zA-Z] character.</param>
        /// <param name="parameterValue">Floating point value for the parameter.</param>
        public Parameter(string parameterName, double parameterValue)
        {
            Name = parameterName;
            Value = parameterValue;
        }
    }
}
