using System;
namespace Firebase.WebGL.Threading
{
    /// <summary>
    /// Wrapper that encapsulates a void task running on the javascript side, and resolves that task when response is back from the browser.
    /// </summary>
    internal class WebGLTaskHandler : IWebGLTask
    {
        TaskCompletionSource<object> task = null;
        public WebGLTaskHandler(TaskCompletionSource<object> task)
        {
            this.task = task;
        }
        public void SetResult(string json, string error)
        {
            if (string.IsNullOrWhiteSpace(error))
            {
                task.SetResult(null);
            }
            else
            {
                FirebaseError firebaseError = FirebaseError.FromJson(error);
                FirebaseException exception = new FirebaseException(firebaseError.ErrorCode, firebaseError.Message);
                task.SetException(exception);
            }
        }

        public void SetResult(IntPtr pBuffer, int bufferLength, string error)
        {
            throw new InvalidOperationException("Trying to callback a void pending task with a byte array, this should be a generic task, please use Task<byte[]> instead.");
        }
    }
}
