using System;

/// <summary>
/// 
/// </summary>
public interface IPromiseTimer
{
    /// <summary>
    /// Resolve the returned promise once the time has elapsed
    /// </summary>
    IPromise WaitFor(float seconds);

    /// <summary>
    /// Resolve the returned promise once the predicate evaluates to true
    /// </summary>
    IPromise WaitUntil(Func<TimeData, bool> predicate);

    /// <summary>
    /// Resolve the returned promise once the predicate evaluates to false
    /// </summary>
    IPromise WaitWhile(Func<TimeData, bool> predicate);

    /// <summary>
    /// Update all pending promises. Must be called for the promises to progress and resolve at all.
    /// </summary>
    void Update(float deltaTime);

    /// <summary>
    /// Cancel a waiting promise and reject it immediately.
    /// </summary>
    bool Cancel(IPromise promise);
}
