using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firebase.Extensions
{
    /// <summary>
    /// Helps dispatch task results to the main thread to be able to operate on unity's API like SetActive, enabled etc...
    /// </summary>
    internal class MainThreadHandler : MonoBehaviour
    {
        Queue<Action> jobs = new Queue<Action>();
        static MainThreadHandler instance = null;
        public static MainThreadHandler Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameObject("MainThreadHandler").AddComponent<MainThreadHandler>();
                return instance;
            }
        }
        private void Awake()
        {
            instance = this;
        }
        private void Destroy()
        {
            if (instance == this)
                instance = null;
        }
        private void Update()
        {
            while (jobs.Count > 0)
                jobs.Dequeue()?.Invoke();
        }

        /// <summary>
        /// Dispatches a function to be executed on unity's main thread to be able to use unity's API.
        /// </summary>
        /// <param name="callback">The callback to dispatch to main thread.</param>
        public static Task<TResult> Dispatch<TResult>(Func<TResult> callback)
        {
            TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
            instance.jobs.Enqueue(new Action(() =>
            {
                try
                {
                    taskCompletionSource.SetResult(callback());
                }
                catch(Exception exception)
                {
                    taskCompletionSource.SetException(exception);
                }
            }));
            return taskCompletionSource.Task;
        }
        private class CallbackStorage<TResult>
        {
            public Exception Exception
            {
                get;
                set;
            }

            public TResult Result
            {
                get;
                set;
            }

            public CallbackStorage()
            {
            }
        }
    }
}