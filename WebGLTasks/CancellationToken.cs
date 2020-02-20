using System;

namespace Firebase.WebGL.Threading
{
    public struct CancellationToken
    {
        private CancellationTokenSource source;

        public bool IsCancellationRequested
        {
            get
            {
                return (this.source == null ? false : this.source.IsCancellationRequested);
            }
        }

        public static CancellationToken None
        {
            get
            {
                return new CancellationToken();
            }
        }

        internal CancellationToken(CancellationTokenSource source)
        {
            this.source = source;
        }

        public CancellationTokenRegistration Register(Action callback)
        {
            if (this.source == null)
            {
                return new CancellationTokenRegistration();
            }
            return this.source.Register(callback);
        }

        public void ThrowIfCancellationRequested()
        {
            if (this.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }
        }
    }
}
