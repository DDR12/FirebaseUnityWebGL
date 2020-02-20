namespace Newtonsoft.Json
{
    /// <summary>
    /// General Firebase Json serializer/deserializer settings to match the behaviour of javascript object.
    /// </summary>
    public class FirebaseJsonSettings
    {
        /// <summary>
        /// Main settings for serializing/deserializing javascript
        /// </summary>
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
        };
    }
}
