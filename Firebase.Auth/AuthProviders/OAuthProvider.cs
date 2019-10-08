namespace Firebase.Auth
{
    /// <summary>
    /// OAuth2.0+UserInfo auth provider (OIDC compliant and non-compliant).
    /// </summary>
    public static class OAuthProvider
    {
        /// <summary>
        /// Generate a credential for an OAuth2 provider.
        /// </summary>
        /// <param name="providerID">Name of the OAuth2 provider.</param>
        /// <param name="idToken">The OAuth ID token if OIDC compliant.</param>
        /// <param name="accessToken">The OAuth access token.</param>
        /// <returns></returns>
        public static Credential GetCredential(string providerID, string idToken = null, string accessToken = null)
        {
            return Credential.FromJson(AuthPInvoke.GetOAuthCredential_WebGL(providerID, idToken, accessToken));
        }
    }
}
