namespace Firebase.Auth
{
    /// <summary>
    /// Use an ID token and access token provided by Google to authenticate.
    /// </summary>
    public static class GoogleAuthProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public static string PROVIDER_ID => AuthPInvoke.GetGoogleProviderID_WebGL();
        /// <summary>
        /// This corresponds to the sign-in method identifier as returned in <see cref="FirebaseAuth.FetchProvidersForEmailAsync(string)"/>
        /// </summary>
        public static string GOOGLE_SIGN_IN_METHOD => AuthPInvoke.GetGoogleSignInMethod_WebGL();

        /// <summary>
        /// Creates a credential for Google. At least one of ID token and access token is required.
        /// </summary>
        /// <param name="idToken">Google ID token.</param>
        /// <param name="accessToken">Google access token.</param>
        /// <returns></returns>
        public static Credential GetCredential(string idToken = null, string accessToken = null)
        {
            if (string.IsNullOrEmpty(idToken) && string.IsNullOrEmpty(accessToken))
                throw new System.ArgumentNullException($"{nameof(idToken)}, {nameof(accessToken)}", "are both null, at least id token or access token must be provided.");

            return Credential.FromJson(AuthPInvoke.GetGoogleCredential_WebGL(idToken, accessToken));
        }
    }
}
