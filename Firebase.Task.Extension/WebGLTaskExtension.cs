using System;
using System.Threading;
using System.Threading.Tasks;

namespace Firebase.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Task"/> and <see cref="Task{TResult}"/> that allow the continuation function to be executed on the main thread in Unity.
    /// </summary>
    public static class WebGLTaskExtension
    {
        /// <summary>
        /// Returns a failed task with the given error
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static Task FromException(Exception exception)
        {
            return FromException<bool>(exception);
        }
        /// <summary>
        /// Returns a failed task with the given error
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static Task<TResult> FromException<TResult>(Exception exception)
        {
            if(exception == null)
                throw new ArgumentNullException("exception");

            TaskCompletionSource<TResult> completionSource = new TaskCompletionSource<TResult>();
            completionSource.TrySetException(exception);
            return completionSource.Task;
        }
        /// <summary>
        /// Returns a Task which completes once the given task is complete and the given continuation function is called from the main thread in Unity.
        /// </summary>
        /// <param name="task">The task to continue with.</param>
        /// <param name="continuation">The continuation function to be executed on the main thread once the given task completes.</param>
        /// <returns>A new Task that is complete after the continuation is executed on the main thread.</returns>
        public static Task ContinueWithOnMainThread(this Task task, Action<Task> continuation)
        {
            return TaskExtensions.Unwrap<bool>(task.ContinueWith<Task<bool>>((Task t) => MainThreadHandler.Dispatch<bool>(() => {
                continuation(t);
                return true;
            })));
        }
        /// <summary>
        /// Returns a Task which completes once the given task is complete and the given continuation function is called from the main thread in Unity, with a cancellation token.
        /// </summary>
        /// <param name="task">The task to continue with.</param>
        /// <param name="continuation">The continuation function to be executed on the main thread once the given task completes.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A new Task that is complete after the continuation is executed on the main thread.</returns>
        public static Task ContinueWithOnMainThread(this Task task, Action<Task> continuation, CancellationToken cancellationToken)
        {
            return TaskExtensions.Unwrap<bool>(task.ContinueWith<Task<bool>>((Task t) => MainThreadHandler.Dispatch<bool>(() => {
                continuation(t);
                return true;
            }), cancellationToken));
        }

        /// <summary>
        /// Returns a Task which completes once the given task is complete and the given continuation function is called from the main thread in Unity.
        /// </summary>
        /// <typeparam name="T">The Task template type used as an input parameter for the continuation.</typeparam>
        /// <param name="task">The task to continue with.</param>
        /// <param name="continuation">The continuation function to be executed on the main thread once the given task completes.</param>
        /// <returns>A new Task that is complete after the continuation is executed on the main thread.</returns>
        public static Task ContinueWithOnMainThread<T>(this Task<T> task, Action<Task<T>> continuation)
        {
            return TaskExtensions.Unwrap<bool>(task.ContinueWith<Task<bool>>((Task<T> t) => MainThreadHandler.Dispatch<bool>(() => {
                continuation(t);
                return true;
            })));
        }

        /// <summary>
        /// Returns a <see cref="Task{TResult}"/> which completes once the given task is complete and the given continuation function with return value TResult is called from the main thread in Unity.
        /// </summary>
        /// <typeparam name="TResult">The type returned by the continuation.</typeparam>
        /// <param name="task">The task to continue with.</param>
        /// <param name="continuation">The continuation function to be executed on the main thread once the given task completes.</param>
        /// <returns>A new Task of type TResult, that is complete after the continuation is executed on the main thread.</returns>
        public static Task<TResult> ContinueWithOnMainThread<TResult>(this Task task, Func<Task, TResult> continuation)
        {
            return TaskExtensions.Unwrap<TResult>(task.ContinueWith<Task<TResult>>((Task t) => MainThreadHandler.Dispatch<TResult>(() => continuation(t))));
        }

        /// <summary>
        /// Returns a <see cref="Task{TResult}"/> which completes once the given task is complete and the given continuation function with return value TResult is called from the main thread in Unity, with a cancellation token.
        /// </summary>
        /// <typeparam name="TResult">The type returned by the continuation.</typeparam>
        /// <param name="task">The task to continue with.</param>
        /// <param name="continuation">The continuation function to be executed on the main thread once the given task completes.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A new Task of type TResult, that is complete after the continuation is executed on the main thread.</returns>
        public static Task<TResult> ContinueWithOnMainThread<TResult>(this Task task, Func<Task, TResult> continuation, CancellationToken cancellationToken)
        {
            return TaskExtensions.Unwrap<TResult>(task.ContinueWith<Task<TResult>>((Task t) => MainThreadHandler.Dispatch<TResult>(() => continuation(t)), cancellationToken));
        }

        /// <summary>
        /// Returns a <see cref="Task{TResult}"/> which completes once the given task is complete and the given continuation function with return value TResult is called from the main thread in Unity.
        /// </summary>
        /// <typeparam name="TResult">The return type from continuation and the Task template return type of this extension method.continuation.</typeparam>
        /// <typeparam name="T">The Task template type used as an input parameter for the continuation.</typeparam>
        /// <param name="task">The task to continue with.</param>
        /// <param name="continuation">The continuation function to be executed on the main thread once the given task completes.</param>
        /// <returns>A new Task of type TResult, that is complete after the continuation is executed on the main thread.</returns>
        public static Task<TResult> ContinueWithOnMainThread<TResult, T>(this Task<T> task, Func<Task<T>, TResult> continuation)
        {
            return TaskExtensions.Unwrap<TResult>(task.ContinueWith<Task<TResult>>((Task<T> t) => MainThreadHandler.Dispatch<TResult>(() => continuation(t))));
        }
    }
}
