namespace Firebase.Auth
{
    /// <summary>
    /// Use an access token provided by Facebook to authenticate.
    /// </summary>
    public static class FacebookAuthProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public static string PROVIDER_ID => AuthPInvoke.GetFacebookProviderID_WebGL();

        /// <summary>
        /// This corresponds to the sign-in method identifier as returned in <see cref="FirebaseAuth.FetchProvidersForEmailAsync(string)"/>
        /// </summary>
        public static string FACEBOOK_SIGN_IN_METHOD => AuthPInvoke.GetFacebookSignInMethod_WebGL();

        /// <summary>
        /// Generate a credential from the given Facebook token.
        /// </summary>
        /// <param name="accessToken">Facebook access token.</param>
        /// <returns></returns>
        public static Credential GetCredential(string accessToken)
        {
            return Credential.FromJson(AuthPInvoke.GetFacebookCredential_WebGL(accessToken));
        }
    }
}
