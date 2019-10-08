using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Firebase.RemoteConfig
{
    /// <summary>
    /// Wrapper for a Remote Config parameter value, with methods to get it as different types, such as bools and doubles, along with information about where the data came from.
    /// </summary>
    public struct ConfigValue
    {
        private static Regex booleanTruePattern;

        private static Regex booleanFalsePattern;
        static ConfigValue()
        {
            booleanTruePattern = new Regex("^(1|true|t|yes|y|on)$", RegexOptions.IgnoreCase);
            booleanFalsePattern = new Regex("^(0|false|f|no|n|off|)$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Gets the value as a bool.
        /// </summary>
        [JsonIgnore]
        public bool BooleanValue
        {
            get
            {
                string stringValue = this.StringValue;
                if (booleanTruePattern.IsMatch(stringValue))
                {
                    return true;
                }
                if (!booleanFalsePattern.IsMatch(stringValue))
                {
                    throw new FormatException(string.Format("ConfigValue '{0}' is not a boolean value", stringValue));
                }
                return false;
            }
        }
        /// <summary>
        /// Gets the value as an IEnumerable.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<byte> ByteArrayValue => Encoding.UTF8.GetBytes(StringValue);

        /// <summary>
        /// Gets the value as a double.
        /// </summary>
        [JsonIgnore]
        public double DoubleValue => Convert.ToDouble(StringValue);
        /// <summary>
        /// Gets the value as a long.
        /// </summary>
        [JsonIgnore]
        public long LongValue => Convert.ToInt64(StringValue);
        /// <summary>
        /// Indicates which source this value came from.
        /// </summary>
        [JsonProperty("_source"), JsonConverter(typeof(JavaScriptToCSharpEnumConverter))]
        public ValueSource Source { get; private set; }
        /// <summary>
        /// Gets the value as a string.
        /// </summary>
        [JsonProperty("_value")]
        public string StringValue { get; private set; }

    }
}
