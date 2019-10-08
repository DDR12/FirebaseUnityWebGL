using System;

namespace Firebase.RemoteConfig
{
    /// <summary>
    /// Describes the state of the most recent Fetch() call.
    /// Normally returned as a result of the GetInfo() function.
    /// </summary>
    public sealed class ConfigInfo
    {
        FirebaseRemoteConfig m_RemoteConfig = null;

        private readonly DateTime UnixEpochUtc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// The time when the last fetch operation completed.
        /// </summary>
        public DateTime FetchTime
        {
            get
            {
                ulong milliseconds = RemoteConfigPInvoke.GetRemoteConfigFetchTime_WebGL(m_RemoteConfig.App.Name);

                return this.UnixEpochUtc.AddMilliseconds(milliseconds);
            }
        }
        /// <summary>
        /// The reason the most recent fetch failed.
        /// </summary>
        public FetchFailureReason LastFetchFailureReason
        {
            get
            {
                string lastFetchStatus = RemoteConfigPInvoke.GetLastFetchStatus_WebGL(m_RemoteConfig.App.Name);
                if (lastFetchStatus == "no-fetch-yet" || lastFetchStatus == "success")
                    return FetchFailureReason.Invalid;
                else if (lastFetchStatus == "throttle")
                    return FetchFailureReason.Throttled;
                else
                    return FetchFailureReason.Error;
            }
        }

        /// <summary>
        /// The status of the last fetch request.
        /// </summary>
        public LastFetchStatus LastFetchStatus
        {
            get
            {
                string lastFetchStatus = RemoteConfigPInvoke.GetLastFetchStatus_WebGL(m_RemoteConfig.App.Name);
                if (lastFetchStatus == "no-fetch-yet")
                    return LastFetchStatus.NoFetchYet;
                else if (lastFetchStatus == "success")
                    return LastFetchStatus.Success;
                else
                    return LastFetchStatus.Failure;
            }
        }
        /// <summary>
        /// The time when Remote Config data refreshes will no longer be throttled.
        /// </summary>
        public DateTime ThrottledEndTime
        {
            get
            {
                throw new NotImplementedException("Throttled End Time has no implementation in WebGL");
            }
        }
        public ConfigInfo(FirebaseRemoteConfig remoteConfig)
        {
            m_RemoteConfig = remoteConfig ?? throw new ArgumentNullException("remoteConfig", "Can't create ConfigInfo for a null FirebaseRemoteConfig instance.");
        }
    }
}
