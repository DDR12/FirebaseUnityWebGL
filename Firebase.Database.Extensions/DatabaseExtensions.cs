namespace Firebase.Database
{
    /// <summary>
    /// This class helps match different platform sdks, when an sdk is missing a method but the other provides it, this class is a meeting point which both implementations can be accessed using same code instead of the #if #else directive nightmare.
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Returns the snapshot data in a specific type, if the value is not of this type, an exception is thrown.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="snapshot">The snapshot whose value to get.</param>
        /// <returns>The converted value or error if type doesn't match the data layout.</returns>
        public static T GetValue<T>(this DataSnapshot snapshot)
        {
            return JsonConvert.DeserializeObject<T>(snapshot.GetRawJsonValue());
        }
        /// <summary>
        /// Returns the snapshot data in a specific type, if the value is not of this type, an exception is thrown.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="mutableData">The snapshot whose value to get.</param>
        /// <returns>The converted value or error if type doesn't match the data layout.</returns>
        public static T GetValue<T>(this MutableData mutableData)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return JsonConvert.DeserializeObject<T>(mutableData.RawJsonValue);
#else
            return (T)mutableData.Value;
#endif
        }

        public static void SetValue(this MutableData mutableData, object value)
        {

#if !UNITY_EDITOR && UNITY_WEBGL
            mutableData.RawJsonValue = JsonConvert.SerializeObject(value);
#else
            mutableData.Value = value;
#endif
        }
    }
}
