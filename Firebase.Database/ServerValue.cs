using System.Collections.Generic;

namespace Firebase.Database
{
    /// <summary>
    /// Contains placeholder values to use when writing data to the <see cref="FirebaseDatabase"/>.
    /// </summary>
    public static class ServerValue
    {
        private const string NameSubkeyServervalue = ".sv";
        /// <summary>
        /// A placeholder value for auto-populating the current timestamp (time since the Unix epoch, in milliseconds) by the <see cref="FirebaseDatabase"/> servers.
        /// </summary>
        public readonly static object Timestamp;

        static ServerValue()
        {
            Timestamp = CreateServerValuePlaceholder("timestamp");
        }

        private static IDictionary<string, object> CreateServerValuePlaceholder(string key)
        {
            IDictionary<string, object> strs = new Dictionary<string, object>();
            strs[".sv"] = key;
            return strs;
        }
    }
}
