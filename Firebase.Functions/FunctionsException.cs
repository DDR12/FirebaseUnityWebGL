using System;

namespace Firebase.Functions
{
    /// <summary>
    /// Represents an Exception resulting from an operation on a FunctionsReference.
    /// </summary>
    public sealed class FunctionsException : Exception
    {
        public FunctionsErrorCode ErrorCode { get; private set; }
        internal FunctionsException(FirebaseException e) : base(e.Message)
        {
            this.ErrorCode = (FunctionsErrorCode)e.ErrorCode;
        }
    }
}
