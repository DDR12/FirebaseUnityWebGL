namespace Firebase.Database
{
    internal enum Error
    {
        None,
        Disconnected,
        ExpiredToken,
        InvalidToken,
        MaxRetries,
        NetworkError,
        OperationFailed,
        OverriddenBySet,
        PermissionDenied,
        Unavailable,
        UnknownError,
        WriteCanceled,
        InvalidVariantType,
        ConflictingOperationInProgress,
        TransactionAbortedByUser
    }
}