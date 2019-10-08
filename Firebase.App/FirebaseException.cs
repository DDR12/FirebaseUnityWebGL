using System;

namespace Firebase
{
    /// <summary>
    /// Exception thrown for any Task/Promise exception.
    /// </summary>
    public sealed class FirebaseException : Exception
    {
        /// <summary>
        /// Returns the API-defined non-zero error code, in case 
        /// </summary>
        public int ErrorCode { get; private set; }

        string m_ErrorName = null;
        /// <summary>
        /// Errors from the javascript sdk don't always have an integer error code, some have strings.
        /// </summary>
        internal string ErrorName
        {
            get => m_ErrorName;
            set
            {
                m_ErrorName = value;
                if(int.TryParse(m_ErrorName, out int code))
                {
                    ErrorCode = code;
                }
                else
                {
                    ErrorCode = m_ErrorName.GetHashCode();
                }
            }
        }

        /// <summary>
        /// Initializes a new <see cref="FirebaseException"/>.
        /// </summary>
        public FirebaseException()
        {
            ErrorName = "unknown";
            ErrorCode = 0;
        }

        internal FirebaseException(string errorName)
        {
            ErrorName = errorName;
        }
        internal FirebaseException(string errorName, string message) : base(message)
        {
            ErrorName = errorName;
        }
        /// <summary>
        /// Initializes a new <see cref="FirebaseException"/>, with the given error code.
        /// </summary>
        public FirebaseException(int errorCode)
        {
            ErrorCode = errorCode;
        }
        /// <summary>
        /// Initializes a new <see cref="FirebaseException"/>, with the given error code and message.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        public FirebaseException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
        /// <summary>
        /// Initializes a new <see cref="FirebaseException"/>, with the given error code, message, and a reference to the inner exception.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public FirebaseException(int errorCode, string message, Exception inner) : base(message, inner)
        {
            this.ErrorCode = errorCode;
        }
    }
}
