using Newtonsoft.Json;

namespace Firebase
{
    internal class WrapperError
    {
        [JsonProperty("error")]
        public FirebaseError Error { get; set; }

        [JsonConstructor]

        public WrapperError() { }
        public static FirebaseError FromJson(string json) => JsonConvert.DeserializeObject<WrapperError>(json).Error;
    }
    /// <summary>
    /// 
    /// </summary>
    public class FirebaseError
    {
        /// <summary>
        /// The error name or code in a string format.
        /// </summary>
        [JsonProperty("code")]
        public string ErrorName { get; private set; }
        /// <summary>
        /// An explanatory message for the error that just occurred.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonConstructor]
        public FirebaseError() { }

        /// <summary>
        /// The error code.
        /// </summary>
        public int ErrorCode
        {
            get
            {
                if (int.TryParse(ErrorName, out int code))
                    return code;
                return ErrorName.GetHashCode();
            }
        }
        /// <summary>
        /// Get a <see cref="FirebaseError"/> from a JSON string.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static FirebaseError FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;
            try
            {
                return JsonConvert.DeserializeObject<FirebaseError>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}
