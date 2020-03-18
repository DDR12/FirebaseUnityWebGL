
using System;
using System.Collections.Generic;

namespace System.Threading.Tasks
{
    public class ThreadLocal<T> : IDisposable
    {
        private static long lastId;
        private static IDictionary<long, T> threadLocalData;

        private static IList<WeakReference> allDataDictionaries;

        private bool disposed;

        private readonly long id;

        private readonly Func<T> valueFactory;

        private static IDictionary<long, T> ThreadLocalData
        {
            get
            {
                if (threadLocalData == null)
                {
                    lock (new Dictionary<long, T>())
                    {
                        allDataDictionaries.Add(new WeakReference(threadLocalData));
                    }
                }
                return threadLocalData;
            }
        }

        public T Value
        {
            get
            {
                T t;
                this.CheckDisposed();
                if (ThreadLocalData.TryGetValue(this.id, out t))
                {
                    return t;
                }
                T t1 = this.valueFactory();
                ThreadLocalData[this.id] = t1;
                return t1;
            }
            set
            {
                this.CheckDisposed();
                ThreadLocalData[this.id] = value;
            }
        }

        static ThreadLocal()
        {
            lastId = (long)-1;
            allDataDictionaries = new List<WeakReference>();
        }

        public ThreadLocal() : this(() => default(T))
        {
        }

        public ThreadLocal(Func<T> valueFactory)
        {
            this.valueFactory = valueFactory;
            this.id = lastId++;
        }

        private void CheckDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("ThreadLocal has been disposed.");
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < allDataDictionaries.Count; i++)
            {
                IDictionary<object, T> target = allDataDictionaries[i].Target as IDictionary<object, T>;
                if (target != null)
                {
                    target.Remove(this.id);
                }
                else
                {
                    allDataDictionaries.RemoveAt(i);
                    i--;
                }
            }
            this.disposed = true;
        }

        ~ThreadLocal()
        {
            if (!this.disposed)
            {
                this.Dispose();
            }
        }
    }
}
