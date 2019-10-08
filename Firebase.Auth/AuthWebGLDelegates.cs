namespace Firebase.Auth
{
    internal delegate void WebGLAuthChangedCallback(string appName, string userJson, string error);
    internal delegate void WebGLIdTokenChangedCallback(string appName, string userJson, string error);
}
