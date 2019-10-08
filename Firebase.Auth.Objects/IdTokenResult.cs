using Newtonsoft.Json;

namespace Firebase.Auth
{
    /// <summary>
    /// Representing ID token result obtained from FirebaseUser.GetIdTokenResultAsync.
    /// It contains the ID token JWT string and other helper properties for getting different data associated with the token as well as all the decoded payload claims.
    /// Note that these claims are not to be trusted as they are parsed client side.
    /// Only server side verification can guarantee the integrity of the token claims.
    /// </summary>
    public sealed class IdTokenResult
    {
        /// <summary>
        /// The authentication time formatted as a UTC string. This is the time the user authenticated (signed in) and not the time the token was refreshed.
        /// </summary>
        [JsonProperty("authTime")]
        public string AuthTime { get; set; }

        /// <summary>
        /// The ID token expiration time formatted as a UTC string.
        /// </summary>
        [JsonProperty("expirationTime")]
        public string ExpirationTime { get; set; }

        /// <summary>
        /// The ID token issued at time formatted as a UTC string.
        /// </summary>
        [JsonProperty("issuedAtTime")]
        public string IssuedAtTime { get; set; }

        /// <summary>
        /// The sign-in provider through which the ID token was obtained (anonymous, custom, phone, password, etc). 
        /// Note, this does not map to provider IDs.
        /// </summary>
        [JsonProperty("signInProvider")]
        public string SignInProvider { get; set; }

        /// <summary>
        /// The Firebase Auth ID token JWT string.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
