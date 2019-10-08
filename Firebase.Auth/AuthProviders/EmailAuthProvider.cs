namespace Firebase.Auth
{
    /// <summary>
    /// Email and password auth provider implementation.
    /// </summary>
    public static class EmailAuthProvider
    {
        /// <summary>
        /// Email Auth Provider ID
        /// </summary>
        public static string PROVIDER_ID => AuthPInvoke.GetEmailProviderID_WebGL();
        /// <summary>
        /// This corresponds to the sign-in method identifier as returned in <see cref="FirebaseAuth.FetchProvidersForEmailAsync(string)"/>
        /// </summary>
        public static string EMAIL_LINK_SIGN_IN_METHOD => AuthPInvoke.GetEmailLinkSignInMethod_WebGL();
        /// <summary>
        /// This corresponds to the sign-in method identifier as returned in <see cref="FirebaseAuth.FetchProvidersForEmailAsync(string)"/>
        /// </summary>
        public static string EMAIL_PASSWORD_SIGN_IN_METHOD => AuthPInvoke.GetEmailPasswordSignInMethod_WebGL();
       
        /// <summary>
        /// Returns a <see cref="Auth.Credential"/> you can submit that to the API and it'll take care of converting it to an object before using it.
        /// </summary>
        /// <param name="email">User Email</param>
        /// <param name="password">User Password</param>
        /// <returns></returns>
        public static Credential GetCredential(string email, string password)
        {
            return Credential.FromJson(AuthPInvoke.GetEmailCredential_WebGL(email, password));
        }
        /// <summary>
        /// Returns a <see cref="Credential"/> you can submit that to the API and it'll take care of converting it to an object before using it.
        /// </summary>
        /// <param name="email">User Email</param>
        /// <param name="emailLink">User email link, invalid link will throw an exception.</param>
        /// <returns></returns>
        public static Credential GetCredentialWithLink(string email, string emailLink)
        {
            return Credential.FromJson(AuthPInvoke.GetEmailLinkCredential_WebGL(email, emailLink));
        }
    }
}
