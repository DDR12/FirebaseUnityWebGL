using System;
/// <summary>
/// Interface for a promise that can be rejected.
/// </summary>
public interface IRejectable
    {
        /// <summary>
        /// Reject the promise with an exception.
        /// </summary>
        void SetException(Exception ex);
    }
