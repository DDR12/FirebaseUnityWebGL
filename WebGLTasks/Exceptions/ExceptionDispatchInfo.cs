using System;
using System.Runtime.CompilerServices;

namespace System.Runtime.ExceptionServices
{
    public class ExceptionDispatchInfo
    {
        public Exception SourceException
        {
            get;
            private set;
        }

        private ExceptionDispatchInfo(Exception ex)
        {
            this.SourceException = ex;
        }

        public static ExceptionDispatchInfo Capture(Exception ex)
        {
            return new ExceptionDispatchInfo(ex);
        }

        public void Throw()
        {
            throw this.SourceException;
        }
    }
}