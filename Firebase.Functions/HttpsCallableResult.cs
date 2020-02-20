namespace Firebase.Functions
{
    /// <summary>
    /// An <see cref="HttpsCallableResult"/> contains the result of calling an HttpsCallable.
    /// </summary>
    public sealed class HttpsCallableResult
    {
        [JsonProperty("data")]
        string RawDataJson { get; set; }

        /// <summary>
        /// The Result of the call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Data<T>()
        {
            if (string.IsNullOrWhiteSpace(RawDataJson))
                return default;
            return JsonConvert.DeserializeObject<T>(RawDataJson);
        }
    }
}
