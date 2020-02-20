using UnityEngine;

namespace Firebase
{
    /// <summary>
    /// A simple unity asset file to contain the app options.
    /// </summary>
    [CreateAssetMenu(fileName = "FirebaseAppOptions.asset", menuName = "Firebase/Create App Options")]
    public class FirebaseAppOptions : ScriptableObject
    {
        /// <summary>
        /// Gets or sets the API key used to authenticate requests from your app.
        /// </summary>
        [Tooltip("The API key used to authenticate requests from your app.")]
        public string ApiKey = string.Empty;
        /// <summary>
        /// Gets or sets the App Id.
        /// </summary>
        [Tooltip("The App Id.")]
        public string AppId = string.Empty;
        /// <summary>
        /// The database root URL, e.g. "http://abc-xyz-123.firebaseio.com".
        /// </summary>
        [Tooltip("The database root URL, e.g. 'http://abc-xyz-123.firebaseio.com'.")]
        public string DatabaseUrl = string.Empty;
        /// <summary>
        /// Gets or sets the messaging sender Id.
        /// </summary>
        [Tooltip("The messaging sender Id.")]
        public string MessageSenderId = string.Empty;
        /// <summary>
        /// The Hosting Link of your app/game.
        /// </summary>
        [Tooltip("The Hosting Link of your app/game.")]
        public string AuthDomain = string.Empty;
        /// <summary>
        /// Gets or sets the Google Cloud project ID.
        /// </summary>
        [Tooltip("The Google Cloud project ID.")]
        public string ProjectId = string.Empty;
        /// <summary>
        /// Gets or sets the Google Cloud Storage bucket name, e.g.
        /// </summary>
        [Tooltip("The Google Cloud Storage bucket name, e.g.")]
        public string StorageBucket = string.Empty;
        /// <summary>
        /// This key is used by Firebase Cloud Messaging when sending message requests to different push services.
        /// </summary>
        [Tooltip("This key is used by Firebase Cloud Messaging when sending message requests to different push services.")]
        public string VapidPublicKey = string.Empty;

        /// <summary>
        /// This server key is used to send messages from the client as well as subscribe/unsubscribe to topics.
        /// </summary>
        [Tooltip("This server key is used to send messages from the client as well as subscribe/unsubscribe to topics.")]
        public string MessagingServerKey = string.Empty;


        static FirebaseAppOptions defaultInstance = null;
        /// <summary>
        /// The default app options file.
        /// </summary>
        public static FirebaseAppOptions AppOptions
        {
            get
            {
                if (defaultInstance == null)
                {
                    defaultInstance = Resources.Load<FirebaseAppOptions>("FirebaseAppOptions");
                    if (defaultInstance == null)
                        throw new System.Exception("There must be a 'FirebaseAppOptions' file created under any resources folder, please create one by right clicking a resources folder then Create/Firebase/FirebaseAppOptions");
                }
                return defaultInstance;
            }
        }
    }
}