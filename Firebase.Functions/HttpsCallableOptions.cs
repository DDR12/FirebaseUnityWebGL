namespace Firebase.Functions
{
    /// <summary>
    /// Optional options used while creating a <see cref="HttpsCallableReference"/> to a cloud function.
    /// </summary>
    public class HttpsCallableOptions
    {
        [JsonProperty("timeout")]
        public uint Timeout { get; set; }
    }
}
