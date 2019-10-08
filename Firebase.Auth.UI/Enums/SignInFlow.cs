namespace Firebase.Auth.UI
{
    /// <summary>
    /// Auth UI Sign in Flows.
    /// </summary>
    public enum SignInFlow
    {
        /// <summary>
        /// Redirects users to a dedicated sign in page, on which they pick the provider and sign in, on completion they are redirected to  <see cref="Config.SignInSuccessUrl"/> or to the app/game page if the <see cref="Config.SignInSuccessUrl"/> wasn't specified when the UI was shown.
        /// </summary>
        Redirect = 0,
        /// <summary>
        /// Shows a popup widgent in a separate pop up page, containing all selected auth providers.
        /// </summary>
        Popup = 1,
    }
}
