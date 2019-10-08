using System;
using System.Collections.Generic;

namespace Firebase.Storage
{
    /// <summary>
    /// <see cref="FirebaseStorage"/> is a service that supports uploading and downloading large objects to Google Cloud Storage.
    /// </summary>
    public sealed class FirebaseStorage : IDisposable
    {
        static IDictionary<string, FirebaseStorage> storageByAppName;
        static FirebaseStorage()
        {
            storageByAppName = new Dictionary<string, FirebaseStorage>();
        }
        static FirebaseStorage defaultInstance = null;

        /// <summary>
        /// Returns the <see cref="FirebaseStorage"/> , initialized with the default <see cref="FirebaseApp"/>
        /// </summary>
        public static FirebaseStorage DefaultInstance
        {
            get
            {
                if (defaultInstance == null)
                    defaultInstance = GetInstance(FirebaseApp.DefaultInstance, null);
                return defaultInstance;
            }
        }

        public FirebaseApp App { get; }

        /// <summary>
        /// The maximum time to retry downloads.
        /// </summary>
        public TimeSpan MaxDownloadRetryTime
        {
            get => TimeSpan.FromMilliseconds(StoragePInvoke.GetMaxDownloadRetryTime_WebGL(App.Name));
            set => StoragePInvoke.SetMaxDownloadRetryTime_WebGL(App.Name, (uint)value.TotalMilliseconds);
        }
        /// <summary>
        /// The maximum time to retry operations other than uploads or downloads.
        /// </summary>
        public TimeSpan MaxOperationRetryTime
        {
            get => TimeSpan.FromMilliseconds(StoragePInvoke.GetMaxOperationRetryTime_WebGL(App.Name));
            set => StoragePInvoke.SetMaxOperationRetryTime_WebGL(App.Name, (uint)value.TotalMilliseconds);
        }

        /// <summary>
        /// The maximum time to retry uploads.
        /// </summary>
        public TimeSpan MaxUploadRetryTime
        {
            get => TimeSpan.FromMilliseconds(StoragePInvoke.GetMaxUploadRetryTime_WebGL(App.Name));
            set => StoragePInvoke.SetMaxUploadRetryTime_WebGL(App.Name, (uint)value.TotalMilliseconds);
        }

        /// <summary>
        /// Creates a new <see cref="StorageReference"/> initialized at the root Cloud Storage location.
        /// </summary>
        public StorageReference RootReference => GetReference(null);

        private readonly string instanceKey = null;

        private FirebaseStorage(FirebaseApp app, string url)
        {
            instanceKey = GetInstanceKey(app.Name, url);
            storageByAppName.Add(instanceKey, this);
        }

        /// <summary>
        /// Returns a reference for the given path in the default bucket.
        /// </summary>
        /// <param name="path">A relative path to initialize the reference with, for example path/to/image.jpg. If not passed, the returned reference points to the bucket root.</param>
        /// <returns>A reference for the given path.</returns>
        public StorageReference GetReference(string path) => StorageReference.Create(this ,StoragePInvoke.StorageReferenceFromPath_WebGL(App.Name, path));


        /// <summary>
        /// Returns a reference for the given absolute URL.
        /// </summary>
        /// <param name="fullUrl">A gs:// or http[s]:// URL used to initialize the reference.
        /// For example, you can pass in a download URL retrieved from <see cref="StorageReference.GetDownloadUrlAsync"/> or the uri retrieved from <see cref="StorageReference.ToString"/>
        /// An error is thrown if fullUrl is not associated with the <see cref="FirebaseApp"/> used to initialize this <see cref="FirebaseStorage"/>
        /// </param>
        /// <returns>A reference for the given URL.</returns>
        public StorageReference GetReferenceFromUrl(string fullUrl) => StorageReference.Create(this, StoragePInvoke.StorageReferenceFromURL_WebGL(App.Name,fullUrl));

        public void Dispose()
        {
            storageByAppName.Remove(instanceKey);
        }

        /// <summary>
        /// Returns the <see cref="FirebaseStorage"/> , initialized with the default <see cref="FirebaseApp"/> and a custom Storage Bucket
        /// </summary>
        /// <param name="url">The gs:// url to your Cloud Storage Bucket.</param>
        /// <returns></returns>
        public static FirebaseStorage GetInstance(string url)
        {
            return GetInstance(FirebaseApp.DefaultInstance, url);
        }

        /// <summary>
        /// Returns the <see cref="FirebaseStorage"/> , initialized with a custom <see cref="FirebaseApp"/>
        /// </summary>
        /// <param name="app">The custom <see cref="FirebaseApp"/> used for initialization.</param>
        /// <param name="url">The gs:// url to your Cloud Storage Bucket.</param>
        /// <returns></returns>
        public static FirebaseStorage GetInstance(FirebaseApp app, string url = null)
        {
            app = app ?? FirebaseApp.DefaultInstance;

            string str;
            if (!string.IsNullOrWhiteSpace(url))
                str = url;
            else if (string.IsNullOrWhiteSpace(app.Options.StorageBucket))
                str = null;
            else
                str = $"gs://{app.Options.StorageBucket}";

            string instanceKey = GetInstanceKey(app.Name, str);
            if (storageByAppName.TryGetValue(instanceKey, out FirebaseStorage firebaseStorage))
                return firebaseStorage;

            firebaseStorage = new FirebaseStorage(app, url);
            return firebaseStorage;
        }

        public static string GetInstanceKey(string appName, string url) => $"{appName}/{url}";
        public static FirebaseStorage FindByKey(string key)
        {
            if(storageByAppName.TryGetValue(key,out FirebaseStorage instance))
            {
                return instance;
            }
            return null;
        }
    }
}