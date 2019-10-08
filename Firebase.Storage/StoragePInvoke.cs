using System.Runtime.InteropServices;

namespace Firebase.Storage
{
    internal class StoragePInvoke
    {
        [DllImport("__Internal")]
        public static extern void SetMaxOperationRetryTime_WebGL(string appName, uint maxTime);
        [DllImport("__Internal")]
        public static extern uint GetMaxOperationRetryTime_WebGL(string appName);

        [DllImport("__Internal")]
        public static extern void SetMaxUploadRetryTime_WebGL(string appName, uint maxTime);
        [DllImport("__Internal")]
        public static extern uint GetMaxUploadRetryTime_WebGL(string appName);

        [DllImport("__Internal")]
        public static extern void SetMaxDownloadRetryTime_WebGL(string appName, uint maxTime);
        [DllImport("__Internal")]
        public static extern uint GetMaxDownloadRetryTime_WebGL(string appName);

        [DllImport("__Internal")]
        public static extern string StorageReferenceFromURL_WebGL(string appName, string url);
        [DllImport("__Internal")]
        public static extern string StorageReferenceFromPath_WebGL(string appName, string path);

        [DllImport("__Internal")]
        public static extern void CreateStorageReference_WebGL(uint referenceID, string appName, string path);

        [DllImport("__Internal")]
        public static extern void ReleaseStorageReferense_WebGL(uint refID);

        [DllImport("__Internal")]
        public static extern string GetStorageReferenceParentPath_WebGL(uint referenceID);
        [DllImport("__Internal")]
        public static extern string GetStorageReferenceBucketName_WebGL(uint referenceID);
        [DllImport("__Internal")]
        public static extern string GetStorageReferenceFileName_WebGL(uint referenceID);
        [DllImport("__Internal")]
        public static extern string GetStorageReferenceFullPath_WebGL(uint referenceID);
        [DllImport("__Internal")]
        public static extern string StorageReferenceToString_WebGL(uint referenceID);
        [DllImport("__Internal")]
        public static extern void DeleteStorageReferenceFile_WebGL(uint referenceID, int taskID, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void GetStorageReferenceDownloadUrl_WebGL(uint referenceID, int taskID, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void GetStorageReferenceMetadata_WebGL(uint referenceID, int taskID, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void GetStorageReferenseListPartial_WebGL(uint referenceID, int taskID, string optionsJson, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void GetStorageReferenseListAll_WebGL(uint referenceID, int taskID, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void UpdateStorageReferenceMetadata_WebGL(uint referenceID, int taskID, string metadata, GenericTaskWebGLDelegate callback);

        [DllImport("__Internal")]
        public static extern void StorageReferenceUploadBytes_WebGL(uint refID, uint transferTaskID, int taskID, byte[] bytes, int bytesLength, string customMetadata,
            GenericTaskWebGLDelegate promiseCallback, TransferTaskProgressHandlerWebGL progressCallback);
        [DllImport("__Internal")]
        public static extern uint StorageReferenceUploadString_WebGL(uint refID, uint transferTaskID, int taskID, string data, string stringFormat, string customMetadata,
            GenericTaskWebGLDelegate promiseCallback, TransferTaskProgressHandlerWebGL progressCallback);

        [DllImport("__Internal")]
        public static extern void StorageReferenceDownloadBytes_WebGL(uint refID, uint transferTaskID, int taskID,
            ByteArrayTaskWebGLDelegate promiseCallback, TransferTaskProgressHandlerWebGL progressCallback);


        [DllImport("__Internal")]
        public static extern bool CancelTransferTask_WebGL(uint transferTaskID);

    }
}
