namespace Firebase.Auth
{
    /// <summary>
    /// Provider Ids for the most used providers.
    /// </summary>
    public sealed class AuthProviderIDs
    {
        /// <summary>
        /// Email Auth Provider ID.
        /// </summary>
        public static string EmailAuthProvider
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                return Auth.EmailAuthProvider.PROVIDER_ID;
#else
                return "password";
#endif
            }
        }
        /// <summary>
        /// Facebook Auth Provider ID.
        /// </summary>
        public static string FacebookAuthProvider
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                return Auth.FacebookAuthProvider.PROVIDER_ID;
#else
                return "facebook.com";
#endif
            }
        }
        /// <summary>
        /// Phone Auth Provider ID.
        /// </summary>
        public static string PhoneAuthProvider
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                return Auth.PhoneAuthProvider.PROVIDER_ID;
#else
                return "phone";
#endif
            }
        }
        /// <summary>
        /// Github Auth Provider ID.
        /// </summary>
        public static string GithubAuthProvider
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                return Auth.GithubAuthProvider.PROVIDER_ID;
#else
                return "github.com";
#endif
            }
        }
        /// <summary>
        /// Google Auth Provider ID.
        /// </summary>
        public static string GoogleAuthProvider
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                return  Auth.GoogleAuthProvider.PROVIDER_ID;
#else
                return "google.com";
#endif
            }
        }
        /// <summary>
        /// Twitter Auth Provider ID.
        /// </summary>
        public static string TwitterAuthProvider
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                return  Auth.TwitterAuthProvider.PROVIDER_ID;
#else
                return "twitter.com";
#endif
            }
        }
        /// <summary>
        /// Anonymous Auth Provider ID.
        /// </summary>
        public static string AnonymousAuthProvider => "anonymous";
        /// <summary>
        /// Microsoft Auth Provider ID.
        /// </summary>
        public static string MicrosoftAuthProvider => "microsoft.com";
    }
}
