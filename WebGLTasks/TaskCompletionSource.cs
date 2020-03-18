using System;

namespace System.Threading.Tasks
{
    public class TaskCompletionSource<T>
    {
        public Task<T> Task
        {
            get;
            private set;
        }

        public TaskCompletionSource()
        {
            this.Task = new Task<T>();
        }

        public void SetCanceled()
        {
            if (!this.TrySetCanceled())
            {
                throw new InvalidOperationException("Cannot cancel a completed task.");
            }
        }

        public void SetException(AggregateException exception)
        {
            if (!this.TrySetException(exception))
            {
                throw new InvalidOperationException("Cannot set the exception of a completed task.");
            }
        }

        public void SetException(Exception exception)
        {
            if (!this.TrySetException(exception))
            {
                throw new InvalidOperationException("Cannot set the exception of a completed task.");
            }
        }

        public void SetResult(T result)
        {
            if (!this.TrySetResult(result))
            {
                throw new InvalidOperationException("Cannot set the result of a completed task.");
            }
        }

        public bool TrySetCanceled()
        {
            return this.Task.TrySetCanceled();
        }

        public bool TrySetException(AggregateException exception)
        {
            return this.Task.TrySetException(exception);
        }

        public bool TrySetException(Exception exception)
        {
            AggregateException aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                return this.Task.TrySetException(aggregateException);
            }
            return this.Task.TrySetException((new AggregateException(new Exception[] { exception })).Flatten());
        }

        public bool TrySetResult(T result)
        {
            return this.Task.TrySetResult(result);
        }
    }
}
