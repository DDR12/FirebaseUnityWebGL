namespace Firebase.Auth.UI
{
    /// <summary>
    /// Credential helpers act as managers, when users sign into your app with multiple accounts on the same browsers, thus helping them to choose which account to sign in with if they're signed out.
    /// </summary>
    public static class CredentialHelper
    {
        /// <summary>
        /// Supported of credential helpers.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// No credential helper is used on the auth ui, user will be prompted to sign in with the selected sign in method.
            /// </summary>
            None = 0,
            /// <summary>
            /// Google Yolo (You only sign once) is a web widget for "One-tap sign-up and auto sign-in on websites" by Google. your users can authenticate with their Google account or other supported providers on your firebase project, in one simple click.
            /// </summary>
            GoogleYolo = 1,
            /// <summary>
            /// If the user has multiple accounts saved on the browser for your app/game page, and you use Account Chooser as the Credential helper, users will be navigated to a different page to select the account to sign in with.
            /// </summary>
            AccountChooser = 2,
        }

        internal static string NONE
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                    return AuthUIPInvoke.GetCredentialHelperString_WebGL((int)Type.None);
#else
                return "none";
#endif
            }
        }
        internal static string GoogleYolo
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                    return AuthUIPInvoke.GetCredentialHelperString_WebGL((int)Type.GoogleYolo);
#else
                return "googleyolo";
#endif
            }
        }
        internal static string AccountChooser
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                    return AuthUIPInvoke.GetCredentialHelperString_WebGL((int)Type.AccountChooser);
#else
                return "accountchooser.com";
#endif
            }
        }
    }
}
