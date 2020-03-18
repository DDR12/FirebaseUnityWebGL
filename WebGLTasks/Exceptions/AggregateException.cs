using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
    public class AggregateException : Exception
    {
        public ReadOnlyCollection<Exception> InnerExceptions
        {
            get;
            private set;
        }

        public AggregateException(IEnumerable<Exception> innerExceptions)
        {
            this.InnerExceptions = new ReadOnlyCollection<Exception>(innerExceptions.ToList<Exception>());
        }

        public AggregateException Flatten()
        {
            List<Exception> exceptions = new List<Exception>();
            foreach (Exception innerException in this.InnerExceptions)
            {
                AggregateException aggregateException = innerException as AggregateException;
                if (aggregateException == null)
                {
                    exceptions.Add(innerException);
                }
                else
                {
                    exceptions.AddRange(aggregateException.Flatten().InnerExceptions);
                }
            }
            return new AggregateException(exceptions);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(base.ToString());
            foreach (Exception innerException in this.InnerExceptions)
            {
                stringBuilder.AppendLine("\n-----------------");
                stringBuilder.AppendLine(innerException.ToString());
            }
            return stringBuilder.ToString();
        }
    }
}