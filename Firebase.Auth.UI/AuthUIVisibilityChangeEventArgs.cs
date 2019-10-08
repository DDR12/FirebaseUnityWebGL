namespace Firebase.Auth.UI
{
    /// <summary>
    /// Information with the event fired when the visibility state of a native auth ui changes.
    /// </summary>
    public sealed class AuthUIVisibilityChangeEventArgs : System.EventArgs
    {
        /// <summary>
        /// Is the UI visible at the time this event args was created.
        /// </summary>
        public bool IsVisible { get; }
        /// <summary>
        /// </summary>
        /// <param name="isVisible"></param>
        public AuthUIVisibilityChangeEventArgs(bool isVisible)
        {
            IsVisible = isVisible;
        }
    }
}
