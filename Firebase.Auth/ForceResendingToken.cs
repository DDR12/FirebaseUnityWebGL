using System;

namespace Firebase.Auth
{
    /// <summary>
    /// Token to maintain current phone number verification session.
    /// </summary>
    public sealed class ForceResendingToken : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        ~ForceResendingToken()
        {
            Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }
    }
}
