namespace Firebase.RemoteConfig
{
    /// <summary>
    /// Describes the most recent fetch failure.
    /// </summary>
    public enum FetchFailureReason
    {
        /// <summary>
        /// The fetch has not yet failed.
        /// </summary>
        Invalid,
        /// <summary>
        /// The most recent fetch failed because it was throttled by the server.
        /// (You are sending too many fetch requests in too short a time.)
        /// </summary>
        Throttled,
        /// <summary>
        /// The most recent fetch failed for an unknown reason.
        /// </summary>
        Error,
    }
}