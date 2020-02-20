namespace Firebase.Storage
{
    /// <summary>
    /// Holds data about the current state of the upload task.
    /// </summary>
    public sealed class UploadState : BaseTransferState
    {
        /// <summary>
        /// Before the upload completes, contains the metadata sent to the server.
        /// After the upload completes, contains the metadata sent back from the server.
        /// </summary>
        [JsonProperty("metadata")]
        public StorageMetadata Metadata { get; set; }

        [JsonIgnore]
        public override StorageReference Reference
        {
            get => base.Reference;
            set
            {
                base.Reference = value;
                Metadata.Reference = value;
            }
        }
    }
}
