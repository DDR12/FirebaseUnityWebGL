using Newtonsoft.Json;
using System;

namespace Firebase.Analytics
{
    /// <summary>
    /// Standard gtag.js control parameters.
    /// For more information, see the gtag.js documentation on parameters.
    /// </summary>
    public sealed class ControlParams
    {
        [JsonProperty("id")]
        internal uint ParamID { get; set; }

        /// <summary>
        /// Callback function called when processing of an event command has completed.
        /// </summary>
        [JsonIgnore]
        public Action EventCallback { get; set; }

        /// <summary>
        /// Timeout used for <see cref="EventCallback"/>.
        /// </summary>
        [JsonIgnore]
        public TimeSpan EventTimeout
        {
            get => TimeSpan.FromMilliseconds(EventTimeoutMillis);
            set => EventTimeoutMillis = (ulong)value.TotalMilliseconds;
        }
        [JsonProperty("event_timeout")]
        ulong EventTimeoutMillis { get; set; }

        /// <summary>
        /// Used by the config command to assign a target to one or more groups.
        /// </summary>
        [JsonProperty("groups", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Groups { get; set; }

        /// <summary>
        /// Sets the target account/property that is to receive the event data.
        /// </summary>
        [JsonProperty("send_to", NullValueHandling = NullValueHandling.Ignore)]
        public string[] SendTo { get; set; }

    }
}
