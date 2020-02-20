using System;

namespace Firebase.WebGL.Threading
{
    public sealed class Task<T> : Task
    {
        private T result;

        public T Result
        {
            get
            {
                base.Wait();
                return this.result;
            }
        }

        internal Task()
        {
        }

        public Task ContinueWith(Action<Task<T>> continuation)
        {
            return base.ContinueWith((Task t) => continuation((Task<T>)t));
        }

        public Task<TResult> ContinueWith<TResult>(Func<Task<T>, TResult> continuation)
        {
            return base.ContinueWith<TResult>((Task t) => continuation((Task<T>)t));
        }

        private void RunContinuations()
        {
            foreach (Action<Task> continuation in this.continuations)
            {
                continuation(this);
            }
            this.continuations = null;
        }

        internal bool TrySetCanceled()
        {
            bool flag;

            if (!this.IsCompleted)
            {
                this.IsCompleted = true;
                this.IsCanceled = true;
                this.RunContinuations();
                flag = true;
            }
            else
            {
                flag = false;
            }

            return flag;
        }

        internal bool TrySetException(AggregateException exception)
        {
            bool flag;
            if (!this.IsCompleted)
            {
                this.IsCompleted = true;
                this.exception = exception;
                this.RunContinuations();
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        internal bool TrySetResult(T result)
        {
            bool flag;
            if (!this.IsCompleted)
            {
                this.IsCompleted = true;
                this.result = result;
                this.RunContinuations();
                flag = true;
            }
            else
            {
                flag = false;
            }

            return flag;
        }
    }
}
