using Newtonsoft.Json;
namespace Firebase.Auth
{
    /// <summary>
    /// Interface representing a user's metadata.
    /// </summary>
    public class UserMetadata
    {
        /// <summary>
        /// The time since unix epoch that this user was created.
        /// </summary>
        [JsonProperty("creationTime")]
        public ulong CreationTimestamp { get; set; }
        /// <summary>
        /// The last time since unix epoch that this user was created.
        /// </summary>
        [JsonProperty("lastSignInTime")]
        public ulong LastSignInTimestamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonConstructor]
        public UserMetadata() { }
    }
}