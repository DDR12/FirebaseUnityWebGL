using Newtonsoft.Json;

namespace Firebase.Auth.UI
{
    /// <summary>
    /// Configure settings for the signing in with email option.
    /// </summary>
    public sealed class EmailSignInOption : SignInOption
    {
        /// <summary>
        /// Whether or not the sign in operation should all be handled from the same device.
        /// </summary>
        [JsonProperty("forceSameDevice", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ForceSameDevice { get; set; }
        
        /// <summary>
        /// Whether or not the sign in dialog should require the user to input their display name and show display name input field.
        /// </summary>
        [JsonProperty("requireDisplayName", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RequireDisplayName { get; set; }
        /// <summary>
        /// The sign in method, whether it is Email/Password or Email/Email Link.
        /// </summary>
        [JsonProperty("signInMethod", NullValueHandling = NullValueHandling.Ignore)]
        internal string SignInMethodName { get; set; }

        /// <summary>
        /// The sign in method, whether it is Email/Password or Email/Email Link.
        /// </summary>
        [JsonIgnore]
        public EmailSignInMethods SignInMethod
        {
            get
            {
                if (SignInMethodName == AuthProviderMethods.EmailLink)
                    return EmailSignInMethods.EmailLink;

                return EmailSignInMethods.EmailPassword;
            }
            set
            {
                switch (value)
                {
                    case EmailSignInMethods.EmailPassword:
                        SignInMethodName = AuthProviderMethods.EmailPassword;
                        break;
                    case EmailSignInMethods.EmailLink:
                        SignInMethodName = AuthProviderMethods.EmailLink;
                        break;
                    default:
                        break;
                }
            }
        } 
    }
}
