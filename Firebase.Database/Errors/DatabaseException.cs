using System;

namespace Firebase.Database
{
    /// <summary>
    /// This error is thrown when the <see cref="FirebaseDatabase"/> library is unable to operate on the input it has been given.
    /// </summary>
    public sealed class DatabaseException : Exception
    {
        internal DatabaseException(string message) : base(message)
        {
        }

        internal DatabaseException(string message, Exception cause) : base(message, cause)
        {
        }
    }
}