using System;
using System.Collections.Generic;

namespace Firebase.Functions
{
    /// <summary>
    /// <see cref="FirebaseFunctions"/> is a service that supports calling Google Cloud Functions.
    /// </summary>
    public sealed class FirebaseFunctions : IDisposable
    {

        private readonly static IDictionary<string, FirebaseFunctions> functionsByInstanceKey;

        private readonly string instanceKey;
        /// <summary>
        /// This proprty is not used in WebGL and is irrelevant, javascript library doesn't require a region parameter for initializing a functions instance, I'm guessing they handle the region internally.
        /// </summary>
        public string Region { get; }
        /// <summary>
        /// The <see cref="FirebaseApp"/> associated with this <see cref="FirebaseFunctions"/> instance.
        /// </summary>
        public FirebaseApp App { get; }

        static FirebaseFunctions defaultInstance = null;
        /// <summary>
        /// Returns the <see cref="FirebaseFunctions"/> , initialized with the default <see cref="FirebaseApp"/>
        /// </summary>
        public static FirebaseFunctions DefaultInstance
        {
            get
            {
                if (defaultInstance == null)
                    defaultInstance = GetInstance(FirebaseApp.DefaultInstance);
                return defaultInstance;
            }
        }
       
        static FirebaseFunctions()
        {
            functionsByInstanceKey = new Dictionary<string, FirebaseFunctions>();
        }
        private FirebaseFunctions(FirebaseApp app, string region)
        {
            App = app;
            Region = region;
            instanceKey = InstanceKey(app, region);
        }
        ~FirebaseFunctions()
        {
            Dispose();
        }

        /// <summary>
        /// Returns the <see cref="FirebaseFunctions"/> , initialized with a custom <see cref="FirebaseApp"/>
        /// </summary>
        /// <param name="app">The custom <see cref="FirebaseApp"/> used for initialization.</param>
        /// <returns></returns>
        public static FirebaseFunctions GetInstance(FirebaseApp app)
        {
            return GetInstance(app, "us-central1");
        }
        /// <summary>
        /// Returns the <see cref="FirebaseFunctions"/> , initialized with the default <see cref="FirebaseApp"/>
        /// </summary>
        /// <param name="region">The region to call Cloud Functions in., In WebGL this is handled internally, so you can pass null safely.</param>
        /// <returns></returns>
        public static FirebaseFunctions GetInstance(string region)
        {
            return GetInstance(FirebaseApp.DefaultInstance, region);
        }
        /// <summary>
        /// Returns the <see cref="FirebaseFunctions"/> , initialized with a custom <see cref="FirebaseApp"/> and region.
        /// </summary>
        /// <param name="app">The custom <see cref="FirebaseApp"/> used for initialization.</param>
        /// <param name="region">The region to call Cloud Functions in., In WebGL this is handled internally, so you can pass null safely.</param>
        /// <returns></returns>
        public static FirebaseFunctions GetInstance(FirebaseApp app, string region)
        {
            string instanceKey = InstanceKey(app, region);
            if(functionsByInstanceKey.TryGetValue(instanceKey, out FirebaseFunctions result))
            {
                if(result == null)
                {
                    app = app ?? FirebaseApp.DefaultInstance;
                    result = new FirebaseFunctions(app, region);
                }
            }
            return result;
        }

        private static string InstanceKey(FirebaseApp app, string region)
        {
            return $"{app.Name}/{region}";
        }
        /// <summary>
        /// Creates a <see cref="HttpsCallableReference"/> that refers to the function with the given name.
        /// </summary>
        /// <param name="name">The name of the https callable function.</param>
        /// <returns>The <see cref="HttpsCallableReference"/> instance.</returns>
        public HttpsCallableReference GetHttpsCallable(string name)
        {
            return GetHttpsCallable(name, null);
        }
        /// <summary>
        /// Creates a <see cref="HttpsCallableReference"/> that refers to the function with the given name.
        /// </summary>
        /// <param name="name">The name of the https callable function.</param>
        /// <param name="options">The options for this <see cref="HttpsCallableReference"/> instance.</param>
        /// <returns>The <see cref="HttpsCallableReference"/> instance.</returns>
        public HttpsCallableReference GetHttpsCallable(string name, HttpsCallableOptions options = null)
        {
            return HttpsCallableReference.Create(this, name, options);
        }
        /// <summary>
        /// Sets an origin of a Cloud Functions Emulator instance to use, see https://firebase.google.com/docs/functions/local-emulator
        /// </summary>
        /// <param name="origin">URL of the local emulator.</param>
        public void UseFunctionsEmulator(string origin)
        {
            FunctionsPInvoke.UseFunctionsEmulator_WebGL(App.Name, origin);
        }

        public void Dispose()
        {
            functionsByInstanceKey.Remove(instanceKey);
        }
    }
}
