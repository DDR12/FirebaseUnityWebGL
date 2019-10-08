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

        [JsonProperty("photoURL", NullValueHandling = NullValueHandling.Ignore)]
        internal string PhotoLink { get; set; }

        /// <summary>
        /// User photo URI.
        /// The photo url associated with the user, if any.
        /// </summary>
        [JsonIgnore]
        public System.Uri PhotoUrl
        {
            get => FirebaseApp.UrlStringToUri(PhotoLink);
            set => PhotoLink = FirebaseApp.UriToUrlString(value);
        }
    }
}
