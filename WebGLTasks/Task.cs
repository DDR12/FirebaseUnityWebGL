using System.Collections.Generic;
using System.Linq;
namespace System.Threading.Tasks
{
    public abstract class Task
    {
        private readonly static ThreadLocal<int> executionDepth;
        private readonly static Action<Action> immediateExecutor;

        internal IList<Action<Task>> continuations = new List<Action<Task>>();

        internal AggregateException exception;

        public AggregateException Exception => exception;
        public int Id { get; }
        public static TaskFactory Factory
        {
            get
            {
                return new TaskFactory();
            }
        }

        public bool IsCanceled { get; protected set; }

        public bool IsCompleted { get; protected set; }

        public bool IsFaulted
        {
            get
            {
                return this.Exception != null;
            }
        }

        static Task()
        {
            executionDepth = new ThreadLocal<int>(() => 0);
            immediateExecutor = (Action a) => {
                bool flag = AppDomain.CurrentDomain.FriendlyName.Equals("IL2CPP Root Domain");
                int num = 10;
                if (flag)
                {
                    num = 200;
                }
                ThreadLocal<int> value = executionDepth;
                value.Value = value.Value + 1;
                try
                {
                    if (executionDepth.Value > num)
                    {
                        Factory.Scheduler.Post(a);
                    }
                    else
                    {
                        a();
                    }
                }
                finally
                {
                    ThreadLocal<int> threadLocal = executionDepth;
                    threadLocal.Value -= 1;
                }
            };
        }

        internal Task()
        {
            Id = TaskExtensions.GetTaskID();
        }

        public Task<T> ContinueWith<T>(Func<Task, T> continuation)
        {
            return this.ContinueWith<T>(continuation, CancellationToken.None);
        }

        public Task<T> ContinueWith<T>(Func<Task, T> continuation, CancellationToken cancellationToken)
        {
            bool isCompleted = false;
            TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
            CancellationTokenRegistration cancellationTokenRegistration = cancellationToken.Register(() => taskCompletionSource.TrySetCanceled());
            Action<Task> action = (Task t) => Task.immediateExecutor(new Action(() =>
            {
                try
                {
                    taskCompletionSource.TrySetResult(continuation(t));
                    cancellationTokenRegistration.Dispose();
                }
                catch (Exception exception)
                {
                    taskCompletionSource.TrySetException(exception);
                    cancellationTokenRegistration.Dispose();
                }
            }));
            isCompleted = this.IsCompleted;
            if (!isCompleted)
            {
                this.continuations.Add(action);
            }

            if (isCompleted)
            {
                action(this);
            }
            return taskCompletionSource.Task;
        }

        public Task ContinueWith(Action<Task> continuation)
        {
            return this.ContinueWith(continuation, CancellationToken.None);
        }

        public Task ContinueWith(Action<Task> continuation, CancellationToken cancellationToken)
        {
            return this.ContinueWith<int>((Task t) => {
                continuation(t);
                return 0;
            }, cancellationToken);
        }

        public static Task Delay(TimeSpan timespan)
        {
            TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
            UnityWebGLTaskManager.Wait(timespan, () => taskCompletionSource.TrySetResult(0));
            return taskCompletionSource.Task;
        }

        public static Task<T> FromResult<T>(T result)
        {
            TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
            taskCompletionSource.SetResult(result);
            return taskCompletionSource.Task;
        }

        public static Task<T> Run<T>(Func<T> toRun)
        {
            return Task.Factory.StartNew<T>(toRun);
        }

        public static Task Run(Action toRun)
        {
            return Task.Factory.StartNew<int>(() => {
                toRun();
                return 0;
            });
        }

        public void Wait()
        {
            if (this.IsFaulted)
            {
                throw this.Exception;
            }
        }

        public static Task WhenAll(params Task[] tasks)
        {
            return Task.WhenAll((IEnumerable<Task>)tasks);
        }

        public static Task WhenAll(IEnumerable<Task> tasks)
        {
            Func<Task, bool> isFaulted = null;
            Func<Task, AggregateException> exception = null;
            Func<Task, bool> isCanceled = null;
            Task[] taskArray2 = tasks.ToArray<Task>();
            if ((int)taskArray2.Length == 0)
            {
                return Task.FromResult<int>(0);
            }
            TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
            Task.Factory.ContinueWhenAll(taskArray2, (Task[] _) => {
                Task[] taskArray = taskArray2;
                if (isFaulted == null)
                {
                    isFaulted = (Task t) => t.IsFaulted;
                }
                IEnumerable<Task> tasks1 = ((IEnumerable<Task>)taskArray).Where<Task>(isFaulted);
                if (exception == null)
                {
                    exception = (Task t) => t.Exception;
                }
                AggregateException[] array = tasks1.Select<Task, AggregateException>(exception).ToArray<AggregateException>();
                if ((int)array.Length <= 0)
                {
                    Task[] taskArray1 = taskArray2;
                    if (isCanceled == null)
                    {
                        isCanceled = (Task t) => t.IsCanceled;
                    }
                    if (!((IEnumerable<Task>)taskArray1).Any<Task>(isCanceled))
                    {
                        taskCompletionSource.SetResult(0);
                    }
                    else
                    {
                        taskCompletionSource.SetCanceled();
                    }
                }
                else
                {
                    taskCompletionSource.SetException(new AggregateException(array));
                }
            });
            return taskCompletionSource.Task;
        }

        public static Task<T[]> WhenAll<T>(IEnumerable<Task<T>> tasks)
        {
            Func<Task<T>, T> result = null;
            return Task.WhenAll(tasks.Cast<Task>()).OnSuccess<T[]>((Task _) => {
                IEnumerable<Task<T>> tasks1 = tasks;
                if (result == null)
                {
                    result = (Task<T> t) => t.Result;
                }
                return tasks1.Select<Task<T>, T>(result).ToArray<T>();
            });
        }

        internal static Task<Task> WhenAny(params Task[] tasks)
        {
            return Task.WhenAny((IEnumerable<Task>)tasks);
        }

        internal static Task<Task> WhenAny(IEnumerable<Task> tasks)
        {
            TaskCompletionSource<Task> taskCompletionSource = new TaskCompletionSource<Task>();
            foreach (Task task in tasks)
            {
                task.ContinueWith<bool>((Task t) => taskCompletionSource.TrySetResult(t));
            }
            return taskCompletionSource.Task;
        }

        public static Task<T> FromException<T>(Exception exception)
        {
            TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
            taskCompletionSource.SetException(exception);
            return taskCompletionSource.Task;

        }
        public static Task FromException(Exception exception) => FromException<bool>(exception);
    }
}
