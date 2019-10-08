namespace Firebase.Auth
{
    /// <summary>
    /// User state persistence types, in the browser.
    /// </summary>
    public enum PersistenceType
    {
        /// <summary>
        /// Indicates that the state will only be stored in memory and will be cleared when the window or activity is refreshed.
        /// </summary>
        None,
        /// <summary>
        /// Indicates that the state will persist even when the browser window is closed or the activity is destroyed in react-native.
        /// This is the default value.
        /// </summary>
        Local,
        /// <summary>
        /// Indicates that the state will persist relative to current session/tab only, relevant to web only, and will be cleared when the tab is closed.
        /// </summary>
        Session,
    }
}
