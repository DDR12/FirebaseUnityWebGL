using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firebase.RemoteConfig
{
    /// <summary>
    /// The Firebase Remote Config service interface.
    /// </summary>
    public sealed class FirebaseRemoteConfig : IDisposable
    {
        static Dictionary<string, FirebaseRemoteConfig> remoteConfigs;

        static FirebaseRemoteConfig defaultInstance = null;
        /// <summary>
        /// Default instance of the <see cref="FirebaseRemoteConfig"/> for the default <see cref="FirebaseApp"/>.
        /// </summary>
        public static FirebaseRemoteConfig DefaultInstance
        {
            get
            {
                if (defaultInstance == null)
                    defaultInstance = GetInstance(FirebaseApp.DefaultInstance);
                return defaultInstance;
            }
        }
        static FirebaseRemoteConfig()
        {
            remoteConfigs = new Dictionary<string, FirebaseRemoteConfig>();
        }
        /// <summary>
        /// The firebase app that this <see cref="FirebaseRemoteConfig"/> is attached to.
        /// </summary>
        public FirebaseApp App { get; }

        ConfigSettings m_Settings = null;
        /// <summary>
        /// Gets and sets the configuration settings for operations.
        /// </summary>
        public ConfigSettings Settings
        {
            get
            {
                if (m_Settings == null)
                    m_Settings = new ConfigSettings(this);
                return m_Settings;
            }
        }

        private double defaultCacheExpirationMilliseconds = 12 * 60 * 60 * 1000;
        /// <summary>
        /// The default cache expiration used by Fetch(), equal to 12 hours.
        /// </summary>
        public TimeSpan DefaultCacheExpiration
        {
            get => TimeSpan.FromMilliseconds(defaultCacheExpirationMilliseconds);
            set => defaultCacheExpirationMilliseconds = value.TotalMilliseconds;
        }

        ConfigInfo m_Info = null;
        /// <summary>
        /// A <see cref="ConfigInfo"/> struct, containing fields reflecting the state of the most recent fetch request.
        /// </summary>
        public ConfigInfo Info
        {
            get
            {
                if (m_Info == null)
                    m_Info = new ConfigInfo(this);
                return m_Info;
            }
        }
        /// <summary>
        /// The set of all Remote Config parameter keys in the default namespace.
        /// </summary>
        public IEnumerable<string> Keys
        {
            get
            {
                var json = RemoteConfigPInvoke.GetKeysList_WebGL(App.Name);
                string[] keys = JsonConvert.DeserializeObject<string[]>(json);
                return keys;
            }
        }
        /// <summary>
        /// Object containing default values for configs.
        /// </summary>
        public Dictionary<string, object> DefaultConfig
        {
            get
            {
                var defaultConfigJson = RemoteConfigPInvoke.GetDefaultConfig_WebGL(App.Name);
                if (string.IsNullOrWhiteSpace(defaultConfigJson))
                    return new Dictionary<string, object>();
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(defaultConfigJson);
            }
        }
        /// <summary>
        /// Gets all config.
        /// </summary>
        public Dictionary<string, ConfigValue> AllValues
        {
            get
            {
                var json = RemoteConfigPInvoke.GetAllConfig_WebGL(App.Name);
                if (string.IsNullOrWhiteSpace(json))
                    return new Dictionary<string, ConfigValue>();
                return JsonConvert.DeserializeObject<Dictionary<string, ConfigValue>>(json);
            }
        }
    
        private FirebaseRemoteConfig(FirebaseApp app)
        {
            App = app;
            remoteConfigs.Add(App.Name, this);
        }
        ~FirebaseRemoteConfig()
        {
            Dispose();
        }
        /// <summary>
        /// Applies the most recently fetched data, so that its values can be accessed.
        /// Calls to GetLong(), GetDouble(), GetString() and GetData() will not reflect the new data retrieved by Fetch() until ActivateFetched() is called.This gives the developer control over when newly fetched data is visible to their application.
        /// </summary>
        /// <returns>true if a previously fetch configuration was activated, false if a fetched configuration wasn't found or the configuration was previously activated.
        /// </returns>
        public Task<bool> ActivateFetched()
        {
            var task = WebGLTaskManager.GetTask<bool>();
            RemoteConfigPInvoke.RemoteConfigActivateFetched_WebGL(task.Task.Id, App.Name, WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        /// <summary>
        /// Ensures the last activated config are available to the getters.
        /// </summary>
        /// <returns></returns>
        public Task EnsureInitialized()
        {
            var task = WebGLTaskManager.GetTask();
            RemoteConfigPInvoke.EnsureConfigInitialized_WebGL(task.Task.Id, App.Name, WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// Fetches config data from the server.
        /// This does not actually apply the data or make it accessible, it merely retrieves it and caches it.
        /// To accept and access the newly retrieved values, you must call ActivateFetched(). Note that this function is asynchronous, and will normally take an unspecified amount of time before completion.
        /// </summary>
        /// <returns>A Future which can be used to determine when the fetch is complete.</returns>
        public Task FetchAsync()
        {
            var task = WebGLTaskManager.GetTask();
            RemoteConfigPInvoke.RemoteConfigFetch_WebGL(task.Task.Id, App.Name, WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        /// <summary>
        /// Performs fetch and activate operations, as a convenience. Returns a task which resolves to true if the current call activated the fetched configs. 
        /// If the fetched configs were already activated, the task will resolve to false.
        /// </summary>
        /// <returns></returns>
        public Task<bool> FetchAndActivate()
        {
            var task = WebGLTaskManager.GetTask<bool>();
            RemoteConfigPInvoke.RemoteConfigFetchAndActivate_WebGL(task.Task.Id, App.Name, WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// Gets the set of keys that start with the given prefix.
        /// </summary>
        /// <param name="prefix">The key prefix to look for. If empty or null, this method will return all keys. Set of Remote Config parameter keys that start with the specified prefix. Will return an empty set if there are no keys with the given prefix.</param>
        /// <returns></returns>
        public IEnumerable<string> GetKeysByPrefix(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return Keys;
            return Keys.Where(o => o.StartsWith(prefix));
        }

        /// <summary>
        /// Gets the <see cref="ConfigValue"/> corresponding to the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The <see cref="ConfigValue"/> associated with the specified key.</returns>
        public ConfigValue GetValue(string key)
        {
            var json = RemoteConfigPInvoke.GetRemotConfigValue_WebGL(App.Name, key);
            return JsonConvert.DeserializeObject<ConfigValue>(json);
        }
        /// <summary>
        /// Sets the default values based on a string dictionary.
        /// This completely overrides all previous values.
        /// </summary>
        /// <param name="defaults">IDictionary of string keys to values, representing the new set of defaults to apply. If the same key is specified multiple times, the value associated with the last duplicate key is applied.</param>
        public void SetDefaults(IDictionary<string, object> defaults)
        {
            defaults = defaults ?? new Dictionary<string, object>();
            RemoteConfigPInvoke.SetRemoteConfigDefaults_WebGL(App.Name, JsonConvert.SerializeObject(defaults));
        }

        /// <summary>
        /// Returns the <see cref="FirebaseRemoteConfig"/> , initialized with a custom <see cref="FirebaseApp"/>
        /// </summary>
        /// <param name="app">The custom <see cref="FirebaseApp"/> used for initialization.</param>
        /// <returns></returns>
        public static FirebaseRemoteConfig GetInstance(FirebaseApp app)
        {
            app = app ?? FirebaseApp.DefaultInstance;
            if (remoteConfigs.TryGetValue(app.Name, out FirebaseRemoteConfig remoteConfig))
                return remoteConfig;
            return new FirebaseRemoteConfig(app);
        }

        public void Dispose()
        {
            remoteConfigs.Remove(App.Name);
        }
    }
}
