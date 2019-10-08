using Newtonsoft.Json;
namespace Firebase.Auth.UI
{
    /// <summary>
    /// Mandatory settings for any sign in option.
    /// </summary>
    public class SignInOption
    {
        /// <summary>
        /// The Provider ID for this sign in option.
        /// </summary>
        [JsonProperty("provider", NullValueHandling = NullValueHandling.Ignore)]
        public string Provider { get; set; }
        
        /// <summary>
        /// Convert a provider ID to a sign in option.
        /// </summary>
        /// <param name="providerID"></param>
        public static implicit operator SignInOption(string providerID) => new SignInOption() { Provider = providerID };
    }
}
