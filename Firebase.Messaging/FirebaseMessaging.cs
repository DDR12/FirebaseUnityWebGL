using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Firebase.Messaging
{
    public sealed class FirebaseMessaging : IDisposable
    {
        static Dictionary<string, FirebaseMessaging> messagingServices;

        static FirebaseMessaging defaultInstance;
        public static FirebaseMessaging DefaultInstance
        {
            get
            {
                if (defaultInstance == null)
                    defaultInstance = GetInstance(FirebaseApp.DefaultInstance);
                return defaultInstance;
            }
        }
        /// <summary>
        /// Returns true if this browser supports messaging and notifications.
        /// This is always true in the editor.
        /// </summary>
        public static bool IsSupported
        {
            get
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                return MessagingPInvoke.IsMessagingSupported_WebGL();
#else
                return true;
#endif   
            }
        }

        static FirebaseMessaging()
        {
            messagingServices = new Dictionary<string, FirebaseMessaging>();
        }

        public FirebaseApp App { get; }
        private static string m_Token = null;
        public static event EventHandler<MessageReceivedEventArgs> MessageReceived
        {
            add
            {
                DefaultInstance.OnMessageReceived += value;
            }
            remove
            {
                DefaultInstance.OnMessageReceived -= value;
            }
        }


        private event EventHandler<MessageReceivedEventArgs> MessageReceivedEvent;
        /// <summary>
        /// Called on the client when a message arrives.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived
        {
            add
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.SubscribeToEvent(MessageReceivedEvent, value);
                if (id != null)
                    MessagingPInvoke.SubscribeToOnMessage_WebGL(App.Name, id.Value, OnMessageReceivedCallback_AOT);

            }
            remove
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.UnsubscribeFromEvent(MessageReceivedEvent, value);
                if (id != null)
                    MessagingPInvoke.UnsubscribeToOnMessage_WebGL(id.Value);

            }
        }

        public static event EventHandler<TokenReceivedEventArgs> TokenReceived
        {
            add
            {
                DefaultInstance.OnTokenReceived += value;
            }
            remove
            {
                DefaultInstance.OnTokenReceived -= value;
            }
        }

        private event EventHandler<TokenReceivedEventArgs> OnTokenReceivedEvent;
        /// <summary>
        /// Called on the client when a registration token message arrives.
        /// </summary>
        public event EventHandler<TokenReceivedEventArgs> OnTokenReceived
        {
            add
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.SubscribeToEvent(OnTokenReceivedEvent, value);
                if (id != null)
                    MessagingPInvoke.SubscribeToMessagingTokenReceived_WebGL(App.Name, id.Value, OnMessagingTokenReceivedCallback_AOT);

            }
            remove
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.UnsubscribeFromEvent(OnTokenReceivedEvent, value);
                if (id != null)
                    MessagingPInvoke.UnsubscribeToMessagingTokenReceived_WebGL(id.Value);

            }
        }
        /// <summary>
        /// Enable or disable token registration during initialization of Firebase Cloud Messaging.
        /// This token is what identifies the user to Firebase, so disabling this avoids creating any new identity and automatically sending it to Firebase, unless consent has been granted.
        /// If this setting is enabled, it triggers the token registration refresh immediately.
        /// This setting is persisted across app restarts.
        /// By default, token registration during initialization is enabled.
        /// </summary>
        public static bool TokenRegistrationOnInitEnabled
        {
            get => MessagingPInvoke.GetTokenRegistrationOnInitEnabled_WebGL();
            set => MessagingPInvoke.SetTokenRegistrationOnInitEnabled_WebGL(value);
        }
        private FirebaseMessaging(FirebaseApp app)
        {
            App = app;
            if (TokenRegistrationOnInitEnabled)
            {
                RequestPermissionAsync();
            }
        }
        ~FirebaseMessaging()
        {
            Dispose();
        }
        private void NotifyMessage(string payloadJson, string errorJson)
        {
            if (!string.IsNullOrWhiteSpace(errorJson))
            {
                UnityEngine.Debug.Log($"An error occured while receiving a callback to OnMessage, error: {errorJson}");
                return;
            }
            FirebaseMessage message = JsonConvert.DeserializeObject<FirebaseMessage>(payloadJson);
            MessageReceivedEvent?.Invoke(this, new MessageReceivedEventArgs(message));
        }
        private void NotifyTokenReceived(string tokenJson,string errorJson)
        {
            if(!string.IsNullOrWhiteSpace(errorJson))
            {
                UnityEngine.Debug.Log($"An error occured while receiving a callback to TokenReceieved, error: {errorJson}");
                return;
            }
            m_Token = JsonConvert.DeserializeObject<string>(tokenJson);
            OnTokenReceivedEvent?.Invoke(this, new TokenReceivedEventArgs(m_Token));
        }
        public static void UseVapidKey()
        {
            MessagingPInvoke.UseVapidKey_WebGL(DefaultInstance.App.Name, DefaultInstance.App.Options.VapidPublicKey);
        }
        /// <summary>
        /// Subscribe to receive all messages to the specified topic.
        /// Subscribes an app instance to a topic, enabling it to receive messages sent to that topic.
        /// </summary>
        /// <param name="topic">The name of the topic to subscribe. Must match the following regular expression: [a-zA-Z0-9-_.~%]{1,900}.</param>
        /// <returns></returns>
        public Task SubscribeAsync(string topic)
        {
            throw new NotImplementedException("Currently Subscribing to Topic is not supported by javascript library for the safety of your server token.");
        }
        /// <summary>
        /// Unsubscribe from a topic.
        /// Unsubscribes an app instance from a topic, stopping it from receiving any further messages sent to that topic.
        /// </summary>
        /// <param name="topic">The name of the topic to unsubscribe from. Must match the following regular expression: [a-zA-Z0-9-_.~%]{1,900}.</param>
        /// <returns></returns>
        public Task UnsubscribeAsync(string topic)
        {
            throw new NotImplementedException("Currently UnSubscribing from Topic is not supported by javascript library for the safety of your server token.");
        }

        /// <summary>
        /// To forceably stop a registration token from being used, delete it by calling this method.
        /// </summary>
        /// <param name="token">The token to delete.</param>
        /// <returns>A task that resolves when the token has been successfully deleted.</returns>
        public Task<bool> DeleteTokenAsync(string token)
        {
            var task = WebGLTaskManager.GetTask<bool>();
            MessagingPInvoke.DeleteMessagingToken_WebGL(task.Task.Id, App.Name, token, WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// After <see cref="RequestPermissionAsync"/> you can call this method to get an FCM registration token that can be used to send push messages to this user.
        /// </summary>
        /// <returns>The task resolves if an FCM token can be retrieved. This method returns null if the current origin does not have permission to show notifications.</returns>
        public Task<string> GetToken()
        {
            var task = WebGLTaskManager.GetTask<string>();
            MessagingPInvoke.GetMessagingToken_WebGL(task.Task.Id, App.Name, WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// Displays a consent dialog to let users grant your app permission to receive notifications in the browser or the app.
        /// If permission is denied, FCM registration token requests result in an error.
        /// </summary>
        /// <returns>A task that completes when the notification prompt has been dismissed.</returns>
        public static Task RequestPermissionAsync()
        {
            var task = WebGLTaskManager.GetTask<bool>();
            task.Task.ContinueWith(permissionTask =>
            {
                bool granted = permissionTask.Result;
                if (!granted)
                {
                    Debug.Log($"With TokenRegistrationOnInitEnabled, failed to get the device token, the user denied notifications permission.");
                    return;
                }
                DefaultInstance.GetToken().ContinueWith(result =>
                {
                    if (result.IsSuccess())
                    {
                        m_Token = result.Result;
                    }
                    else
                    {
                        if (result.IsFaulted)
                            Debug.Log($"Failed to get the device token, error: {result.Exception.InnerException.Message}");
                        else if (result.IsCanceled)
                            Debug.Log($"Failed to get the device token, the operation was cancelled.");
                    }
                });
            });
            MessagingPInvoke.RequestNotificationPermission_WebGL(task.Task.Id, WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// Creates an instance of <see cref="FirebaseMessaging"/> that belongs to the specified <see cref="FirebaseApp"/> instance.
        /// </summary>
        /// <param name="app">The app to which a messaging service will be created.</param>
        /// <returns>The existing messaging service for the specified app if it exists, or a new one.</returns>
        public static FirebaseMessaging GetInstance(FirebaseApp app)
        {
            if (!IsSupported)
                throw new NotImplementedException("Push notifications are not supported on this browser.");

            app = app ?? FirebaseApp.DefaultInstance;
            if (messagingServices.TryGetValue(app.Name, out FirebaseMessaging messaging))
                return messaging;
            messaging = new FirebaseMessaging(app);
            messagingServices.Add(app.Name, messaging);
            return messaging;
        }


        [AOT.MonoPInvokeCallback(typeof(MessageReceivedWebGLCallback))]
        static void OnMessageReceivedCallback_AOT(string appName, string payloadJson, string errorJson)
        {
            if (messagingServices.TryGetValue(appName, out FirebaseMessaging messaging))
                messaging.NotifyMessage(payloadJson, errorJson);
        }


        [AOT.MonoPInvokeCallback(typeof(TokenReceivedWebGLCallback))]
        static void OnMessagingTokenReceivedCallback_AOT(string appName, string tokenJson, string jsonErrorOrNull)
        {
            if (messagingServices.TryGetValue(appName, out FirebaseMessaging messaging))
                messaging.NotifyTokenReceived(tokenJson, jsonErrorOrNull);
        }

        public void Dispose()
        {
            messagingServices.Remove(App.Name);
        }
    }
}

