using System;

namespace Firebase.Performance
{
    /// <summary>
    /// The Firebase Performance Monitoring service interface.
    /// </summary>
    public sealed class Performance : IDisposable
    {
        static Performance defaultInstance;
        /// <summary>
        /// Default Instance of the Performance Monitoring Service.
        /// </summary>
        public static Performance DefaultInstance
        {
            get
            {
                if (defaultInstance == null)
                {
                    defaultInstance = GetInstance(FirebaseApp.DefaultInstance);

                }
                return defaultInstance;
            }
        }
        /// <summary>
        /// The app this service instance monitors.
        /// </summary>
        public FirebaseApp App { get; }
        private Performance(FirebaseApp app)
        {
            App = app;
        }

        /// <summary>
        /// Controls the logging of custom traces.
        /// </summary>
        public bool DataCollectionEnabled
        {
            get => PerformancePInvoke.GetDataCollectionEnabled_WebGL(App.Name);
            set => PerformancePInvoke.SetDataCollectionEnabled_WebGL(App.Name, value);
        }

        /// <summary>
        /// Controls the logging of automatic traces and HTTP/S network monitoring.
        /// </summary>
        public bool InstrumentationEnabled
        {
            get => PerformancePInvoke.GetInstrumentationEnabled_WebGL(App.Name);
            set => PerformancePInvoke.SetInstrumentationEnabled_WebGL(App.Name, value);
        }

        /// <summary>
        /// Creates an uninitialized instance of <see cref="Firebase.Performance.Trace"/> and returns it.
        /// </summary>
        /// <param name="traceName">The name of the trace instance.</param>
        /// <returns>The Trace instance.</returns>
        public Trace Trace(string traceName)
        {
            return Firebase.Performance.Trace.Create(App.Name, traceName);
        }

        public void Dispose()
        {
            
        }

        /// <summary>
        /// Creates a performance monitor service instance for the passed app, if a null app is used, the default app will be used.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static Performance GetInstance(FirebaseApp app)
        {
            if(app == null)
            {
                if (defaultInstance == null)
                    defaultInstance = new Performance(FirebaseApp.DefaultInstance);
                return defaultInstance;
            }
            Performance performance = new Performance(app);
            return performance;
        }
    }
}
