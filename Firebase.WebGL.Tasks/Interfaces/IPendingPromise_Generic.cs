
/// <summary>
/// Interface for a promise that can be rejected or resolved.
/// </summary>
public interface IPendingPromise<PromisedT> : IRejectable
{
    /// <summary>
    /// ID of the promise, useful for debugging.
    /// </summary>
    uint Id { get; }

    /// <summary>
    /// Resolve the promise with a particular value.
    /// </summary>
    void SetResult(PromisedT value);

    /// <summary>
    /// Report progress in a promise.
    /// </summary>
    void ReportProgress(float progress);
}
