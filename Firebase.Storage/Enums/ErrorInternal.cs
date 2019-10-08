namespace Firebase.Storage
{
    internal enum ErrorInternal
    {
        ErrorNone,
        ErrorUnknown,
        ErrorObjectNotFound,
        ErrorBucketNotFound,
        ErrorProjectNotFound,
        ErrorQuotaExceeded,
        ErrorUnauthenticated,
        ErrorUnauthorized,
        ErrorRetryLimitExceeded,
        ErrorNonMatchingChecksum,
        ErrorDownloadSizeExceeded,
        ErrorCancelled
    }
}