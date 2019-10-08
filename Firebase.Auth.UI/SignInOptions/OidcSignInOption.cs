using Newtonsoft.Json;
using System.Collections.Generic;

namespace Firebase.Auth.UI
{
    /// <summary>
    /// Optional options for Oidc provider sign in method.
    /// </summary>
    public sealed class OidcSignInOption : SignInOption
    {
        /// <summary>
        /// Custom color for the sign in button.
        /// </summary>
        [JsonProperty("buttonColor", NullValueHandling = NullValueHandling.Ignore)]
        public string ButtonColor { get; set; }
        /// <summary>
        /// Custom parameters for the auth provider.
        /// </summary>
        [JsonProperty("customParameters", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> CustomParameters { get; set; }
        /// <summary>
        /// Link for an icon representing the auth provider logo, or any icon you want to display.
        /// </summary>
        [JsonProperty("iconUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string IconUrl { get; set; }
        /// <summary>
        /// Name of the auth provider, displayed to the users on the native UI.
        /// </summary>
        [JsonProperty("providerName", NullValueHandling = NullValueHandling.Ignore)]
        public string ProviderName { get; set; }
    }
}
