using System;
using System.Threading.Tasks;

namespace Firebase
{
    /// <summary>
    /// Wrapper that encapsulates a void task running on the javascript side, and resolves that task when response is back from the browser.
    /// </summary>
    public class WebGLTaskHandler : IWebGLTask
    {
        public int ID { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public TaskCompletionSource<object> Promise { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="id"></param>
        public WebGLTaskHandler(TaskCompletionSource<object> task, int id)
        {
            this.ID = id;
            this.Promise = task;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="error"></param>
        public void SetResult(string json, string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                Promise.SetResult(null);
            }
            else
            {
                FirebaseError firebaseError = FirebaseError.FromJson(error);
                FirebaseException exception = new FirebaseException(firebaseError.ErrorCode, firebaseError.Message);
                Promise.SetException(exception);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pBuffer"></param>
        /// <param name="bufferLength"></param>
        /// <param name="error"></param>
        public void SetResult(IntPtr pBuffer, int bufferLength, string error)
        {
            throw new InvalidOperationException("Trying to callback a void pending task with a byte array, this should be a generic task, please use Task<byte[]> instead.");
        }
    }
}
