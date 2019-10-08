using System.Runtime.InteropServices;

namespace Firebase
{
    internal class AppPInvoke
    {
        [DllImport("__Internal")]
        public static extern string GetDefaultAppName_WebGL();

        [DllImport("__Internal")]
        public static extern bool InitializeFirebaseApp_WebGL(string appName, string optionsJson);
    }
}
