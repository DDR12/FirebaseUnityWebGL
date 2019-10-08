using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Firebase.Auth
{
    /// <summary>
    /// A structure containing additional user information from a federated identity provider.
    /// </summary>
    public sealed class AdditionalUserInfo : IDisposable
    {
        /// <summary>
        /// Additional identity-provider specific information.
        /// </summary>
        [JsonProperty("profile")]
        public Dictionary<string,object> Profile { get; set; }

        /// <summary>
        /// The provider identifier.
        /// </summary>
        [JsonProperty("providerId")]
        public string ProviderId { get; set; }


        /// <summary>
        /// The name of the user.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }
    }
}