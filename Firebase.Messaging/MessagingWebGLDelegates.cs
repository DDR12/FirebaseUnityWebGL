namespace Firebase.Messaging
{
    internal delegate void MessageReceivedWebGLCallback(string appName, string messageJson, string errorJson);
    internal delegate void TokenReceivedWebGLCallback(string appName, string tokenJson, string errorJson);
}
