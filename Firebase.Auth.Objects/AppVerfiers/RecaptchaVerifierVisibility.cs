namespace Firebase.Auth
{
    /// <summary>
    /// Possible sizes for the Recaptcha verifier html control rendered for specific auth providers.
    /// </summary>
    public enum RecaptchaVerifierVisibility
    {
        /// <summary>
        /// The recaptcha control is visible and requires the user interaction.
        /// </summary>
        Normal = 1,
        /// <summary>
        /// The recaptcha control verifies the user internally without appearing or having the user interact with it.
        /// </summary>
        Invisible = 0,
    }
}
