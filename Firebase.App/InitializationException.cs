using System;

namespace Firebase
{
    /// <summary>
    /// The exception that is thrown when a problem occurs with initialization of a Firebase module or class.
    /// </summary>
    public sealed class InitializationException : Exception
    {
        /// <summary>
        /// The error code describing the cause of the failure.
        /// </summary>
        public InitResult InitResult
        {
            get;
            private set;
        }
        /// <summary>
        /// Initializes a new <see cref="InitializationException"/>, with the given result.
        /// </summary>
        /// <param name="result"></param>
        public InitializationException(InitResult result)
        {
            this.InitResult = result;
        }
        /// <summary>
        /// Initializes a new <see cref="InitializationException"/>, with the given result and message.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        public InitializationException(InitResult result, string message) : base(message)
        {
            this.InitResult = result;
        }
        /// <summary>
        /// Initializes a new <see cref="InitializationException"/>, with the given result, message, and a reference to the inner exception.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InitializationException(InitResult result, string message, Exception inner) : base(message, inner)
        {
            this.InitResult = result;
        }
    }
}
