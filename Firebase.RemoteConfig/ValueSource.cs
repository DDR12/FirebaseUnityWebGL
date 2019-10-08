namespace Firebase.RemoteConfig
{
    /// <summary>
    /// Describes the source a config value was retrieved from.
    /// </summary>
    public enum ValueSource
    {
        /// <summary>
        /// The value was not specified, so the specified default value was returned instead.
        /// </summary>
        DefaultValue,
        /// <summary>
        /// The value was not specified and no default was specified, so a static value (0 for numeric values, an empty string for strings) was returned.
        /// </summary>
        StaticValue,
        /// <summary>
        /// The value was found in the remote data store, and returned.
        /// </summary>
        RemoteValue,
    }
}