using Newtonsoft.Json;
namespace Firebase.Auth
{

    /// <summary>
    /// Result of operations that can affect authentication state.
    /// </summary>
    public class SignInResult
    {
        /// <summary>
        /// Additional info about the user.
        /// </summary>
        [JsonProperty("additionalUserInfo")]
        public AdditionalUserInfo Info { get; set; }
  
        /// <summary>
        /// User meta data.
        /// </summary>
        [JsonIgnore]
        public UserMetadata Meta => User?.Metadata;

        //[JsonProperty("credential")]
        //public OAuthCredential Credential { get; set; }
        ///// <summary>
        ///// Type of the auth operation that this credential is a result of.
        ///// operationType could be 'signIn' for a sign-in operation, 'link' for a linking operation and 'reauthenticate' for a reauthentication operation.
        ///// </summary>
        //[JsonProperty("operationType")]
        //public string OperationType { get; set; }

        /// <summary>
        /// The user account, can be null.
        /// </summary>
        //[JsonProperty("user")] // Commented out so Json doesn't serialize the real user object in the UserCredential object, but rather our wrapper object
        public FirebaseUser User { get; set; }
    }
}
