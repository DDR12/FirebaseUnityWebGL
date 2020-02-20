using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Firebase.Messaging
{
    /// <summary>
    /// Data structure used to send messages to, and receive messages from, cloud messaging.
    /// </summary>
    public sealed class FirebaseMessage : IDisposable
    {
        [JsonProperty("notification")]
        public FirebaseNotification Notification { get; private set; }

        [JsonProperty("time_to_live")]
        private ulong Lifespan { get; set; }

        /// <summary>
        /// Gets the priority level.
        /// Defined values are "normal" and "high". By default messages are sent with normal priority.
        /// This field is only used for downstream messages received through the <see cref="FirebaseMessaging.OnMessageReceived"/> event.
        /// /// </summary>
        [JsonProperty("priority")]
        public string Priority { get; private set; }
        /// <summary>
        /// The Time To Live (TTL) for the message.
        /// This field is only used for downstream messages received through <see cref="FirebaseMessaging.OnMessageReceived"/>.
        /// </summary>
        [JsonIgnore]
        public TimeSpan TimeToLive
        {
            get
            {
                return TimeSpan.FromSeconds(Lifespan);
            }
        }

        public string RawData { get; set; }
        /// <summary>
        /// Gets the collapse key used for collapsible messages.
        /// </summary>
        [JsonProperty("collapseKey")]
        public string CollapseKey { get; set; }
        /// <summary>
        /// Gets or sets the metadata, including all original key/value pairs.
        /// Includes some of the HTTP headers used when sending the message. gcm, google and goog prefixes are reserved for internal use.
        /// This field is used for both upstream messages sent with firebase::messaging::Send() and downstream messages received through the <see cref="FirebaseMessaging.OnMessageReceived"/> event.
        /// </summary>
        [JsonProperty("data")]
        public IDictionary<string, string> Data { get; set; }
        /// <summary>
        /// Gets the error code.
        /// Used in "nack" messages for CCS, and in responses from the server.See the CCS specification for the externally-supported list.
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }
        /// <summary>
        /// Gets the human readable details about the error.
        /// </summary>
        [JsonProperty("errorDescription")]
        public string ErrorDescription { get; set; }
        /// <summary>
        /// Gets the authenticated ID of the sender.
        /// This is a project number in most cases.
        /// </summary>
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("original_priority")]
        public string Original_priority { get; private set; }

        /// <summary>
        /// The link into the app from the message.
        /// This field is only used for downstream messages.
        /// </summary>
        [JsonProperty("link")]
        public Uri Link { get; set; }

        /// <summary>
        /// Gets or sets the message ID.
        /// This can be specified by sender.Internally a hash of the message ID and other elements will be used for storage.
        /// The ID must be unique for each topic subscription - using the same ID may result in overriding the original message or duplicate delivery.
        /// </summary>
        [JsonProperty("messageId")]
        public string MessageId { get; set; }
        /// <summary>
        /// Gets the message type, equivalent with a content-type.
        /// CCS uses "ack", "nack" for flow control and error handling. "control" is used by CCS for connection control.
        /// </summary>
        [JsonProperty("messageType")]
        public string MessageType { get; set; }
        /// <summary>
        /// Gets a flag indicating whether this message was opened by tapping a notification.
        /// If the message was received this way this flag is set to true.
        /// </summary>
        [JsonProperty("notificatiOnopened")]
        public bool NotificationOpened { get; set; }

        /// <summary>
        /// Gets or sets recipient of a message.
        /// For example it can be a registration token, a topic name, a IID or project ID.
        /// This field is used for both upstream messages sent with firebase::messaging:Send() and downstream messages received through the FirebaseMessaging.MessageReceived event. 
        /// For upstream messages, PROJECT_ID @gcm.googleapis.com or the more general IID format are accepted.
        /// </summary>
        [JsonProperty("to")]
        public string To { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonConstructor]
        public FirebaseMessage() { }
        public void Dispose()
        {

        }
    }
}
