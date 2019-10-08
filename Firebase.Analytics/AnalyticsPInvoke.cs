using System.Runtime.InteropServices;
namespace Firebase.Analytics
{
    internal class AnalyticsPInvoke
    {
        [DllImport("__Internal")]
        public static extern void SetAnalyticsSettings_WebGL(string settingsJson);
        [DllImport("__Internal")]
        public static extern void SetAnalyticsUserProperty_WebGL(string appName, string propertyName, string propertyValue);
        [DllImport("__Internal")]
        public static extern void SetAnalyticsUserId_WebGL(string appName, string userId);
        [DllImport("__Internal")]
        public static extern void SetAnalyticsCurrentScreen_WebGL(string appName, string screenName, string optionsJson);
        [DllImport("__Internal")]
        public static extern void LogAnalyticsEvent_WebGL(string appName, string eventName, string eventParamsJson, string optionsJson);

        [DllImport("__Internal")]
        public static extern void SetAnalyticsCollectionEnabled_WebGL(string appName, bool enabled);

    }
}
