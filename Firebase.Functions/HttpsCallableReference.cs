using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Firebase.Functions
{
    /// <summary>
    /// Represents a reference to a Google Cloud Functions HTTPS callable function.
    /// </summary>
    public sealed class HttpsCallableReference : IDisposable
    {
        static uint ReferencesID = 0;

        /// <summary>
        /// Returns the <see cref="FirebaseFunctions"/> service which created this reference.
        /// </summary>
        public FirebaseFunctions Functions { get; }
        /// <summary>
        /// Name of the cloud function this reference points to.
        /// </summary>
        public string FunctionName { get; }

        private readonly uint id = 0;

        private HttpsCallableReference(FirebaseFunctions functions, string name, HttpsCallableOptions options)
        {
            Functions = functions;
            FunctionName = name;
            id = ReferencesID++;
            FunctionsPInvoke.CreateCallableReference_WebGL(id, Functions.App.Name, name, options == null ? null : JsonConvert.SerializeObject(options));
        }

        /// <summary>
        /// Calls the cloud function that this reference points to.
        /// </summary>
        /// <returns>A task that resolves with the call <see cref="HttpsCallableResult"/> or an error.</returns>
        public Task<HttpsCallableResult> CallAsync()
        {
            return CallAsync(null);
        }
        /// <summary>
        /// Calls the cloud function that this reference points to.
        /// </summary>
        /// <param name="data">The data to pass to the function.</param>
        /// <returns>A task that resolves with the call <see cref="HttpsCallableResult"/> or an error.</returns>
        public Task<HttpsCallableResult> CallAsync(object data)
        {
            string json = data == null ? null : JsonConvert.SerializeObject(data, FirebaseJsonSettings.Settings);
            var task = WebGLTaskManager.GetTask<HttpsCallableResult>();
            var finalTask = task.Task.ContinueWith(result =>
            {
                if(result.IsFaulted)
                {
                    AggregateException exception = result.Exception;
                    foreach (Exception innerException in exception.InnerExceptions)
                    {
                        if (innerException is FirebaseException firebaseException)
                        {
                            throw new FunctionsException(firebaseException);
                        }
                    }
                    throw exception;
                }
                return result.Result;
            });
            FunctionsPInvoke.CallFunction_WebGL(id, task.Task.Id, json, WebGLTaskManager.DequeueTask);
            return finalTask;
        }

        /// <summary>
        /// Creates a <see cref="HttpsCallableReference"/> that refers to the function with the given name.
        /// </summary>
        /// <param name="functions"></param>
        /// <param name="name">The name of the https callable function.</param>
        /// <param name="options">The options for this <see cref="HttpsCallableReference"/> instance.</param>
        /// <returns>The <see cref="HttpsCallableReference"/> instance.</returns>
        public static HttpsCallableReference Create(FirebaseFunctions functions, string name, HttpsCallableOptions options = null)
        {
            return new HttpsCallableReference(functions, name, options);
        }
        /// <summary>
        /// Dispose of this reference instance.
        /// </summary>
        public void Dispose()
        {
            FunctionsPInvoke.ReleaseHttpCallableReference_WebGL(id);
        }
    }
}
