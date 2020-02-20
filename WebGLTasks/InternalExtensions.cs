using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Firebase.WebGL.Threading
{
    internal static class InternalExtensions
    {
        internal static Task<TResult> OnSuccess<TIn, TResult>(this Task<TIn> task, Func<Task<TIn>, TResult> continuation)
        {
            return ((Task)task).OnSuccess<TResult>((Task t) => continuation((Task<TIn>)t));
        }

        internal static Task OnSuccess<TIn>(this Task<TIn> task, Action<Task<TIn>> continuation)
        {
            return task.OnSuccess<TIn, object>((Task<TIn> t) => {
                continuation(t);
                return null;
            });
        }

        internal static Task<TResult> OnSuccess<TResult>(this Task task, Func<Task, TResult> continuation)
        {
            return task.ContinueWith<Task<TResult>>((Task t) => {
                if (!t.IsFaulted)
                {
                    if (!t.IsCanceled)
                    {
                        return Task.FromResult<TResult>(continuation(t));
                    }
                    TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
                    taskCompletionSource.SetCanceled();
                    return taskCompletionSource.Task;
                }
                AggregateException aggregateException = t.Exception.Flatten();
                if (aggregateException.InnerExceptions.Count != 1)
                {
                    ExceptionDispatchInfo.Capture(aggregateException).Throw();
                }
                else
                {
                    ExceptionDispatchInfo.Capture(aggregateException.InnerExceptions[0]).Throw();
                }
                return Task.FromResult<TResult>(default(TResult));
            }).Unwrap<TResult>();
        }

        internal static Task OnSuccess(this Task task, Action<Task> continuation)
        {
            return task.OnSuccess<object>((Task t) => {
                continuation(t);
                return null;
            });
        }

        internal static Task WhileAsync(Func<Task<bool>> predicate, Func<Task> body)
        {
            Func<Task> func = null;
            func = () => predicate().OnSuccess<bool, Task>((Task<bool> t) => {
                if (!t.Result)
                {
                    return Task.FromResult<int>(0);
                }
                return body().OnSuccess<Task>((Task _) => _.ContinueWith(null)).Unwrap();
            }).Unwrap();
            return func();
        }
    }
}
