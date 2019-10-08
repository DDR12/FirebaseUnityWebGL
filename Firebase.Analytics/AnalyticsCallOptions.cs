using Newtonsoft.Json;

namespace Firebase.Analytics
{
    /// <summary>
    /// Additional options that can be passed to Firebase Analytics method calls such as <see cref="FirebaseAnalytics.LogEvent(string, AnalyticsCallOptions, Parameter[])"/>, <see cref="FirebaseAnalytics.SetCurrentScreen(string, AnalyticsCallOptions)"/>, etc.
    /// </summary>
    public sealed class AnalyticsCallOptions
    {
        /// <summary>
        /// If true, this config or event call applies globally to all analytics properties on the page/app.
        /// </summary>
        [JsonProperty("global")]
        public bool Global { get; set; }
        /// <summary>
        /// Easy instance of the default looks of the recaptcha html verifier control.
        /// </summary>
        public static AnalyticsCallOptions Default => new AnalyticsCallOptions() { Global = true };
    }
}
