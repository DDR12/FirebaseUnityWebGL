using System;
using Newtonsoft.Json;

namespace Firebase.Auth
{
    /// <summary>
    /// User profile information, visible only to the Firebase project's
    /// </summary>
    public class UserInfo : IUserInfo
    {
        /// <summary>
        /// Gets the unique user ID for the user.
        /// </summary>
        [JsonProperty("uid")]
        public virtual string UserId { get; set; }
        /// <summary>
        /// Gets the display name associated with the user, if any.
        /// </summary>
        [JsonProperty("displayName")]
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// Gets email associated with the user, if any.
        /// </summary>
        [JsonProperty("email")]
        public virtual string Email { get; set; }
        /// <summary>
        /// Gets the provider ID for the user (For example, "Facebook").
        /// </summary>
        [JsonProperty("providerId")]
        public virtual string ProviderId { get; set; }

        /// <summary>
        /// Gets the photo url associated with the user, if any.
        /// </summary>
        [JsonProperty("photoURL")]
        public virtual Uri PhotoUrl { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [JsonConstructor]
        public UserInfo() { }
    }
}