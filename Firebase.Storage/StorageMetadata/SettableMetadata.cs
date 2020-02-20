using System.Collections.Generic;

namespace Firebase.Storage
{
    /// <summary>
    /// Object metadata that can be set at any time.
    /// </summary>
    public class SettableMetadata
    {
        /// <summary>
        /// Served as the 'Cache-Control' header on object download.
        /// </summary>
        [JsonProperty("cacheControl")]
        public string CacheControl { get; set; }

        /// <summary>
        /// Gets or sets the content disposition for the <see cref="StorageReference"/>.
        /// </summary>
        [JsonProperty("contentDisposition")]
        public string ContentDisposition { get; set; }


        /// <summary>
        /// Served as the 'Content-Encoding' header on object download.
        /// </summary>
        [JsonProperty("contentEncoding")]
        public string ContentEncoding { get; set; }

        /// <summary>
        /// Served as the 'Content-Language' header on object download.
        /// </summary>
        [JsonProperty("contentLanguage")]
        public string ContentLanguage { get; set; }

        /// <summary>
        /// Served as the 'Content-Type' header on object download.
        /// </summary>
        [JsonProperty("contentType")]
        public string ContentType { get; set; }


        /// <summary>
        /// Gets or sets custom metadata.
        /// </summary>
        [JsonProperty("customMetadata")]
        public Dictionary<string,object> CustomMetadata { get; set; }

        /// <summary>
        /// Returns custom metadata for a <see cref="StorageReference"/> if exists.
        /// </summary>
        /// <param name="key">The key for which the metadata should be returned</param>
        /// <returns>The metadata stored in the object with the given key.</returns>
        public object GetCustomMetadata(string key)
        {
            if (CustomMetadata == null)
                return null;
            if (CustomMetadata.TryGetValue(key, out object metadata))
                return metadata;
            return null;
        }
    }
}
