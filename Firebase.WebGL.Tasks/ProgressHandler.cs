using System;
/// <summary>
/// 
/// </summary>
public struct ProgressHandler
{
    /// <summary>
    /// Callback fn.
    /// </summary>
    public Action<float> callback;

    /// <summary>
    /// The promise that is rejected when there is an error while invoking the handler.
    /// </summary>
    public IRejectable rejectable;
}
