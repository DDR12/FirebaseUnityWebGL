using Newtonsoft.Json;

namespace Firebase.Auth.UI
{
    /// <summary>
    /// Configure settings for the signing in with SAML.
    /// </summary>
    public sealed class SamlSignInOption : SignInOption
    {
        /// <summary>
        /// Sign in Button Color.
        /// </summary>
        [JsonProperty("buttonColor", NullValueHandling = NullValueHandling.Ignore)]
        public string ButtonColor { get; set; }
        /// <summary>
        /// Url of an icon to display on the login form.
        /// </summary>
        [JsonProperty("iconUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string IconUrl { get; set; }
        /// <summary>
        /// Name of the SAML provider, displayed on the login form.
        /// </summary>
        [JsonProperty("providerName", NullValueHandling = NullValueHandling.Ignore)]
        public string ProviderName { get; set; }
    }
}
