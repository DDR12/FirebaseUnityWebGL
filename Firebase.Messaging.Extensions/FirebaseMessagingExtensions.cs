namespace Firebase.Messaging
{
    /// <summary>
    /// This class helps match different platform sdks, when an sdk is missing a method but the other provides it, this class is a meeting point which both implementations can be accessed using same code instead of the #if #else directive nightmare.
    /// </summary>
    public static class FirebaseMessagingExtensions
    {
        /// <summary>
        /// [Effective only for Web Builds]
        /// The FCM Web interface uses Web credentials called "Voluntary Application Server Identification," or "VAPID" keys, to authorize send requests to supported web push services. 
        /// To subscribe your app to push notifications, you need to associate a pair of keys with your Firebase project. 
        /// You can either generate a new key pair or import your existing key pair through the Firebase Console.
        /// </summary>
        public static void UseVapidKey()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            FirebaseMessaging.UseVapidKey();
#else
            
#endif

        }
    }
}
