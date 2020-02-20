using System;
using System.Collections.Generic;

namespace Firebase.WebGL.Threading
{
    public sealed class CancellationTokenSource
    {
        private Action actions;

        internal bool IsCancellationRequested { get; private set; }

        public CancellationToken Token
        {
            get
            {
                return new CancellationToken(this);
            }
        }

        public CancellationTokenSource()
        {
        }

        public void Cancel()
        {
            this.Cancel(false);
        }

        public void Cancel(bool throwOnFirstException)
        {
            this.IsCancellationRequested = true;
            if (this.actions != null)
            {
                try
                {
                    if (!throwOnFirstException)
                    {
                        Delegate[] invocationList = this.actions.GetInvocationList();
                        for (int i = 0; i < (int)invocationList.Length; i++)
                        {
                            Delegate @delegate = invocationList[i];
                            List<Exception> exceptions = new List<Exception>();
                            try
                            {
                                ((Action)@delegate)();
                            }
                            catch (Exception exception)
                            {
                                exceptions.Add(exception);
                            }
                            if (exceptions.Count > 0)
                            {
                                throw new AggregateException(exceptions);
                            }
                        }
                    }
                    else
                    {
                        this.actions();
                    }
                }
                finally
                {
                    this.actions = null;
                }
            }
        }

        internal CancellationTokenRegistration Register(Action action)
        {
            CancellationTokenRegistration cancellationTokenRegistration;
            this.actions += action;
            if (this.IsCancellationRequested)
            {
                this.Cancel(false);
            }
            cancellationTokenRegistration = new CancellationTokenRegistration(this, action);
            return cancellationTokenRegistration;
        }

        internal void Unregister(Action action)
        {
            this.actions -= action;
        }
    }
}
