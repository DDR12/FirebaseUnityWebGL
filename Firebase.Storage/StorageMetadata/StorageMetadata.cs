using Newtonsoft.Json;
namespace Firebase.Storage
{
    /// <summary>
    /// The full set of object metadata, including read-only properties.
    /// </summary>
    public sealed class StorageMetadata : UploadMetadata
    {
        /// <summary>
        /// The associated <see cref="StorageReference"/> for which this metadata belongs.
        /// </summary>
        [JsonIgnore]
        public StorageReference Reference { get; set; }

        /// <summary>
        /// The bucket this object is contained in.
        /// </summary>
        [JsonProperty("bucket")]
        public string Bucket { get; set; }

        /// <summary>
        /// The full path of this object.
        /// </summary>
        [JsonProperty("fullPath")]
        public string FullPath { get; set; }

        /// <summary>
        /// A version String indicating what version of the <see cref="StorageReference"/>
        /// See also https://cloud.google.com/storage/docs/generations-preconditions
        /// </summary>
        [JsonProperty("generation")]
        public string Generation { get; set; }

        /// <summary>
        /// A version String indicating the version of this <see cref="StorageMetadata"/>
        /// See also https://cloud.google.com/storage/docs/generations-preconditions
        /// </summary>
        [JsonProperty("metageneration")]
        public string MetadataGeneration { get; set; }

        /// <summary>
        /// The short name of this object, which is the last component of the full path.
        /// For example, if fullPath is 'full/path/image.png', Name is 'image.png'.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The size of this object, in bytes.
        /// </summary>
        [JsonProperty("size")]
        public long SizeBytes { get; set; }

        /// <summary>
        /// A date string representing when this object was created.
        /// </summary>
        [JsonProperty("timeCreated")]
        public string TimeCreated { get; set; }

        /// <summary>
        /// A date string representing when this object was last updated.
        /// </summary>
        [JsonProperty("updated")]
        public string Updated { get; set; }

    }
}
