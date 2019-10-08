using System.Runtime.InteropServices;

namespace Firebase.Functions
{
    internal class FunctionsPInvoke
    {
        [DllImport("__Internal")]
        public static extern void UseFunctionsEmulator_WebGL(string appName, string origin);


        [DllImport("__Internal")]
        public static extern void CreateCallableReference_WebGL(uint refID, string appName, string functionName, string optionsJson);


        [DllImport("__Internal")]
        public static extern void ReleaseHttpCallableReference_WebGL(uint refID);


        [DllImport("__Internal")]
        public static extern void CallFunction_WebGL(uint refID, int promiseID, string data, GenericTaskWebGLDelegate callback);

    }
}
