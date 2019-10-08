using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Firebase.Messaging
{
    /// <summary>
    /// Used for messages that display a notification.
    /// </summary>
    public sealed class FirebaseNotification : IDisposable
    {
        /// <summary>
        /// Android-specific data to show.
        /// </summary>
        [JsonProperty("android", NullValueHandling = NullValueHandling.Ignore)]
        public AndroidNotificationParams Android { get; set; }

        /// <summary>
        /// Indicates the badge on the client app home icon. iOS only.
        /// </summary>
        [JsonProperty("badge", NullValueHandling = NullValueHandling.Ignore)]
        public string Badge { get; set; }
        /// <summary>
        /// Indicates notification body text.
        /// </summary>
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public string Body { get; set; }

        /// <summary>
        /// Indicates the string value to replace format specifiers in body string for localization.
        /// On iOS, this corresponds to "loc-args" in APNS payload.
        /// On Android, these are the format arguments for the string resource. For more information, see Formatting strings.
        /// </summary>
        [JsonProperty("bodyLocalizationArgs", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> BodyLocalizationArgs { get; set; }

        /// <summary>
        /// Indicates the key to the body string for localization.
        /// On iOS, this corresponds to "loc-key" in APNS payload.
        /// On Android, use the key in the app's string resources when populating this value.
        /// </summary>
        [JsonProperty("bodyLocalizationKey", NullValueHandling = NullValueHandling.Ignore)]
        public string BodyLocalizationKey { get; set; }

        /// <summary>
        /// The action associated with a user click on the notification.
        /// On Android, if this is set, an activity with a matching intent filter is launched when user clicks the notification.
        /// If set on iOS, corresponds to category in APNS payload.
        /// </summary>
        [JsonProperty("clickAction", NullValueHandling = NullValueHandling.Ignore)]
        public string ClickAction { get; set; }


        /// <summary>
        /// Indicates color of the icon, expressed in #rrggbb format.
        /// </summary>
        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        /// <summary>
        /// Indicates notification icon.
        /// Sets value to myicon for drawable resource myicon.
        /// </summary>
        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }

        /// <summary>
        /// Indicates a sound to play when the device receives the notification.
        /// Supports default, or the filename of a sound resource bundled in the app.
        /// </summary>
        [JsonProperty("sound", NullValueHandling = NullValueHandling.Ignore)]
        public string Sound { get; set; }
        /// <summary>
        /// Indicates whether each notification results in a new entry in the notification drawer on Android.
        /// </summary>
        [JsonProperty("tag", NullValueHandling = NullValueHandling.Ignore)]
        public string Tag { get; set; }

        /// <summary>
        /// Indicates notification title.
        /// </summary>
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("titleLocalizationArgs", NullValueHandling = NullValueHandling.Ignore)]
        string[] titleLocalizationArgs { get; set; }

        /// <summary>
        /// Indicates the string value to replace format specifiers in title string for localization.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<string> TitleLocalizationArgs
        {
            get
            {
                IEnumerable<string> result = null;
                if (titleLocalizationArgs == null)
                    result = new string[0];
                else
                    result = Array.AsReadOnly(titleLocalizationArgs);
                return result;
            }
        }
        [JsonProperty("titleLocalizationKey", NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// Indicates the key to the title string for localization.
        /// On iOS, this corresponds to "title-loc-key" in APNS payload.
        /// On Android, use the key in the app's string resources when populating this value.
        /// </summary>
        public string TitleLocalizationKey { get; private set; }
        public void Dispose()
        {
            
        }

        ~FirebaseNotification()
        {
            this.Dispose();
        }
        
    }
}
