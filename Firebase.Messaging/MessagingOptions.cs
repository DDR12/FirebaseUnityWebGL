using System;

namespace Firebase.Messaging
{
    /// <summary>
    /// A class to configure the behavior of Firebase Cloud Messaging.
    /// </summary>
    public sealed class MessagingOptions : IDisposable
    {
        public bool SuppressNotificationPermissionPrompt
        {
            get
            {
                PlatformHandler.NotifyFeatureIsUselessInWebGL();
                return false;
            }
            set
            {
                PlatformHandler.NotifyFeatureIsUselessInWebGL();
            }
        }
        public void Dispose()
        {

        }
    }
}
