using System;

namespace Firebase.Messaging
{
    /// <summary>
    /// Data structure for parameters that are unique to the Android implementation.
    /// </summary>
    public class AndroidNotificationParams : IDisposable
    {
        public string ChannelId { get; set; }
        
        public virtual void Dispose()
        {
        }

        ~AndroidNotificationParams()
        {
            this.Dispose();
        }
    }
}
