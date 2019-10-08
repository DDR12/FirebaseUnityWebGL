using Newtonsoft.Json;

namespace Firebase.Analytics
{
    /// <summary>
    /// Specifies custom options for your <see cref="FirebaseAnalytics"/> instance. You must set these before initializing <see cref="FirebaseAnalytics.DefaultInstance"/>.
    /// </summary>
    public sealed class SettingsOptions
    {
        /// <summary>
        /// Sets custom name for dataLayer array used by gtag.
        /// </summary>
        [JsonProperty("dataLayerName", NullValueHandling = NullValueHandling.Ignore)]
        public string DataLayerName { get; set; }

        /// <summary>
        /// Sets custom name for gtag function.
        /// </summary>
        [JsonProperty("gtagName", NullValueHandling = NullValueHandling.Ignore)]
        public string GTagName { get; set; }
    }
}
