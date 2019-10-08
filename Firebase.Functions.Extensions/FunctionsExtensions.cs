using System;

namespace Firebase.Functions
{
    /// <summary>
    /// This class helps match different platform sdks, when an sdk is missing a method but the other provides it, this class is a meeting point which both implementations can be accessed using same code instead of the #if #else directive nightmare.
    /// </summary>
    public static class FunctionsExtensions
    {
        /// <summary>
        /// Creates a <see cref="HttpsCallableReference"/> given a name, but also provides an option to use a timeout option, timeout is only available in the webgl sdk, so it will use the default GetHttpCallable under the hood in all builds except in WebGL.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static HttpsCallableReference GetHttpsCallable(this FirebaseFunctions firebaseFunctions, string name, TimeSpan timeout)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseFunctions.GetHttpsCallable(name, new HttpsCallableOptions() { Timeout = (uint)timeout.TotalSeconds });
#else
            return firebaseFunctions.GetHttpsCallable(name);
#endif
        }
    }
}
