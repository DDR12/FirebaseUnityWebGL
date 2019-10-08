using System;

namespace Firebase.Storage
{
    /// <summary>
    /// A class that receives progress updates for storage uploads and downloads.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class StorageProgress<T> : IProgress<T>
    {
        private readonly Action<T> reportCallback;

        /// <summary>
        /// Creates an instance of a StorageProgress class.
        /// </summary>
        /// <param name="callback">A delegate that will be called periodically during a long running operation.</param>
        public StorageProgress(Action<T> callback)
        {
            reportCallback = callback;
        }
        /// <summary>
        /// Called periodically during a long running operation, this method will pass value to the delegate passed in the constructor.
        /// </summary>
        /// <param name="value">Current state of the long running operation.</param>
        public void Report(T value)
        {
            reportCallback?.Invoke(value);
        }
    }
}
