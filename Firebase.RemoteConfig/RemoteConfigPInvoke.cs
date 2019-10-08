using System.Runtime.InteropServices;
namespace Firebase.RemoteConfig
{
    internal class RemoteConfigPInvoke
    {
        [DllImport("__Internal")]
        public static extern void SetConfigFetchTimeoutMillis_WebGL(string appName, ulong milliseconds);
        [DllImport("__Internal")]
        public static extern ulong GetFetchTimeoutMillis_WebGL(string appName);
        [DllImport("__Internal")]
        public static extern ulong GetMinimumFetchInterval_WebGL(string appName);
        [DllImport("__Internal")]
        public static extern void SetConfigFetchIntervalMillis_WebGL(string appName, ulong milliseconds);
        [DllImport("__Internal")]
        public static extern ulong GetRemoteConfigFetchTime_WebGL(string appName);
        [DllImport("__Internal")]
        public static extern string GetLastFetchStatus_WebGL(string appName);
        [DllImport("__Internal")]
        public static extern void RemoteConfigActivateFetched_WebGL(int taskID, string appName, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void EnsureConfigInitialized_WebGL(int taskID, string appName, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void RemoteConfigFetch_WebGL(int taskID, string appName, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void RemoteConfigFetchAndActivate_WebGL(int taskID, string appName, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern string GetKeysList_WebGL(string appName);
        [DllImport("__Internal")]
        public static extern string GetDefaultConfig_WebGL(string appName);
        [DllImport("__Internal")]
        public static extern string GetAllConfig_WebGL(string appName);
        [DllImport("__Internal")]
        public static extern string GetRemotConfigValue_WebGL(string appName,string key);
        [DllImport("__Internal")]
        public static extern void SetRemoteConfigDefaults_WebGL(string appName, string defaultsMap);

    }
}
