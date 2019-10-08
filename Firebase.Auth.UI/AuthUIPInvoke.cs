using System.Runtime.InteropServices;

namespace Firebase.Auth.UI
{
    internal class AuthUIPInvoke
    {
        [DllImport("__Internal")]
        public static extern bool GetAuthUIIsPendingRedirect_WebGL(uint uiID);
        [DllImport("__Internal")]
        public static extern void CreateNewAuthUI_WebGL(uint id, string appName, WebGLAuthUIVisibilityCallback visibilityCallback);
        [DllImport("__Internal")]
        public static extern void DisableAuthUIAutoSignIn_WebGL(uint uiID);
        [DllImport("__Internal")]
        public static extern void StartAuthUI_WebGL(uint uiID, int taskID, string configJson, GenericTaskWebGLDelegate signInCallback);
        [DllImport("__Internal")]
        public static extern void AuthUISignIn_WebGL(uint uiID);
        [DllImport("__Internal")]
        public static extern void ResetAuthUI_WebGL(uint uiID);
        [DllImport("__Internal")]
        public static extern void DeleteAuthUI_WebGL(int taskID, uint uiID, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void ReleaseAuthUI_WebGL(uint id);

        [DllImport("__Internal")]
        public static extern string GetAnonymousAuthProviderID_WebGL();

        [DllImport("__Internal")]
        public static extern string GetCredentialHelperString_WebGL(int typeIndex);

    }
}
