using Newtonsoft.Json;

namespace Firebase.Auth
{
    /// <summary>
    /// Parameters to the <see cref="FirebaseUser.UpdateUserProfileAsync"/> function.
    /// For fields you don't want to update, pass NULL. For fields you want to reset, pass ""
    /// </summary>
    public sealed class UserProfile
    {
        /// <summary>
        /// Gets or sets the display name associated with the user.
        /// </summary>
        [JsonProperty("displayName", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }
        /// <summary>
        /// User photo URI.
        /// The photo url associated with the user, if any.
        /// </summary>
        [JsonProperty("photoURL", NullValueHandling = NullValueHandling.Ignore)]
        public System.Uri PhotoUrl { get; set; }
    }
}
