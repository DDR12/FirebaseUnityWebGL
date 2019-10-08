namespace Firebase.RemoteConfig
{
    /// <summary>
    /// Describes the most recent fetch request status.
    /// </summary>
    public enum LastFetchStatus
    {
        /// <summary>
        /// Indicates the <see cref="FirebaseRemoteConfig"/> instance has not yet attempted to fetch config, or that SDK initialization is incomplete.
        /// </summary>
        NoFetchYet,
        /// <summary>
        /// The most recent fetch was a success, and its data is ready to be applied, if you have not already done so.
        /// </summary>
        Success,
        /// <summary>
        /// The most recent fetch request failed.
        /// </summary>
        Failure,
        /// <summary>
        /// The most recent fetch is still in progress.
        /// </summary>
        Pending,
    }
}