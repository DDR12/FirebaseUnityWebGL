using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
namespace Firebase.Database
{
    /// <summary>
    /// A DataSnapshot contains data from a Database location.
    /// A DataSnapshot is an efficiently generated, immutable copy of the data at a Database location. 
    /// It cannot be modified and will never change (to modify data, you always call the <see cref="DatabaseReference.Set"/> method on a <see cref="DatabaseReference"/> directly).
    /// </summary>
    public sealed class DataSnapshot : IDisposable
    {
        static readonly char[] pathSplitter = new char[1] { '/' };

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
                        jObject = JObject.Parse(SnapshotRawJson);
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
        /// The key (last part of the path) of the location of this <see cref="DataSnapshot"/>
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; private set; }

        [JsonProperty("snapshotRawJson")]
        string SnapshotRawJson { get; set; }
        /// <summary>
        /// ID of the <see cref="DatabaseReference"/> that generated this <see cref="DataSnapshot"/>.
        /// </summary>
        [JsonProperty("refID")]
        uint ReferenceID { get; set; }
        /// <summary>
        /// Gets the priority value of the data in this <see cref="DataSnapshot"/>
        /// </summary>
        [JsonProperty("priority")]
        public object Priority { get; private set; }

        /// <summary>
        /// The Reference for the location that generated this snapshot.
        /// </summary>
        [JsonIgnore]
        public DatabaseReference Reference => DatabaseReference.GetReference(ReferenceID);

        /// <summary>
        /// Returns true if a snapshot exists, this is slightly more efficient than using <see cref="Value"/> != null.
        /// </summary>
        [JsonIgnore]
        public bool Exists => string.IsNullOrWhiteSpace(SnapshotRawJson) == false;
        /// <summary>
        /// Returns the number of child properties of this <see cref="DataSnapshot"/>
        /// </summary>
        [JsonIgnore]
        public long ChildrenCount => JsonObject == null ? 0 : JsonObject.Count;
        /// <summary>
        /// Indicates whether this snapshot has any children
        /// </summary>
        [JsonIgnore]
        public bool HasChildren => ChildrenCount > 0;
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public object Value
        {
            get
            {
                throw new NotImplementedException("Please use GetValue<T> instead, this is an extension method that works with all versions as long as you import the 'Database.Extensions' dll.");
            }
        }
        /// <summary>
        /// Gives access to all of the immediate children of this Snapshot. Can be used in native for loops.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<DataSnapshot> Children
        {
            get
            {
                if (JsonObject == null)
                    return new DataSnapshot[0];

                DataSnapshot[] result = new DataSnapshot[JsonObject.Count];
                int counter = 0;
                foreach (var item in JsonObject)
                {
                    result[counter] = new DataSnapshot(ReferenceID, item.Key, item.Value.ToString());
                    counter++;
                }
                return result;
            }
        }
        [JsonConstructor]
        private DataSnapshot()
        {

        }
        private DataSnapshot(uint refID, string key, string json)
        {
            ReferenceID = refID;
            Key = key;
            SnapshotRawJson = json;
        }
        ~DataSnapshot()
        {
            Dispose();
        }
        /// <summary>
        /// Gets another <see cref="DataSnapshot"/> for the location at the specified relative path.
        /// Passing a relative path to the <see cref="Child(string)"/> method of a <see cref="DataSnapshot"/> returns another <see cref="DataSnapshot"/> for the location at the specified relative path.
        /// The relative path can either be a simple child name(for example, "ada") or a deeper, slash-separated path(for example, "ada/name/first").
        /// If the child location has no data, an empty <see cref="DataSnapshot"/>(that is, a DataSnapshot whose value is null) is returned.
        /// </summary>
        /// <param name="path">A relative path to the location of child data.</param>
        /// <returns></returns>
        public DataSnapshot Child(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return this;

            string[] points = path.Split(pathSplitter, StringSplitOptions.RemoveEmptyEntries);
            if (points.Length == 0)
                return this;

            DataSnapshot result = null;
            if (JsonObject != null)
            {
                JToken token = JsonObject.SelectToken(string.Join(".", points), false);
                if (token != null)
                {
                    result = new DataSnapshot(ReferenceID, points[points.Length - 1], token.ToString());
                }
            }
            if (result == null)
                result = new DataSnapshot(ReferenceID, points[points.Length - 1], null);
            return result;
        }

        /// <summary>
        ///  Returns the data contained in this snapshot as a json serialized string.
        /// </summary>
        /// <returns>The data contained in this snapshot as json. Return null if no data.</returns>
        public string GetRawJsonValue() => SnapshotRawJson;

        /// <summary>
        /// Returns the data contained in this snapshot deserialized as the type T, null if no data found or data doesn't match the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="useExportFormat">Whether or not to include priority information</param>
        /// <returns>The data, along with its priority, in native types</returns>
        public object GetValue(bool useExportFormat)
        {
            throw new NotImplementedException("Please use GetValue<T> instead, this is an extension method that works with all versions as long as you import the 'Database.Extensions' dll.");
        }
        /// <summary>
        /// Returns true if the specified child path has (non-null) data.
        /// </summary>
        /// <param name="path">A relative path to the location of a potential child.</param>
        /// <returns>Whether or not the specified child location has data</returns>
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
        
        public void Dispose()
        {
        }
    }
}
