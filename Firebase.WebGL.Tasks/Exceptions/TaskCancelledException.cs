using System;
/// <summary>
/// 
/// </summary>
public class TaskCancelledException : Exception
{
    /// <summary>
    /// Just create the exception
    /// </summary>
    public TaskCancelledException()
    {

    }

    /// <summary>
    /// Create the exception with description
    /// </summary>
    /// <param name="message">Exception description</param>
    public TaskCancelledException(String message) : base(message)
    {

    }
}
