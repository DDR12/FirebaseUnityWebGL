using System;
using System.Collections.Generic;

namespace Firebase.Database
{
    /// <summary>
    /// The Firebase Database service.
    /// </summary>
    public sealed class FirebaseDatabase : IDisposable
    {
        private static IDictionary<string, FirebaseDatabase> databases;

        static FirebaseDatabase()
        {
            databases = new Dictionary<string, FirebaseDatabase>();
        }
        static FirebaseDatabase defaultInstance = null;
        /// <summary>
        /// Gets the instance of <see cref="FirebaseDatabase"/> for the default Firebase.App.
        /// </summary>
        public static FirebaseDatabase DefaultInstance
        {
            get
            {
                if (defaultInstance == null)
                    defaultInstance = GetInstance(FirebaseApp.DefaultInstance);
                return defaultInstance;
            }
        }
        internal static FirebaseDatabase AnyInstance
        {
            get
            {
                foreach (var item in databases)
                {
                    if (item.Value == null)
                        continue;
                    return item.Value;
                }
                return DefaultInstance;
            }
        }
        internal string Name { get; }
        /// <summary>
        /// Returns the <see cref="FirebaseApp"/> instance to which this <see cref="FirebaseDatabase"/> belongs.
        /// </summary>
        public FirebaseApp App { get; }

        /// <summary>
        /// Gets a <see cref="DatabaseReference"/> for the root location of this <see cref="FirebaseDatabase"/>.
        /// </summary>
        public DatabaseReference RootReference => GetReference(null);

        LogLevel m_LogLevel = LogLevel.Debug;
        /// <summary>
        /// By default, this is set to Info This includes any internal errors ( Error ) and any security debug messages ( Info ) that the client receives.
        /// Set to Debug to turn on the diagnostic logging.
        /// </summary>
        public LogLevel LogLevel
        {
            get
            {
                PlatformHandler.NotifyWebGLFeatureDoesntHaveAMatch();
                return m_LogLevel;
            }
            set
            {
                PlatformHandler.NotifyWebGLFeatureDoesntHaveAMatch();
                m_LogLevel = value;
            }
        }

        private FirebaseDatabase(FirebaseApp app, string name)
        {
            App = app;
            Name = name;
        }
        ~FirebaseDatabase()
        {
            Dispose();
        }
        /// <summary>
        /// Disconnects from the server (all Database operations will be completed offline).
        /// The client automatically maintains a persistent connection to the Database server, which will remain active indefinitely and reconnect when disconnected.
        /// However, the goOffline() and goOnline() methods may be used to control the client connection in cases where a persistent connection is undesirable.
        /// While offline, the client will no longer receive data updates from the Database.However, all Database operations performed locally will continue to immediately fire events, allowing your application to continue behaving normally. 
        /// Additionally, each operation performed locally will automatically be queued and retried upon reconnection to the Database server.
        /// To reconnect to the Database and begin receiving remote events, see <see cref="GoOnline"/>
        /// </summary>
        public void GoOffline()
        {
            DatabasePInvoke.DatabaseGoOffline_WebGL(App.Name);
        }

        /// <summary>
        /// Reconnects to the server and synchronizes the offline Database state with the server state.
        /// This method should be used after disabling the active connection with <see cref="GoOffline"/>. 
        /// Once reconnected, the client will transmit the proper data and fire the appropriate events so that your client "catches up" automatically.
        /// </summary>
        public void GoOnline()
        {
            DatabasePInvoke.DatabaseGoOnline_WebGL(App.Name);
        }

        /// <summary>
        /// Gets a <see cref="DatabaseReference"/> for the provided path.
        /// </summary>
        /// <param name="path">Path to a location in your <see cref="FirebaseDatabase"/>.</param>
        /// <returns>A <see cref="DatabaseReference"/> pointing to the specified path.</returns>
        public DatabaseReference GetReference(string path)
        {
            return DatabaseReference.Create(this, DatabasePInvoke.DatabaseReferenceFromPath_WebGL(App.Name, path));
        }

        /// <summary>
        /// Gets a <see cref="DatabaseReference"/> for the provided URL.
        /// </summary>
        /// <param name="url">A URL to a path within your database.</param>
        /// <returns>A <see cref="DatabaseReference"/> for the provided URL.</returns>
        public DatabaseReference GetReferenceFromUrl(Uri url)
        {
            return GetReferenceFromUrl(url.ToString());
        }
        /// <summary>
        /// Gets a <see cref="DatabaseReference"/> for the provided URL.
        /// </summary>
        /// <param name="url">A URL to a path within your database.</param>
        /// <returns>A <see cref="DatabaseReference"/> for the provided URL.</returns>
        public DatabaseReference GetReferenceFromUrl(string url)
        {
            return DatabaseReference.Create(this, DatabasePInvoke.DatabaseReferenceFromURL_WebGL(App.Name, url));
        }
        public void Dispose()
        {
            databases.Remove(Name);
        }

        /// <summary>
        /// The <see cref="FirebaseDatabase"/> client automatically queues writes and sends them to the server at the earliest opportunity, depending on network connectivity. 
        /// In some cases (e.g. offline usage) there may be a large number of writes waiting to be sent. 
        /// Calling this method will purge all outstanding writes so they are abandoned.
        /// All writes will be purged, including transactions and <see cref="DatabaseReference.OnDisconnect"/> writes. 
        /// The writes will be rolled back locally, perhaps triggering events for affected event listeners, and the client will not (re-)send them to the Firebase backend.
        /// </summary>
        public void PurgeOutstandingWrites()
        {
            PlatformHandler.NotifyWebGLFeatureDoesntHaveAMatch();
        }

        /// <summary>
        /// The <see cref="FirebaseDatabase"/> client will cache synchronized data and keep track of all writes you've initiated while your application is running. 
        /// It seamlessly handles intermittent network connections and re-sends write operations when the network connection is restored. 
        /// However by default your write operations and cached data are only stored in-memory and will be lost when your app restarts. 
        /// By setting this value to true, the data will be persisted to on-device (disk) storage and will thus be available again when the app is restarted (even when there is no network connectivity at that time).
        /// </summary>
        /// <param name="enabled">Set this to true to persist write data to on-device (disk) storage, or false to discard pending writes when the app exists.</param>
        public void SetPersistenceEnabled(bool enabled)
        {
            PlatformHandler.NotifyFeatureIsUselessInWebGL();
        }


        /// <summary>
        /// Gets an instance of <see cref="FirebaseDatabase"/> for a specific <see cref="FirebaseApp"/>.
        /// </summary>
        /// <param name="app">The <see cref="FirebaseApp"/> to get a <see cref="FirebaseDatabase"/> for.</param>
        /// <returns>A <see cref="FirebaseDatabase"/> instance.</returns>
        public static FirebaseDatabase GetInstance(FirebaseApp app)
        {
            return GetInstance(app, app.Options.DatabaseUrl);
        }
        /// <summary>
        /// Gets an instance of <see cref="FirebaseDatabase"/> for the specified URL.
        /// </summary>
        /// <param name="url">The URL to the <see cref="FirebaseDatabase"/> instance you want to access.</param>
        /// <returns>A <see cref="FirebaseDatabase"/> instance.</returns>
        public static FirebaseDatabase GetInstance(string url)
        {
            FirebaseApp defaultapp = FirebaseApp.DefaultInstance;
            if (defaultapp == null)
            {
                throw new DatabaseException("FirebaseApp could not be initialized.");
            }
            return GetInstance(defaultapp, url);
        }
        /// <summary>
        /// Gets a <see cref="FirebaseDatabase"/> instance for the specified URL, using the specified <see cref="FirebaseApp"/>.
        /// </summary>
        /// <param name="app">The <see cref="FirebaseApp"/> to get a <see cref="FirebaseDatabase"/> for.</param>
        /// <param name="url">The URL to the <see cref="FirebaseDatabase"/> instance you want to access.</param>
        /// <returns>A <see cref="FirebaseDatabase"/> instance.</returns>
        public static FirebaseDatabase GetInstance(FirebaseApp app, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new DatabaseException("Failed to get FirebaseDatabase instance: Specify DatabaseURL within FirebaseApp or from your GetInstance() call.");

            string str = $"({app.Name}, {url})";
            if (!databases.TryGetValue(str, out FirebaseDatabase firebaseDatabase))
            {
                firebaseDatabase = new FirebaseDatabase(app, str);
                databases[str] = firebaseDatabase;
            }
            return firebaseDatabase;
        }


    }
}