namespace Firebase.Storage
{
    /// <summary>
    /// The options `List()` accepts.
    /// </summary>
    public sealed class ListOptions
    {
        /// <summary>
        /// If set, limits the total number of `prefixes` and `items` to return.
        /// The default and maximum maxResults is 1000.
        /// </summary>
        [JsonProperty("maxResults")]
        public uint MaxResults { get; set; } = 1000;

        /// <summary>
        /// The `nextPageToken` from a previous call to `list()`. If provided, listing is resumed from the previous position.
        /// </summary>
        [JsonProperty("pageToken")]
        public string PageToken { get; set; }

        /// <summary>
        /// Serialize the list options object to a JSON string.
        /// </summary>
        /// <param name="options">Options to serialize.</param>
        /// <returns>The JSON string or null if the options are null.</returns>
        public static string ToJson(ListOptions options)
        {
            if (options == null)
                return null;
            return JsonConvert.SerializeObject(options, FirebaseJsonSettings.Settings);
        }
    }
}
