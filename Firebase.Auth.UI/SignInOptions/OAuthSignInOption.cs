using Newtonsoft.Json;
using System.Collections.Generic;

namespace Firebase.Auth.UI
{
    /// <summary>
    /// Optional options for OAuth2.0 provider sign in method, like facebook etc...
    /// </summary>
    public sealed class OAuthSignInOption : SignInOption
    {
        /// <summary>
        /// Custom color for the sign in button.
        /// </summary>
        [JsonProperty("buttonColor")]
        public string ButtonColor { get; set; }
        /// <summary>
        /// Custom parameters for the auth provider.
        /// </summary>
        [JsonProperty("customParameters", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> CustomParameters { get; set; }
        /// <summary>
        /// Link for an icon representing the auth provider logo, or any icon you want to display.
        /// </summary>
        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("loginHintKey", NullValueHandling = NullValueHandling.Ignore)]
        public string LoginHintKey { get; set; }

        /// <summary>
        /// Name of the auth provider, displayed to the users on the native UI.
        /// </summary>
        [JsonProperty("providerName", NullValueHandling = NullValueHandling.Ignore)]
        public string ProviderName { get; set; }
        /// <summary>
        /// Custom scopes, these are data you ask the users for access for example "email" "friends", search for your provider scopes by searching google for 'facebook sign in scopes' for example.
        /// </summary>
        [JsonProperty("scopes", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Scopes { get; set; }
    }
}
