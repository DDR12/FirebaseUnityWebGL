using System;

namespace Firebase.Messaging
{
    /// <summary>
    /// Token argument for the TokenReceived event containing the token string.
    /// </summary>
    public sealed class TokenReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// An identity token string provided by the TokenReceived event handler.
        /// </summary>
        public string Token
        {
            get;
            set;
        }

        public TokenReceivedEventArgs(string token)
        {
            this.Token = token;
        }
    }
}