namespace Firebase.Auth
{
    /// <summary>
    /// Twitter auth provider.
    /// </summary>
    public static class TwitterAuthProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public static string PROVIDER_ID => AuthPInvoke.GetTwitterProviderID_WebGL();
        /// <summary>
        /// This corresponds to the sign-in method identifier as returned in <see cref="FirebaseAuth.FetchProvidersForEmailAsync(string)"/>
        /// </summary>
        public static string TWITTER_SIGN_IN_METHOD => AuthPInvoke.GetTwitterSignInMethod_WebGL();

        /// <summary>
        /// Get firebase credentials from twitter auth provider, you can use the result to sign into firebase with <see cref="FirebaseAuth.SignInWithCredentialAsync"/>
        /// </summary>
        /// <param name="token">Twitter access token.</param>
        /// <param name="secret">Twitter secret.</param>
        /// <returns></returns>
        public static Credential GetCredential(string token, string secret)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(token, nameof(token));
            PreconditionUtilities.CheckNotNullOrEmpty(secret, nameof(secret));
            return Auth.Credential.FromJson(AuthPInvoke.TwitterProviderGetCredentials_WebGL(token, secret));
        }
    }
}
