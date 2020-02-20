namespace Firebase.Storage
{
    /// <summary>
    /// Object metadata that can be set at upload.
    /// </summary>
    public class UploadMetadata : SettableMetadata
    {
        /// <summary>
        /// A Base64-encoded MD5 hash of the object being uploaded.
        /// </summary>
        [JsonProperty("md5Hash")]
        public string Md5Hash { get; set; }


        public static string ToJson(UploadMetadata metadata)
        {
            if (metadata == null)
                return null;
            return JsonConvert.SerializeObject(metadata, FirebaseJsonSettings.Settings);
        }
    }
}
