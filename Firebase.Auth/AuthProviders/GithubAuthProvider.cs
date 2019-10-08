namespace Firebase.Auth
{
    /// <summary>
    /// Use an access token provided by GitHub to authenticate.
    /// </summary>
    public static class GithubAuthProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public static string PROVIDER_ID => AuthPInvoke.GetGithubProviderID_WebGL();

        /// <summary>
        /// This corresponds to the sign-in method identifier as returned in <see cref="FirebaseAuth.FetchProvidersForEmailAsync(string)"/>
        /// </summary>
        public static string GITHUB_SIGN_IN_METHOD => AuthPInvoke.GetGithubSignInMethod_WebGL();
        
        /// <summary>
        /// Generate a credential from the given GitHub token. <see cref="FirebaseAuth.SignInWithCredentialAsync"/>
        /// </summary>
        /// <param name="token">The github sign in token.</param>
        /// <returns>A credential object in json format, used to sign into firebase auth service.</returns>
        public static Credential GetCredential(string token) => Auth.Credential.FromJson(AuthPInvoke.GetGithubCredential_WebGL(token));
    }
}
