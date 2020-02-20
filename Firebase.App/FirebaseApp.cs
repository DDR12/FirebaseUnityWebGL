using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Firebase.WebGL.Threading;
namespace Firebase
{
    /// <summary>
    /// Firebase application object.
    /// </summary>
    public sealed class FirebaseApp : IDisposable
    {
        static IDictionary<string, FirebaseApp> apps;

        static FirebaseApp defaultInstance = null;
        /// <summary>
        /// Get the default <see cref="FirebaseApp"/> instance.
        /// </summary>
        public static FirebaseApp DefaultInstance
        {
            get
            {
                if (defaultInstance == null)
                    defaultInstance = Create();
                return defaultInstance;
            }
        }

        /// <summary>
        /// Gets the default name for <see cref="FirebaseApp"/> objects.
        /// </summary>
        public static string DefaultName => "[DEFAULT]";

        static LogLevel m_LogLevel = LogLevel.Debug;
        /// <summary>
        /// Gets or sets the minimum log verbosity level for Firebase features.
        /// </summary>
        public static LogLevel LogLevel
        {
            get
            {
                PlatformHandler.NotifyWebGLFeatureDoesntHaveAMatch();
                return m_LogLevel;
            }
            set
            {
                PlatformHandler.NotifyWebGLFeatureDoesntHaveAMatch();
                m_LogLevel = value;
            }
        }

        /// <summary>
        /// Get the name of this App instance.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Get the <see cref="AppOptions"/> the <see cref="FirebaseApp"/> was created with.
        /// </summary>
        public AppOptions Options { get; }


        private FirebaseApp(string name, AppOptions options)
        {
            this.Name = name;
            Options = options;
            apps.Add(name, this);
        }
        /// <summary>
        /// 
        /// </summary>
        ~FirebaseApp()
        {
            Dispose();
        }
        static FirebaseApp()
        {
            apps = new Dictionary<string, FirebaseApp>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            apps.Remove(Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string UriToUrlString(Uri uri)
        {
            return (uri == null ? string.Empty : uri.OriginalString);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlString"></param>
        /// <returns></returns>
        public static Uri UrlStringToUri(string urlString)
        {
            Uri uri;
            if (string.IsNullOrWhiteSpace(urlString))
            {
                return null;
            }
            try
            {
                uri = new Uri(urlString);
            }
            catch (UriFormatException)
            {
                uri = null;
            }
            return uri;
        }

        /// <summary>
        /// Asynchronously checks if all of the necessary dependencies for Firebase are present on the system, and in the necessary state and attempts to fix them if they are not.
        /// </summary>
        /// <returns>A Task that on completion will contain the DependencyStatus enum value, indicating the state of the required dependencies.</returns>
        public static Task<DependencyStatus> CheckAndFixDependenciesAsync()
        {
            TaskCompletionSource<DependencyStatus> task = new TaskCompletionSource<DependencyStatus>();
            task.SetResult(DependencyStatus.Available);
            return task.Task;
        }
        /// <summary>
        /// Asynchronously checks if all of the necessary dependencies for Firebase are present on the system, and in the necessary state.
        /// </summary>
        /// <returns>A Task that on completion will contain the DependencyStatus enum value, indicating the state of the required dependencies.</returns>
        public static Task<DependencyStatus> CheckDependenciesAsync()
        {
            // Since both methods do nothing in webgl build, let's just return the same result.
            return CheckAndFixDependenciesAsync();
        }
        /// <summary>
        /// Initializes the default <see cref="FirebaseApp"/> with default options.
        /// </summary>
        /// <returns>New <see cref="FirebaseApp"/> instance.</returns>
        public static FirebaseApp Create()
        {
            return Create(AppOptions.LoadDefaultOptions(), DefaultName);
        }

        /// <summary>
        /// Initializes the default <see cref="FirebaseApp"/> with the given options.
        /// </summary>
        /// <param name="options">Options that control the creation of the <see cref="FirebaseApp"/>.</param>
        /// <returns>New <see cref="FirebaseApp"/> instance.</returns>
        public static FirebaseApp Create(AppOptions options)
        {
            return Create(options, DefaultName);
        }
        /// <summary>
        /// Initializes a <see cref="FirebaseApp"/> with the given options that operate on the named app.
        /// </summary>
        /// <param name="options">Options that control the creation of the <see cref="FirebaseApp"/>.</param>
        /// <param name="name">Name of this <see cref="FirebaseApp"/> instance. This is only required when one application uses multiple <see cref="FirebaseApp"/> instances.</param>
        /// <returns>New <see cref="FirebaseApp"/> instance.</returns>
        public static FirebaseApp Create(AppOptions options, string name)
        {
            string optionsJson = JsonConvert.SerializeObject(options);
            bool created = AppPInvoke.InitializeFirebaseApp_WebGL(name, optionsJson);
            if (created)
            {
                return new FirebaseApp(name, options);
            }
            return null;
        }

        /// <summary>
        /// Attempts to fix any missing dependencies that would prevent Firebase from working on the system.
        /// </summary>
        /// <returns><see cref="Task"/> A task that tracks the progress of the fix.</returns>
        public static Task FixDependenciesAsync()
        {
            TaskCompletionSource<object> task = new TaskCompletionSource<object>();
            task.SetResult(null);
            return task.Task;
        }
        /// <summary>
        /// Get an instance of an app by name.
        /// </summary>
        /// <param name="name">Name of the app to retrieve.</param>
        /// <returns>Reference to the app if it was previously created, null otherwise.</returns>
        public static FirebaseApp GetInstance(string name)
        {
            if (apps.TryGetValue(name, out FirebaseApp app))
            {
                return app;
            }
            return null;
        }
    }
}
