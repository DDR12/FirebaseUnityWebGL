using Newtonsoft.Json;
using System.Collections.Generic;

namespace Firebase.Auth.UI
{
    /// <summary>
    /// Information about federated sign in option like google.com.
    /// </summary>
    public sealed class FederatedSignInOption : SignInOption
    {
        /// <summary>
        /// Sign in Method, default methods can be accessed at <see cref="AuthProviderMethods"/>
        /// </summary>
        [JsonProperty("authMethod", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthMethod { get; set; }
        /// <summary>
        /// Auth provider client id, for example the google sign in client id found in your Firebase/Authentication/SignInMethods/Google dashboard.
        /// </summary>
        [JsonProperty("clientId", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientId { get; set; }
        /// <summary>
        /// Custom parameters for the auth provider.
        /// </summary>
        [JsonProperty("customParameters", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> CustomParameters { get; set; }

        /// <summary>
        /// Custom scopes, these are data you ask the users for access for example "email" "friends", search for your provider scopes by searching google for 'facebook sign in scopes' for example.
        /// </summary>
        [JsonProperty("scopes", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Scopes { get; set; }
    }
}
