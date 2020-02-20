
/// <summary>
/// Interface for a promise that can be rejected or resolved.
/// </summary>
public interface IPendingPromise : IRejectable
{
    /// <summary>
    /// ID of the promise, useful for debugging.
    /// </summary>
    uint Id { get; }

    /// <summary>
    /// Resolve the promise with a particular value.
    /// </summary>
    void Resolve();

    /// <summary>
    /// Report progress in a promise.
    /// </summary>
    void ReportProgress(float progress);
}
