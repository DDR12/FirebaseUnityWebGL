using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Firebase.Database
{
    /// <summary>
    /// Instances of this class encapsulate the data and priority at a location.
    /// </summary>
    /// <summary>
    /// Instances of this class encapsulate the data and priority at a location.
    /// </summary>
    public sealed class MutableData
    {
        static readonly char[] pathSplitter = new char[1] { '/' };

        [JsonProperty("value")]
        public string RawJsonValue { get; set; }
        /// <summary>
        /// The key name of this location, or null if it is the top-most location.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; private set; }
        /// <summary>
        /// Gets the current priority at this location.
        [JsonProperty("priority")]
        public object Priority { get; set; }

        [JsonIgnore]
        public object Value
        {
            get
            {
                throw new NotImplementedException("Please use MutableData.GetValue<T> instead, this is an extension method that works with all sdks as long as you import the 'Database.Extensions' dll.");
            }
            set
            {
                throw new NotImplementedException("Please use MutableData.SetValue instead, this is an extension method that works with all sdks as long as you import the 'Database.Extensions' dll.");
            }
        }

        [JsonIgnore]
        JObject jObject = null;
        [JsonIgnore]
        bool triedToCreateObject = false;

        JObject JsonObject
        {
            get
            {
                if (jObject == null && !triedToCreateObject)
                {
                    triedToCreateObject = true;
                    try
                    {
                        jObject = JObject.Parse(RawJsonValue);
                    }
                    catch
                    {
                        jObject = null;
                    }
                }
                return jObject;
            }
        }


        /// <summary>
        /// Used to iterate over the immediate children at this location
        /// </summary>
        [JsonIgnore]
        public IEnumerable<MutableData> Children
        {
            get
            {
                if (JsonObject == null)
                    return new MutableData[0];

                MutableData[] result = new MutableData[JsonObject.Count];
                int counter = 0;
                foreach (var item in JsonObject)
                {
                    result[counter] = new MutableData(item.Key, item.Value.ToString());
                    counter++;
                }
                return result;
            }
        }
        /// <summary>
        /// The number of immediate children at this location.
        /// </summary>
        [JsonIgnore]
        public long ChildrenCount => JsonObject == null ? 0 : JsonObject.Count;
        /// <summary>
        /// True if the data at this location has children, false otherwise.
        /// </summary>
        [JsonIgnore]
        public bool HasChildren => ChildrenCount > 0;
        
        public void SetValue(object value)
        {
            RawJsonValue = JsonConvert.SerializeObject(value);
        }
        private MutableData(string key, string json)
        {
            Key = key;
            RawJsonValue = json;
        }
        public MutableData() { }
        /// <summary>
        /// Used to obtain a <see cref="MutableData"/> instance that encapsulates the data and priority at the given relative path.
        /// </summary>
        /// <param name="path">A relative path</param>
        /// <returns>An instance encapsulating the data and priority at the given path</returns>
        public MutableData Child(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return this;

            string[] points = path.Split(pathSplitter, StringSplitOptions.RemoveEmptyEntries);
            if (points.Length == 0)
                return this;

            MutableData result = null;
            if (JsonObject != null)
            {
                JToken token = JsonObject.SelectToken(string.Join(".", points), false);
                if (token != null)
                {
                    result = new MutableData(points[points.Length - 1], token.ToString());
                }
            }
            if (result == null)
                result = new MutableData(points[points.Length - 1], null);
            return result;
        }
        /// <summary>
        /// Determines if data exists at the given path.
        /// </summary>
        /// <param name="path">A relative path</param>
        /// <returns>True if data exists at the given path, otherwise false</returns>
        public bool HasChild(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            string[] points = path.Split(pathSplitter, StringSplitOptions.RemoveEmptyEntries);
            if (points.Length == 0)
                return false;

            if (JsonObject == null)
                return false;

            JToken token = JsonObject.SelectToken(string.Join(".", points), false);
            return token != null;
        }
        /// <summary>
        /// Representation of the mutable data as a string containing a key, value pair.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Concat(new object[] { "MutableData { key = ", this.Key, ", value = ", this.RawJsonValue, " }" });
        }
    }
}
