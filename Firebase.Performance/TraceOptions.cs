using System.Collections.Generic;

namespace Firebase.Performance
{
    /// <summary>
    /// Container for custom metrics and custom attributes to use when recording a trace.
    /// </summary>
    public sealed class TraceOptions
    {
        /// <summary>
        /// Custom attributes, if null will be ignored.
        /// </summary>
        [JsonProperty("attributes")]
        public Dictionary<string, string> Attributes { get; set; }
        /// <summary>
        /// Custom metrics, if null will be ignored.
        /// </summary>
        [JsonProperty("metrics")]
        public Dictionary<string, int> Metrics { get; set; }

        public static string ToJson(TraceOptions traceOptions)
        {
            if (traceOptions == null)
                return null;
            return JsonConvert.SerializeObject(traceOptions, FirebaseJsonSettings.Settings);
        }
    }
}
