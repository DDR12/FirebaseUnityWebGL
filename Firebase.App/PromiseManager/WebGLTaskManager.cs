using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Firebase
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebGLTaskManager
    {
        static Dictionary<int, IWebGLTask> tasks;

        static WebGLTaskManager()
        {
            tasks = new Dictionary<int, IWebGLTask>();
        }
        /// <summary>
        /// Creates a generic task and adds it to pending tasks.
        /// </summary>
        /// <typeparam name="TResult">Type of the return type for the task.</typeparam>
        /// <returns>The created task.</returns>
        public static TaskCompletionSource<TResult> GetTask<TResult>()
        {
            TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
            WebGLTaskHandler<TResult> promiseHandler = new WebGLTaskHandler<TResult>(taskCompletionSource);
            tasks.Add(taskCompletionSource.Task.Id, promiseHandler);
            return taskCompletionSource;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TaskCompletionSource<object> GetTask()
        {
            TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
            WebGLTaskHandler promiseHandler = new WebGLTaskHandler(taskCompletionSource);
            tasks.Add(taskCompletionSource.Task.Id, promiseHandler);
            return taskCompletionSource;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="json"></param>
        /// <param name="error"></param>
        [AOT.MonoPInvokeCallback(typeof(GenericTaskWebGLDelegate))]
        public static void DequeueTask(int taskID, string json, string error)
        {
            PlatformHandler.CaptureWebGLInput(true);
            if (tasks.TryGetValue(taskID, out IWebGLTask promise))
            {
                promise.SetResult(json, error);
                tasks.Remove(taskID);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="error"></param>
        [AOT.MonoPInvokeCallback(typeof(VoidTaskWebGLDelegate))]
        public static void DequeueTask(int taskID, string error)
        {
            PlatformHandler.CaptureWebGLInput(true);
            if (tasks.TryGetValue(taskID, out IWebGLTask promise))
            {
                promise.SetResult(null, error);
                tasks.Remove(taskID);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="pBuffer"></param>
        /// <param name="bufferLength"></param>
        /// <param name="errorJson"></param>
        [AOT.MonoPInvokeCallback(typeof(ByteArrayTaskWebGLDelegate))]
        public static void DequeueTask(int taskID, IntPtr pBuffer, int bufferLength, string errorJson)
        {
            PlatformHandler.CaptureWebGLInput(true);
            if (tasks.TryGetValue(taskID, out IWebGLTask promise))
            {
                promise.SetResult(pBuffer, bufferLength, errorJson);
                tasks.Remove(taskID);
            }
        }
    }
}