using Newtonsoft.Json;

namespace Firebase.Auth
{
    /// <summary>
    /// Authentication credentials for an authentication provider.
    /// </summary>
    public sealed class Credential
    {
        /// <summary>
        /// The authentication provider ID for the credential. For example, 'facebook.com', or 'google.com'.
        /// </summary>
        [JsonProperty("providerId")]
        public string Provider { get; set; }

        /// <summary>
        /// The authentication sign in method for the credential. For example, 'password', or 'emailLink. This corresponds to the sign-in method identifier as returned in
        /// </summary>
        [JsonProperty("signInMethod")]
        public string SignInMethod { get; set; }

        internal string FullJson { get; set; }

        [JsonConstructor]

        private Credential()
        {

        }

        /// <summary>
        /// Creates a credential object from a JSON string.
        /// </summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns></returns>
        public static Credential FromJson(string json)
        {
            Credential instance = JsonConvert.DeserializeObject<Credential>(json);
            instance.FullJson = json;
            return instance;
        }

        /// <summary>
        /// Checks if a credential is usable or not.
        /// </summary>
        /// <param name="credential"></param>
        public static void CheckIsEmpty(Credential credential)
        {
            PreconditionUtilities.CheckNotNull(credential, nameof(credential));
            PreconditionUtilities.CheckNotNullOrEmpty(credential.FullJson, nameof(credential));
        }
    }
}
