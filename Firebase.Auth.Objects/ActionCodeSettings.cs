using Newtonsoft.Json;
namespace Firebase.Auth
{
    /// <summary>
    /// This is the interface that defines the required continue/state URL with optional Android and iOS bundle identifiers
    /// </summary>
    public class ActionCodeSettings
    {
        /// <summary>
        /// Sets the link continue/state URL, which has different meanings in different contexts:
        /// When the link is handled in the web action widgets, this is the deep link in the continueUrl query parameter.
        /// When the link is handled in the app directly, this is the continueUrl query parameter in the deep link of the Dynamic Link.
        /// </summary>
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        /// <summary>
        /// Sets the Android package name. 
        /// This will try to open the link in an android app if it is installed. 
        /// If installApp is passed, it specifies whether to install the Android app if the device supports it and the app is not already installed. 
        /// If this field is provided without a packageName, an error is thrown explaining that the packageName must be provided in conjunction with this field. 
        /// If minimumVersion is specified, and an older version of the app is installed, the user is taken to the Play Store to upgrade the app.
        /// </summary>
        [JsonProperty("android",NullValueHandling = NullValueHandling.Ignore)]
        public AndroidActionCodeSettings Android { get; set; }

        /// <summary>
        /// Sets the iOS bundle ID. This will try to open the link in an iOS app if it is installed.
        /// </summary>
        [JsonProperty("iOS", NullValueHandling = NullValueHandling.Ignore)]
        public IOSActionCodeSettings IOS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("dynamicLinkDomain", NullValueHandling = NullValueHandling.Ignore)]
        public string DynamicLinkDomain { get; set; }

        /// <summary>
        /// The default is false. 
        /// When set to true, the action code link will be be sent as a Universal Link or Android App Link and will be opened by the app if installed.
        /// In the false case, the code will be sent to the web widget first and then on continue will redirect to the app if installed.
        /// </summary>
        [JsonProperty("handleCodeInApp", NullValueHandling = NullValueHandling.Ignore)]
        public bool? HandleCodeInApp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public class AndroidActionCodeSettings
        {
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("installApp")]
            public bool InstallApp { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("minimumVersion", NullValueHandling = NullValueHandling.Ignore)]
            public string MinimumVersion { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("packageName", NullValueHandling = NullValueHandling.Ignore)]
            public string PackageName { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public class IOSActionCodeSettings
        {
            /// <summary>
            /// Sets the iOS bundle ID. This will try to open the link in an iOS app if it is installed.
            /// </summary>
            [JsonProperty("bundleId", NullValueHandling = NullValueHandling.Ignore)]
            public string BundleId { get; set; }
        }
        /// <summary>
        /// Serialize ActionCodeSettings object to the JSON format.
        /// </summary>
        /// <param name="actionCodeSettings"></param>
        /// <returns></returns>
        public static string ToJson(ActionCodeSettings actionCodeSettings)
        {
            if (actionCodeSettings == null)
                return null;
            return JsonConvert.SerializeObject(actionCodeSettings);
        }
    }
}
