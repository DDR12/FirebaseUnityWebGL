/// <summary>
/// Specifies the state of a promise.
/// </summary>
public enum PromiseState
{
    /// <summary>
    /// The promise is in-flight.
    /// </summary>
    Pending,
    /// <summary>
    /// The promise has been rejected.
    /// </summary>
    Rejected,
    /// <summary>
    ///  The promise has been resolved.
    /// </summary>
    Resolved
}
