using System;
using System.Collections.Generic;
using System.Net;

namespace Firebase.Storage
{
    public sealed class StorageException : Exception
    {
        public const int ErrorUnknown = -13000;

        public const int ErrorObjectNotFound = -13010;

        public const int ErrorBucketNotFound = -13011;

        public const int ErrorProjectNotFound = -13012;

        public const int ErrorQuotaExceeded = -13013;

        public const int ErrorNotAuthenticated = -13020;

        public const int ErrorNotAuthorized = -13021;

        public const int ErrorRetryLimitExceeded = -13030;

        public const int ErrorInvalidChecksum = -13031;

        public const int ErrorCanceled = -13040;

        private readonly static Dictionary<ErrorInternal, Tuple<int, HttpStatusCode>> errorToStorageErrorAndHttpStatusCode;

        private readonly static Tuple<int, HttpStatusCode> unknownError;

        public int ErrorCode
        {
            get;
            private set;
        }

        public int HttpResultCode
        {
            get;
            private set;
        }

        public bool IsRecoverableException
        {
            get
            {
                return this.ErrorCode == -13030;
            }
        }

        static StorageException()
        {
            Dictionary<ErrorInternal, Tuple<int, HttpStatusCode>> errorInternals = new Dictionary<ErrorInternal, Tuple<int, HttpStatusCode>>()
            {
                { ErrorInternal.ErrorObjectNotFound, new Tuple<int, HttpStatusCode>(-13010, HttpStatusCode.NotFound) },
                { ErrorInternal.ErrorBucketNotFound, new Tuple<int, HttpStatusCode>(-13011, HttpStatusCode.NotFound) },
                { ErrorInternal.ErrorProjectNotFound, new Tuple<int, HttpStatusCode>(-13012, HttpStatusCode.NotFound) },
                { ErrorInternal.ErrorQuotaExceeded, new Tuple<int, HttpStatusCode>(-13012, HttpStatusCode.ServiceUnavailable) },
                { ErrorInternal.ErrorUnauthenticated, new Tuple<int, HttpStatusCode>(-13020, HttpStatusCode.Unauthorized) },
                { ErrorInternal.ErrorUnauthorized, new Tuple<int, HttpStatusCode>(-13021, HttpStatusCode.Unauthorized) },
                { ErrorInternal.ErrorRetryLimitExceeded, new Tuple<int, HttpStatusCode>(-13030, HttpStatusCode.Conflict) },
                { ErrorInternal.ErrorNonMatchingChecksum, new Tuple<int, HttpStatusCode>(-13031, HttpStatusCode.Conflict) },
                { ErrorInternal.ErrorDownloadSizeExceeded, new Tuple<int, HttpStatusCode>(-13000, 0) },
                { ErrorInternal.ErrorCancelled, new Tuple<int, HttpStatusCode>(-13040, 0) }
            };
            StorageException.errorToStorageErrorAndHttpStatusCode = errorInternals;
            unknownError = new Tuple<int, HttpStatusCode>(-13000, HttpStatusCode.MultipleChoices);
        }

        internal StorageException(int errorCode, int httpResultCode, string errorMessage) : base((!string.IsNullOrEmpty(errorMessage) ? errorMessage : StorageException.GetErrorMessageForCode(errorCode)))
        {
            this.ErrorCode = errorCode;
            this.HttpResultCode = httpResultCode;
        }

        internal static StorageException CreateFromException(Exception exception)
        {
            Tuple<int, HttpStatusCode> tuple;
            StorageException storageException;
            AggregateException aggregateException = (AggregateException)exception;
            FirebaseException firebaseException = null;
            string message = null;
            using (IEnumerator<Exception> enumerator = aggregateException.InnerExceptions.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Exception current = enumerator.Current;
                    StorageException storageException1 = current as StorageException;
                    firebaseException = current as FirebaseException;
                    if (storageException1 == null)
                    {
                        if (firebaseException == null)
                        {
                            continue;
                        }
                        break;
                    }
                    else
                    {
                        storageException = storageException1;
                        return storageException;
                    }
                }
                if (firebaseException != null)
                {
                    message = firebaseException.Message;
                    if (!StorageException.errorToStorageErrorAndHttpStatusCode.TryGetValue((ErrorInternal)firebaseException.ErrorCode, out tuple))
                    {
                        tuple = StorageException.unknownError;
                    }
                }
                else
                {
                    message = exception.ToString();
                    tuple = StorageException.unknownError;
                }
                int item2 = (int)tuple.Item2;
                return new StorageException(tuple.Item1, item2, message);
            }
        }

        internal static string GetErrorMessageForCode(int errorCode)
        {
            switch (errorCode)
            {
                case -13013:
                    {
                        return "Quota for bucket exceeded, please view quota on www.firebase.google.com/storage.";
                    }
                case -13012:
                    {
                        return "Project does not exist.";
                    }
                case -13011:
                    {
                        return "Bucket does not exist.";
                    }
                case -13010:
                    {
                        return "Object does not exist at location.";
                    }
                default:
                    {
                        if (errorCode == -13031)
                        {
                            break;
                        }
                        else
                        {
                            if (errorCode == -13030)
                            {
                                return "The operation retry limit has been exceeded.";
                            }
                            if (errorCode == -13021)
                            {
                                return "User does not have permission to access this object.";
                            }
                            if (errorCode == -13020)
                            {
                                return "User is not authenticated, please authenticate using Firebase Authentication and try again.";
                            }
                            if (errorCode == -13040)
                            {
                                return "The operation was cancelled.";
                            }
                            if (errorCode == -13000)
                            {
                                return "An unknown error occurred, please check the HTTP result code and inner exception for server response.";
                            }
                            return "An unknown error occurred, please check the HTTP result code and inner exception for server response.";
                        }
                    }
            }
            return "Object has a checksum which does not match. Please retry the operation.";
        }
    }
}
