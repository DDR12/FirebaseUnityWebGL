
namespace Firebase.Auth
{
    /// <summary>
    /// Popular constant values for Sign in methods for various providers.
    /// </summary>
    public class AuthProviderMethods
    {
        /// <summary>
        /// This corresponds to the sign-in method identifier for signing in with email and password.
        /// </summary>
        public static string EmailPassword
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                return EmailAuthProvider.EMAIL_PASSWORD_SIGN_IN_METHOD;
#else
                return "password";
#endif
            }
        }
        /// <summary>
        /// This corresponds to the sign-in method identifier for signing in with email and email link.
        /// </summary>
        public static string EmailLink
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                return EmailAuthProvider.EMAIL_LINK_SIGN_IN_METHOD;
#else
                return "emailLink";
#endif
            }
        }
        /// <summary>
        /// This corresponds to the sign-in method identifier for signing in with facebook.
        /// </summary>
        public static string FacebookSignInMethod
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                return FacebookAuthProvider.FACEBOOK_SIGN_IN_METHOD;
#else
                return "facebook.com";
#endif
            }
        }
        /// <summary>
        /// This corresponds to the sign-in method identifier for signing in with github.
        /// </summary>
        public static string GitHubSignInMethod
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                return GithubAuthProvider.GITHUB_SIGN_IN_METHOD;
#else
                return "github.com";
#endif
            }
        }

        /// <summary>
        /// This corresponds to the sign-in method identifier for signing in with a google account.
        /// </summary>
        public static string GoogleSignInMethod
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                return GoogleAuthProvider.GOOGLE_SIGN_IN_METHOD;
#else
                return "google.com";
#endif
            }
        }

        /// <summary>
        /// This corresponds to the sign-in method identifier for signing in with a phone number.
        /// </summary>
        public static string PhoneSignInMethod
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                return PhoneAuthProvider.PHONE_SIGN_IN_METHOD;
#else
                return "phone";
#endif
            }
        }
        /// <summary>
        /// This corresponds to the sign-in method identifier for signing in with twitter account.
        /// </summary>
        public static string TwitternSignInMethod
        {
            get
            {

#if !UNITY_EDITOR && UNITY_WEBGL
                return TwitterAuthProvider.TWITTER_SIGN_IN_METHOD;
#else
                return "twitter.com";
#endif

            }
        }
    }
}
