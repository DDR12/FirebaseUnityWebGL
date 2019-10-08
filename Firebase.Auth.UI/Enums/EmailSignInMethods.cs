namespace Firebase.Auth.UI
{
    /// <summary>
    /// Defines the possible sign in methods using EmailAuthProvider.
    /// </summary>
    public enum EmailSignInMethods
    {
        /// <summary>
        /// The usual email sign in method using an email and password.
        /// </summary>
        EmailPassword,
        /// <summary>
        /// Sign in using an email and a link sent to that email and user clicks to sign in.
        /// </summary>
        EmailLink,
    }
}
