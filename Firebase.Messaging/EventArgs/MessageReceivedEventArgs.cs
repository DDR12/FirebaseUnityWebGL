using System;

namespace Firebase.Messaging
{
    /// <summary>
    /// Event argument for the MessageReceived event containing the message data.
    /// </summary>
    public sealed class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Message data passed to the MessageReceived event handler.
        /// </summary>
        public FirebaseMessage Message
        {
            get;
            set;
        }

        public MessageReceivedEventArgs(FirebaseMessage msg)
        {
            this.Message = msg;
        }
    }
}