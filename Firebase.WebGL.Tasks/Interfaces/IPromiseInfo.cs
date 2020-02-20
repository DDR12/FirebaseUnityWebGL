
/// <summary>
/// Used to list information of pending promises.
/// </summary>
public interface IPromiseInfo
{
    /// <summary>
    /// Id of the promise.
    /// </summary>
    uint Id { get; }

    /// <summary>
    /// Human-readable name for the promise.
    /// </summary>
    string Name { get; }
}
