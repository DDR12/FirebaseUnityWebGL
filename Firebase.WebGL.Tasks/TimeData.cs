
/// <summary>
/// Time data specific to a particular pending promise.
/// </summary>
public struct TimeData
{
    /// <summary>
    /// The amount of time that has elapsed since the pending promise started running
    /// </summary>
    public float elapsedTime;

    /// <summary>
    /// The amount of time since the last time the pending promise was updated.
    /// </summary>
    public float deltaTime;

    /// <summary>
    /// The amount of times that update has been called since the pending promise started running
    /// </summary>
    public int elapsedUpdates;
}
