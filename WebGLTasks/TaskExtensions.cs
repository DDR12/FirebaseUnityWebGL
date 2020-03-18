using System;
using System.IO;

namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
        static int taskID = 0;
        public static int GetTaskID()
        {
            int id = taskID;
            taskID++;
            return id;
        }
        public static Task CopyToAsync(this Stream stream, Stream destination)
        {
            return stream.CopyToAsync(destination, 2048, CancellationToken.None);
        }

        public static Task CopyToAsync(this Stream stream, Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            byte[] numArray = new byte[bufferSize];
            int result = 0;
            return InternalExtensions.WhileAsync(() => stream.ReadAsync(numArray, 0, bufferSize, cancellationToken).OnSuccess<int, bool>((Task<int> readTask) => {
                result = readTask.Result;
                return result > 0;
            }), () => {
                cancellationToken.ThrowIfCancellationRequested();
                return destination.WriteAsync(numArray, 0, result, cancellationToken).OnSuccess((Task _) => cancellationToken.ThrowIfCancellationRequested());
            });
        }

        public static Task<int> ReadAsync(this Stream stream, byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
                taskCompletionSource.SetCanceled();
                return taskCompletionSource.Task;
            }
            TaskFactory factory = Task.Factory;
            Stream stream1 = stream;
            Stream stream2 = stream;
            return factory.FromAsync(new Func<byte[], int, int, AsyncCallback, object, IAsyncResult>(stream1.BeginRead), new Func<IAsyncResult, int>(stream2.EndRead), buffer, offset, count, null);
        }

        public static Task<string> ReadToEndAsync(this StreamReader reader)
        {
            return Task.Run<string>(() => reader.ReadToEnd());
        }

        public static Task Unwrap(this Task<Task> task)
        {
            TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
            task.ContinueWith((Task<Task> t) => {
                if (t.IsFaulted)
                {
                    taskCompletionSource.TrySetException(t.Exception);
                }
                else if (!t.IsCanceled)
                {
                    task.Result.ContinueWith((Task inner) => {
                        if (inner.IsFaulted)
                        {
                            taskCompletionSource.TrySetException(inner.Exception);
                        }
                        else if (!inner.IsCanceled)
                        {
                            taskCompletionSource.TrySetResult(0);
                        }
                        else
                        {
                            taskCompletionSource.TrySetCanceled();
                        }
                    });
                }
                else
                {
                    taskCompletionSource.TrySetCanceled();
                }
            });
            return taskCompletionSource.Task;
        }

        public static Task<T> Unwrap<T>(this Task<Task<T>> task)
        {
            TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
            task.ContinueWith((Task<Task<T>> t) => {
                if (t.IsFaulted)
                {
                    taskCompletionSource.TrySetException(t.Exception);
                }
                else if (!t.IsCanceled)
                {
                    t.Result.ContinueWith((Task<T> inner) => {
                        if (inner.IsFaulted)
                        {
                            taskCompletionSource.TrySetException(inner.Exception);
                        }
                        else if (!inner.IsCanceled)
                        {
                            taskCompletionSource.TrySetResult(inner.Result);
                        }
                        else
                        {
                            taskCompletionSource.TrySetCanceled();
                        }
                    });
                }
                else
                {
                    taskCompletionSource.TrySetCanceled();
                }
            });
            return taskCompletionSource.Task;
        }

        public static Task WriteAsync(this Stream stream, byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
                taskCompletionSource.SetCanceled();
                return taskCompletionSource.Task;
            }
            TaskFactory factory = Task.Factory;
            Stream stream1 = stream;
            Stream stream2 = stream;
            return factory.FromAsync<byte[], int, int>(new Func<byte[], int, int, AsyncCallback, object, IAsyncResult>(stream1.BeginWrite), new Action<IAsyncResult>(stream2.EndWrite), buffer, offset, count, null);
        }
    }
}
