using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Firebase
{
    /// <summary>
    /// Options that control the creation of a Firebase App.
    /// </summary>
    public sealed class AppOptions : IDisposable
    {
        /// <summary>
        /// Gets or sets the API key used to authenticate requests from your app.
        /// </summary>
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the App Id.
        /// </summary>
        [JsonProperty("appId")]
        public string AppId { get; set; } = string.Empty;
        /// <summary>
        /// The database root URL, e.g. "http://abc-xyz-123.firebaseio.com".
        /// </summary>
        [JsonProperty("databaseURL")]
        public string DatabaseUrl { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the messaging sender Id.
        /// </summary>
        [JsonProperty("messagingSenderId")]
        public string MessageSenderId { get; set; } = string.Empty;
        /// <summary>
        /// The Hosting Link of your game.
        /// </summary>
        [JsonProperty("authDomain")]
        public string AuthDomain { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the Google Cloud project ID.
        /// </summary>
        [JsonProperty("projectId")]
        public string ProjectId { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the Google Cloud Storage bucket name, e.g.
        /// </summary>
        [JsonProperty("storageBucket")]
        public string StorageBucket { get; set; } = string.Empty;
        /// <summary>
        /// This key is used by Firebase Cloud Messaging when sending message requests to different push services.
        /// </summary>
        [JsonProperty("vapidPublicKey")]
        public string VapidPublicKey { get; set; } = string.Empty;

        /// <summary>
        /// This server key is used to send messages from the client as well as subscribe/unsubscribe to topics.
        /// </summary>
        [JsonProperty("messaginServerKey")]
        public string MessagingServerKey { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public AppOptions() { }
        /// <summary>
        /// Dispose the current object.
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Load options from a JSON string.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns>Returns an <see cref="AppOptions"/> instance if successful, null otherwise.</returns>
        public static AppOptions LoadFromJsonConfig(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<AppOptions>(jsonString);
            }
            catch(Exception ex)
            {
                Debug.Log($"Failed to load app options from json, error:\n{ex.Message}");
                return null;
            }
        }

        internal static AppOptions LoadDefaultOptions()
        {
            var textFile = Resources.Load<TextAsset>("FirebaseAppOptions");
            if (textFile == null)
                throw new Exception("Failed to load app options, please make sure your options are places in a file named 'FirebaseAppOptions' in any 'Resources' folder in your project.");

            string json = textFile.text;
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException("appOptionsJson", "The FirebaseAppOptions file is empty!");
            Debug.Log($"Loaded config successfully!");
            return LoadFromJsonConfig(json);
        }
    }
}
