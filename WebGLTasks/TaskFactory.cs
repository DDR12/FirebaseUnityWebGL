using System;

namespace System.Threading.Tasks
{
    public class TaskFactory
    {
        private readonly CancellationToken cancellationToken;

        public TaskScheduler Scheduler { get; }

        internal TaskFactory(TaskScheduler scheduler, CancellationToken cancellationToken)
        {
            this.Scheduler = scheduler;
            this.cancellationToken = cancellationToken;
        }

        public TaskFactory(TaskScheduler scheduler) : this(scheduler, CancellationToken.None)
        {
        }

        public TaskFactory(CancellationToken cancellationToken) : this(new TaskScheduler(null), cancellationToken)
        {
        }

        public TaskFactory() : this(new TaskScheduler(null), CancellationToken.None)
        {
        }

        public TaskFactory(CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) : this(scheduler, cancellationToken)
        {
        }

        public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction)
        {
            int length = (int)tasks.Length;
            TaskCompletionSource<Task[]> taskCompletionSource = new TaskCompletionSource<Task[]>();
            if (length == 0)
            {
                taskCompletionSource.TrySetResult(tasks);
            }
            Task[] taskArray = tasks;
            for (int i = 0; i < (int)taskArray.Length; i++)
            {
                Task task = taskArray[i];
                task.ContinueWith((Task _) => 
                {
                    length--;
                    if (length == 0)
                    {
                        taskCompletionSource.TrySetResult(tasks);
                    }
                });
            }
            return taskCompletionSource.Task.ContinueWith((Task<Task[]> t) => continuationAction(t.Result));
        }

        public Task FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
        {
            return this.FromAsync((AsyncCallback callback, object _) => beginMethod(arg1, arg2, arg3, callback, state), endMethod, state);
        }

        public Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
        {
            return this.FromAsync<TResult>((AsyncCallback callback, object _) => beginMethod(arg1, arg2, arg3, callback, state), endMethod, state);
        }

        public Task FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
        {
            return this.FromAsync((AsyncCallback callback, object _) => beginMethod(arg1, arg2, callback, state), endMethod, state);
        }

        public Task<TResult> FromAsync<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
        {
            return this.FromAsync<TResult>((AsyncCallback callback, object _) => beginMethod(arg1, arg2, callback, state), endMethod, state);
        }

        public Task FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, object state)
        {
            return this.FromAsync((AsyncCallback callback, object _) => beginMethod(arg1, callback, state), endMethod, state);
        }

        public Task<TResult> FromAsync<TArg1, TResult>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state)
        {
            return this.FromAsync<TResult>((AsyncCallback callback, object _) => beginMethod(arg1, callback, state), endMethod, state);
        }

        public Task FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, object state)
        {
            return this.FromAsync<int>(beginMethod, (IAsyncResult result) => {
                endMethod(result);
                return 0;
            }, state);
        }

        public Task<TResult> FromAsync<TResult>(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state)
        {
            TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
            CancellationToken cancellationToken = this.cancellationToken;
            CancellationTokenRegistration cancellationTokenRegistration = cancellationToken.Register(() => taskCompletionSource.TrySetCanceled());
            if (this.cancellationToken.IsCancellationRequested)
            {
                taskCompletionSource.TrySetCanceled();
                cancellationTokenRegistration.Dispose();
                return taskCompletionSource.Task;
            }
            try
            {
                beginMethod(new AsyncCallback((IAsyncResult result) => {
                    try
                    {
                        TResult tResult = endMethod(result);
                        taskCompletionSource.TrySetResult(tResult);
                        cancellationTokenRegistration.Dispose();
                    }
                    catch (Exception exception)
                    {
                        taskCompletionSource.TrySetException(exception);
                        cancellationTokenRegistration.Dispose();
                    }
                }), state);
            }
            catch (Exception exception1)
            {
                taskCompletionSource.TrySetException(exception1);
                cancellationTokenRegistration.Dispose();
            }
            return taskCompletionSource.Task;
        }

        public Task<T> StartNew<T>(Func<T> func)
        {
            TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
            this.Scheduler.Post(() => {
                try
                {
                    taskCompletionSource.SetResult(func());
                }
                catch (Exception exception)
                {
                    taskCompletionSource.SetException(exception);
                }
            });
            return taskCompletionSource.Task;
        }
    }
}
