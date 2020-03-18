using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
namespace Firebase
{
    /// <summary>
    /// Wrapper that encapsulates a generic task running on the javascript side, and resolves that task when response is back from the browser.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class WebGLTaskHandler<TResult> : IWebGLTask
    {
        public TaskCompletionSource<TResult> Promise { get; private set; }
        public int ID { get; private set; }
        public WebGLTaskHandler(TaskCompletionSource<TResult> task, int id)
        {
            this.ID = id;
            this.Promise = task;
        }
        public void SetResult(string json, string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                Promise.SetResult(JsonConvert.DeserializeObject<TResult>(json));
            }
            else
            {
                FirebaseError firebaseError = FirebaseError.FromJson(error);
                FirebaseException exception = new FirebaseException(firebaseError.ErrorCode, firebaseError.Message);
                Promise.SetException(exception);
            }
        }

        public void SetResult(IntPtr pBuffer, int bufferLength, string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                byte[] buffer = new byte[bufferLength];
                // Copy data from the 'unmanaged' memory to managed memory. Buffer will be reclaimed by the GC.
                Marshal.Copy(pBuffer, buffer, 0, bufferLength);
                TaskCompletionSource<byte[]> convertedTask = Promise as TaskCompletionSource<byte[]>;
                convertedTask.SetResult(buffer);
            }
            else
            {
                SetResult(null, error);
            }
        }
    }
    
}