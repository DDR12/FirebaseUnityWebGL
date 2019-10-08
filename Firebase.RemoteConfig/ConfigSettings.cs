using Newtonsoft.Json;
using System;

namespace Firebase.RemoteConfig
{
    /// <summary>
    /// Settings for <see cref="FirebaseRemoteConfig"/> operations.
    /// </summary>
    public sealed class ConfigSettings
    {
        FirebaseRemoteConfig m_Config = null;

        /// <summary>
        /// Enable / disable developer mode.
        /// </summary>
        public bool IsDeveloperMode { get; set; }

        /// <summary>
        /// Defines the maximum amount of time to wait for a response when fetching configuration from the Remote Config server. 
        /// Defaults to (One minute).
        /// </summary>
        [JsonIgnore]
        public TimeSpan FetchTimeout
        {
            get => TimeSpan.FromMilliseconds(FetchTimeoutMillis);
            set => FetchTimeoutMillis = (ulong)value.TotalMilliseconds;
        }

        [JsonProperty("fetchTimeoutMillis")]
        private ulong FetchTimeoutMillis
        {
            get => RemoteConfigPInvoke.GetFetchTimeoutMillis_WebGL(m_Config.App.Name);
            set => RemoteConfigPInvoke.SetConfigFetchTimeoutMillis_WebGL(m_Config.App.Name, value);
        }

        /// <summary>
        /// Defines the maximum age in milliseconds of an entry in the config cache before it is considered stale. Defaults (Twelve hours).
        /// </summary>
        [JsonIgnore]
        public TimeSpan MinimumFetchInterval
        {
            get => TimeSpan.FromMilliseconds(MinimumFetchIntervalMillis);
            set => MinimumFetchIntervalMillis = (ulong)value.TotalMilliseconds;
        }

        [JsonProperty("minimumFetchIntervalMillis")]
        private ulong MinimumFetchIntervalMillis
        {
            get => RemoteConfigPInvoke.GetMinimumFetchInterval_WebGL(m_Config.App.Name);
            set => RemoteConfigPInvoke.SetConfigFetchIntervalMillis_WebGL(m_Config.App.Name, value);
        }


        public ConfigSettings(FirebaseRemoteConfig remoteConfig)
        {
            m_Config = remoteConfig ?? throw new ArgumentNullException("remoteConfig", "Can't create config settings for a null remote config.");
        }
    }
}
