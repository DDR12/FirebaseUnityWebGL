using Newtonsoft.Json;
using System;

namespace Firebase.Storage
{
    /// <summary>
    /// Result returned by <see cref="StorageReference.List"/>, this feature is WebGL only.
    /// </summary>
    public sealed class ListResult
    {
        /// <summary>
        /// References to prefixes (sub-folders). You can call list() on them to get its contents.
        /// Folders are implicit based on '/' in the object paths.
        ///  For example, if a bucket has two objects '/a/b/1' and '/a/b/2', list('/a') will return '/a/b' as a prefix.
        /// </summary>
        [JsonProperty("prefixes")]
        string[] prefixesPaths = null;

        /// <summary>
        /// Objects in this directory.
        /// You can call getMetadate() and getDownloadUrl() on them.
        /// </summary>
        [JsonProperty("items")]
        string[] itemsPaths = null;


        [JsonProperty("appName")]
        string appName = null;
        [JsonProperty("storageBucket")]
        string storageBucket = null;

        [JsonProperty("refID")]
        uint sourceRefID = 0;

        /// <summary>
        /// If set, there might be more results for this list. Use this token to resume the list.
        /// </summary>
        [JsonProperty("nextPageToken")]
        public string NextPageToken { get; set; }

        /// <summary>
        /// The reference location which this list is a child of.
        /// </summary>
        public StorageReference Reference => StorageReference.GetExistingReference(sourceRefID);

        /// <summary>
        /// The storage bucket, in which these items exist.
        /// </summary>
        [JsonIgnore]
        public FirebaseStorage Storage
        {
            get
            {
                if (Reference != null)
                    return Reference.Storage;
                return FirebaseStorage.FindByKey(FirebaseStorage.GetInstanceKey(appName, $"gs://{storageBucket}"));
            }
        }

        [JsonIgnore]
        StorageReference[] m_Prefixes = null;
        [JsonIgnore]
        public StorageReference[] Prefixes
        {
            get
            {
                if (m_Prefixes == null && prefixesPaths == null)
                    return null;
                if((prefixesPaths != null && m_Prefixes == null) || (prefixesPaths.Length != m_Prefixes.Length))
                {
                    m_Prefixes = PathListToReferenceList(Storage, prefixesPaths);
                }
                return m_Prefixes;
            }
        }
        [JsonIgnore]
        StorageReference[] m_Items = null;
        [JsonIgnore]
        public StorageReference[] Items
        {
            get
            {
                if (m_Items == null && itemsPaths == null)
                    return null;
                if((itemsPaths != null && m_Items == null) || (itemsPaths.Length != m_Items.Length))
                {
                    m_Items = PathListToReferenceList(Storage, itemsPaths);
                }
                return m_Items;
            }
        }
        private static StorageReference[] PathListToReferenceList(FirebaseStorage storage, string[] paths)
        {
            if (storage == null)
                throw new InvalidOperationException("Couldn't resolve the storage bucket containing the incoming list, references may have been disposed, please report.");
            StorageReference[] result = new StorageReference[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                result[i] = StorageReference.Create(storage, paths[i]);
            }
            return result;
        }

    }
}
