using System.Runtime.InteropServices;
namespace Firebase.Messaging
{
    internal class MessagingPInvoke
    {
        [DllImport("__Internal")]
        public static extern bool IsMessagingSupported_WebGL();
        [DllImport("__Internal")]
        public static extern void RequestNotificationPermission_WebGL(int taskID, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SubscribeToOnMessage_WebGL(string appName, uint listenerID, MessageReceivedWebGLCallback callback);
        [DllImport("__Internal")]
        public static extern void UnsubscribeToOnMessage_WebGL(uint listenerID);
        [DllImport("__Internal")]
        public static extern void SubscribeToMessagingTokenReceived_WebGL(string appName, uint listenerID, TokenReceivedWebGLCallback callback);
        [DllImport("__Internal")]
        public static extern void UnsubscribeToMessagingTokenReceived_WebGL(uint listenerID);
        [DllImport("__Internal")]
        public static extern void DeleteMessagingToken_WebGL(int taskID, string appName, string tokenToDelete, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void GetMessagingToken_WebGL(int taskID, string appName, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void UseVapidKey_WebGL(string appName, string vapidKey);
        [DllImport("__Internal")]
        public static extern bool GetTokenRegistrationOnInitEnabled_WebGL();
        [DllImport("__Internal")]
        public static extern void SetTokenRegistrationOnInitEnabled_WebGL(bool value);


    }
}
