using Newtonsoft.Json;

namespace Firebase.Storage
{
    /// <summary>
    /// Holds base data about the current state of the upload/download task.
    /// </summary>
    public abstract class BaseTransferState
    {
        /// <summary>
        /// The number of bytes that have been successfully transferred so far.
        /// </summary>
        [JsonProperty("bytesTransferred")]
        public ulong BytesTransferred { get; set; }
        /// <summary>
        /// The total number of bytes to be transferred.
        /// </summary>
        [JsonProperty("totalBytes")]
        public ulong TotalBytes { get; set; }

        /// <summary>
        /// Current progress percent between 0-100
        /// </summary>
        [JsonIgnore]
        public float Progress => ProgressNormalized * 100;
        /// <summary>
        /// Current raw progress 0-1
        /// </summary>
        [JsonIgnore]
        public float ProgressNormalized => BytesTransferred / (float)TotalBytes;

        [JsonIgnore]
        StorageReference reference = null;
        [JsonIgnore]
        public virtual StorageReference Reference
        {
            get => reference;
            set => reference = value;
        }
        
    }
}
