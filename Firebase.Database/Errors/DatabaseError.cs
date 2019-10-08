using System;
using System.Collections.Generic;

namespace Firebase.Database
{
    public sealed class DatabaseError
    {
        internal const int DataStale = -1;
        /// <summary>
        /// The server indicated that this operation failed.
        /// </summary>
        public const int OperationFailed = -2;
        /// <summary>
        /// This client does not have permission to perform this operation.
        /// </summary>
        public const int PermissionDenied = -3;
        /// <summary>
        /// The operation had to be aborted due to a network disconnect.
        /// </summary>
        public const int Disconnected = -4;
        /// <summary>
        /// The supplied auth token has expired.
        /// </summary>
        public const int ExpiredToken = -6;
        /// <summary>
        /// The specified authentication token is invalid.
        /// </summary>
        public const int InvalidToken = -7;
        /// <summary>
        /// The transaction had too many retries.
        /// </summary>
        public const int MaxRetries = -8;
        /// <summary>
        /// The transaction was overridden by a subsequent set.
        /// </summary>
        public const int OverriddenBySet = -9;
        /// <summary>
        /// The service is unavailable.
        /// </summary>
        public const int Unavailable = -10;
        /// <summary>
        /// An exception occurred in user code.
        /// </summary>
        public const int UserCodeException = -11;
        /// <summary>
        /// The operation could not be performed due to a network error.
        /// </summary>
        public const int NetworkError = -24;
        /// <summary>
        /// The write was canceled locally.
        /// </summary>
        public const int WriteCanceled = -25;
        /// <summary>
        /// An unknown error occurred.
        /// </summary>
        public const int UnknownError = -999;

        private readonly static IDictionary<int, string> ErrorReasons;

        private readonly static IDictionary<string, int> ErrorCodes;

        /// <summary>
        /// One of the defined status codes declared under <see cref="DatabaseError"/>, depending on the error
        /// </summary>
        public int Code
        {
            get;
            private set;
        }
        /// <summary>
        /// Human-readable details on the error and additional information.
        /// </summary>
        public string Details
        {
            get;
            private set;
        }
        /// <summary>
        /// A human-readable description of the error
        /// </summary>
        public string Message
        {
            get;
            private set;
        }

        static DatabaseError()
        {
            DatabaseError.ErrorReasons = new Dictionary<int, string>();
            DatabaseError.ErrorCodes = new Dictionary<string, int>();
            DatabaseError.ErrorReasons[-1] = "The transaction needs to be Run again with current data";
            DatabaseError.ErrorReasons[-2] = "The server indicated that this operation failed";
            DatabaseError.ErrorReasons[-3] = "This client does not have permission to perform this operation";
            DatabaseError.ErrorReasons[-4] = "The operation had to be aborted due to a network disconnect";
            DatabaseError.ErrorReasons[-6] = "The supplied auth token has expired";
            DatabaseError.ErrorReasons[-7] = "The supplied auth token was invalid";
            DatabaseError.ErrorReasons[-8] = "The transaction had too many retries";
            DatabaseError.ErrorReasons[-9] = "The transaction was overridden by a subsequent set";
            DatabaseError.ErrorReasons[-10] = "The service is unavailable";
            DatabaseError.ErrorReasons[-11] = "User code called from the Firebase Database runloop threw an exception:\n";
            DatabaseError.ErrorReasons[-24] = "The operation could not be performed due to a network error";
            DatabaseError.ErrorReasons[-25] = "The write was canceled by the user.";
            DatabaseError.ErrorReasons[-999] = "An unknown error occurred";
            DatabaseError.ErrorCodes["datastale"] = -1;
            DatabaseError.ErrorCodes["failure"] = -2;
            DatabaseError.ErrorCodes["permission_denied"] = -3;
            DatabaseError.ErrorCodes["disconnected"] = -4;
            DatabaseError.ErrorCodes["expired_token"] = -6;
            DatabaseError.ErrorCodes["invalid_token"] = -7;
            DatabaseError.ErrorCodes["maxretries"] = -8;
            DatabaseError.ErrorCodes["overriddenbyset"] = -9;
            DatabaseError.ErrorCodes["unavailable"] = -10;
            DatabaseError.ErrorCodes["network_error"] = -24;
            DatabaseError.ErrorCodes["write_canceled"] = -25;
        }

        private DatabaseError(int code, string message) : this(code, message, null)
        {
        }

        private DatabaseError(int code, string message, string details)
        {
            this.Code = code;
            this.Message = message;
            this.Details = (details ?? string.Empty);
        }

        internal static int ErrorToCode(Error error)
        {
            switch (error)
            {
                case Error.Disconnected:
                    {
                        return -4;
                    }
                case Error.ExpiredToken:
                    {
                        return -6;
                    }
                case Error.InvalidToken:
                    {
                        return -7;
                    }
                case Error.MaxRetries:
                    {
                        return -8;
                    }
                case Error.NetworkError:
                    {
                        return -24;
                    }
                case Error.OperationFailed:
                    {
                        return -2;
                    }
                case Error.OverriddenBySet:
                    {
                        return -9;
                    }
                case Error.PermissionDenied:
                    {
                        return -3;
                    }
                case Error.Unavailable:
                    {
                        return -10;
                    }
                case Error.UnknownError:
                    {
                        return -999;
                    }
                case Error.WriteCanceled:
                    {
                        return -25;
                    }
                case Error.InvalidVariantType:
                case Error.ConflictingOperationInProgress:
                case Error.TransactionAbortedByUser:
                    {
                        return -999;
                    }
                default:
                    {
                        return -999;
                    }
            }
        }

        internal static DatabaseError FromCode(int code)
        {
            if (!DatabaseError.ErrorReasons.ContainsKey(code))
            {
                throw new ArgumentException(string.Concat("Invalid Firebase Database error code: ", code));
            }
            return new DatabaseError(code, DatabaseError.ErrorReasons[code], null);
        }

        internal static DatabaseError FromError(Error error, string msg)
        {
            int code = DatabaseError.ErrorToCode(error);
            return new DatabaseError(code, (msg == null || msg == string.Empty ? DatabaseError.ErrorReasons[code] : msg), null);
        }
        internal static DatabaseError FromError(FirebaseError error)
        {
            return new DatabaseError(error.ErrorCode, $"{error.ErrorCode}: {error.Message}");
        }
        internal static DatabaseError FromException(Exception e)
        {
            string str = string.Concat(DatabaseError.ErrorReasons[-11], e.Message);
            return new DatabaseError(-11, str);
        }

        internal static DatabaseError FromStatus(string status)
        {
            return DatabaseError.FromStatus(status, null);
        }

        internal static DatabaseError FromStatus(string status, string reason)
        {
            return DatabaseError.FromStatus(status, reason, null);
        }

        internal static DatabaseError FromStatus(string status, string reason, string details)
        {
            if (!DatabaseError.ErrorCodes.TryGetValue(status.ToLower(), out int num))
            {
                num = -999;
            }
            return new DatabaseError(num, (reason ?? DatabaseError.ErrorReasons[num]), details);
        }

        /// <summary>
        /// Can be used if a third party needs an Exception from <see cref="FirebaseDatabase"/> for integration purposes.
        /// </summary>
        /// <returns></returns>
        public DatabaseException ToException()
        {
            return new DatabaseException(string.Concat("Firebase Database error: ", this.Message));
        }

        public override string ToString()
        {
            return string.Concat("DatabaseError: ", this.Message);
        }
    }
}