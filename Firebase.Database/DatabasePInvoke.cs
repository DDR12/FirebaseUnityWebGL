using System.Runtime.InteropServices;

namespace Firebase.Database
{
    internal class DatabasePInvoke
    {
        [DllImport("__Internal")]
        public static extern void DatabaseGoOffline_WebGL(string appName);

        [DllImport("__Internal")]
        public static extern void DatabaseGoOnline_WebGL(string appName);

        [DllImport("__Internal")]
        public static extern string DatabaseReferenceFromPath_WebGL(string appName, string path);

        [DllImport("__Internal")]
        public static extern string DatabaseReferenceFromURL_WebGL(string appName, string url);

        [DllImport("__Internal")]
        public static extern void CreateDatabaseReference_WebGL(uint refID, string appName, string path);

        [DllImport("__Internal")]
        public static extern string GetDatabaseReferenceKey_WebGL(uint refID);

        [DllImport("__Internal")]
        public static extern string GetDatabaseReferenceParentPath_WebGL(uint refID);

        [DllImport("__Internal")]
        public static extern string GetDatabaseReferenceRootPath_WebGL(uint refID);

        [DllImport("__Internal")]
        public static extern string GetDatabaseReferenceFullPath_WebGL(uint refID);

        [DllImport("__Internal")]
        public static extern string GetPushReferenceFullPath_WebGL(uint refID);

        [DllImport("__Internal")]
        public static extern void DatabaseReferenceRemoveValue_WebGL(uint refID, int taskID, VoidTaskWebGLDelegate callback);

        [DllImport("__Internal")]
        public static extern void RunTransaction_WebGL(uint refID, int taskID, uint transcationHandlerID, bool fireLocalEvents, TransactionHandlerCallback transactionUpdateCallback, GenericTaskWebGLDelegate transactionCompleteCallback);

        [DllImport("__Internal")]
        public static extern void CommunicateTransactionResult_WebGL(uint transactionID, string data, bool success);

        [DllImport("__Internal")]
        public static extern void SetDatabaseReferencePriority_WebGL(uint refID, int taskID, string priority, VoidTaskWebGLDelegate callback);

        [DllImport("__Internal")]
        public static extern void SetDatabaseReferenceValue_WebGL(uint refID, int taskID, string valueJson, VoidTaskWebGLDelegate callback);

        [DllImport("__Internal")]
        public static extern void SetDatabaseReferenceValueWithPriority_WebGL(uint refID, int taskID, string valueJson, string priority, VoidTaskWebGLDelegate callback);

        [DllImport("__Internal")]
        public static extern string GetDatabaseReferenceToString_WebGL(uint refID);

        [DllImport("__Internal")]
        public static extern void UpdateDatabaseReferenceChildren_WebGL(uint refID, int taskID, string update, VoidTaskWebGLDelegate callback);     

        [DllImport("__Internal")]
        public static extern void CancelOnDisconnectForReference_WebGL(uint refID, int taskID, VoidTaskWebGLDelegate callback);

        [DllImport("__Internal")]
        public static extern void RemoveValueOnDisconnectForReference_WebGL(uint refID, int taskID, VoidTaskWebGLDelegate callback);

        [DllImport("__Internal")]
        public static extern void SetValueOnDisconnectForReference_WebGL(uint refID, int taskID, string value, VoidTaskWebGLDelegate callback);

        [DllImport("__Internal")]
        public static extern void SetValueOnDisconnectWithProirity_WebGL(uint refID, int taskID, string valueJson, string priority, VoidTaskWebGLDelegate callback);

        [DllImport("__Internal")]
        public static extern void UpdateChildrenOnDisconnectForReference_WebGL(uint refID, int taskID, string updates, VoidTaskWebGLDelegate callback);

        [DllImport("__Internal")]
        public static extern void GetQueryValue_WebGL(uint refID, int taskID, string queryList, GenericTaskWebGLDelegate callback);

        [DllImport("__Internal")]
        public static extern void ListenToDatabaseReferenceEventOfType_WebGL(uint refID, uint listenerID, string eventType, string queryList, QueryEventCallbackWebGL callback);

        [DllImport("__Internal")]
        public static extern void UnlistenToDatabaseReferenceEventOfType_WebGL(uint refID, uint listenerID, string eventType, string queryList);

    }
}
