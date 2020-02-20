using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Firebase.Storage
{
    public sealed partial class StorageReference : IDisposable
    {
        static uint ReferenceIDs = 0;
        static uint TasksIDs = 1;
        static IDictionary<uint, ITransferTaskHandler> TransferTasks;
        static IDictionary<uint, StorageReference> References;
        static StorageReference()
        {
            TransferTasks = new Dictionary<uint, ITransferTaskHandler>();
            References = new Dictionary<uint, StorageReference>();
        }

        public FirebaseStorage Storage { get; }

        /// <summary>
        /// The name of the bucket containing this reference's object.
        /// </summary>
        public string Bucket => StoragePInvoke.GetStorageReferenceBucketName_WebGL(ID);

        /// <summary>
        /// The full path of this object, on Javascript this is called 'fullPath'.
        /// </summary>
        public string Path => StoragePInvoke.GetStorageReferenceFullPath_WebGL(ID);

        /// <summary>
        /// The short name of this object, which is the last component of the full path. For example, if <see cref="Path"/> is 'full/path/image.png', <see cref="Name"/> is 'image.png'.
        /// </summary>
        public string Name => StoragePInvoke.GetStorageReferenceFileName_WebGL(ID);

        /// <summary>
        /// A reference pointing to the parent location of this reference, or null if this reference is the root.
        /// </summary>
        public StorageReference Parent
        {
            get
            {
                StorageReference parent = null;
                string parentPath = StoragePInvoke.GetStorageReferenceParentPath_WebGL(ID);
                if (!string.IsNullOrWhiteSpace(parentPath))
                    parent = Create(Storage, parentPath);
                return parent;
            }
        }

        /// <summary>
        /// A reference to the root of this reference's bucket.
        /// </summary>
        public StorageReference Root => Storage.RootReference;

        private uint ID = 0;

        private StorageReference(FirebaseStorage storage, string path)
        {
            Storage = storage;
            ID = ReferenceIDs++;
            StoragePInvoke.CreateStorageReference_WebGL(ID, storage.App.Name, path);
            References.Add(ID, this);
        }

        /// <summary>
        /// Returns a reference to a relative path from this reference.
        /// </summary>
        /// <param name="path">The relative path from this reference. Leading, trailing, and consecutive slashes are removed.</param>
        /// <returns>The reference to the given path.</returns>
        public StorageReference Child(string path)
        {
            return Create(Storage, $"{Path}/{path}");
        }

        /// <summary>
        /// Deletes the object at this reference's location.
        /// </summary>
        /// <returns>A task that resolves if the deletion succeeded and rejects if it failed, including if the object didn't exist.</returns>
        public Task DeleteAsync()
        {
            var task = WebGLTaskManager.GetTask<StorageException>();
            StoragePInvoke.DeleteStorageReferenceFile_WebGL(ID, task.Task.Id, WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// Fetches a long lived download URL for this object.
        /// </summary>
        /// <returns>A task that resolves with the download URL or rejects if the fetch failed, including if the object did not exist.</returns>
        public Task<string> GetDownloadUrlAsync()
        {
            var task = WebGLTaskManager.GetTask<string>();
            StoragePInvoke.GetStorageReferenceDownloadUrl_WebGL(ID, task.Task.Id, WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// Fetches metadata for the object at this location, if one exists.
        /// </summary>
        /// <returns>A task that resolves with the metadata, or rejects if the fetch failed, including if the object did not exist.</returns>
        public Task<StorageMetadata> GetMetadataAsync()
        {
            var task = WebGLTaskManager.GetTask<StorageMetadata>();
            task.Task.ContinueWith(result =>
            {
                if (result.IsSuccess())
                    result.Result.Reference = this;
            });
            StoragePInvoke.GetStorageReferenceMetadata_WebGL(ID, task.Task.Id, WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        /// <summary>
        /// List items (files) and prefixes (folders) under this storage reference.
        /// List API is only available for Firebase Rules Version 2.
        /// To adhere to Firebase Rules's Semantics, Firebase Storage does not support objects whose paths end with "/" or contain two consecutive "/"s. 
        /// Firebase Storage List API will filter these unsupported objects. 
        /// List() may fail if there are too many unsupported objects in the bucket.
        /// </summary>
        /// <param name="options">See <see cref="ListOptions"/> for details.</param>
        /// <returns>A task that resolves with the items and prefixes. <see cref="ListResult.Prefixes"/> contains references to sub-folders and <see cref="ListResult.Items"/> contains references to objects in this folder. <see cref="ListResult.NextPageToken"/> can be used to get the rest of the results.</returns>
        public Task<ListResult> ListAsync(ListOptions options = null)
        {
            var task = WebGLTaskManager.GetTask<ListResult>();
            StoragePInvoke.GetStorageReferenseListPartial_WebGL(ID, task.Task.Id, ListOptions.ToJson(options), WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// List all items (files) and prefixes (folders) under this storage reference.
        /// The default pagination size is 1000.
        /// Note: The results may not be consistent if objects are changed while this operation is running.
        /// Warning: <see cref="ListAllAsync"/> may potentially consume too many resources if there are too many results.
        /// </summary>
        /// <returns>A task that resolves with all the items and prefixes under the current storage reference. 
        /// <see cref="ListResult.Prefixes"/> contains references to sub-directories and <see cref="ListResult.Items"/> contains references to objects in this folder.
        /// <see cref="ListResult.NextPageToken"/> is never returned.</returns>
        public Task<ListResult> ListAllAsync()
        {
            var task = WebGLTaskManager.GetTask<ListResult>();
            StoragePInvoke.GetStorageReferenseListAll_WebGL(ID, task.Task.Id, WebGLTaskManager.DequeueTask);
            return task.Task;
        }


        /// <summary>
        /// Uploads byte data to this <see cref="StorageReference"/> This is not recommended for large files.
        /// Instead upload a file via <see cref="PutFileAsync"/>
        /// </summary>
        /// <param name="rawBytes">The byte[] to upload.</param>
        /// <param name="customMetadata"><see cref="SettableMetadata"/> containing additional information (MIME type, etc.) about the object being uploaded.</param>
        /// <param name="progressHandler">Usually an instance of <see cref="StorageProgress{T}"/> that will receive periodic updates during the operation. This value can be null.</param>
        /// <param name="cancelToken">A CancellationToken to control the operation and possibly later cancel it. This value may be CancellationToken.None to indicate no value.</param>
        /// <returns>A task which can be used to monitor the upload.</returns>
        public Task<StorageMetadata> PutBytesAsync(byte[] rawBytes, UploadMetadata customMetadata = null,
            IProgress<UploadState> progressHandler = null, CustomCancellationToken cancelToken = null)
        {
            if (rawBytes == null)
                throw new ArgumentNullException(nameof(rawBytes), "Trying to upload a null data.");
            if (rawBytes.Length == 0)
                throw new InvalidOperationException("There is no data to upload, the length is 0");
            uint taskID = 0;
            if (progressHandler != null)
            {
                TransferTaskHandler<UploadState> transferTask = new TransferTaskHandler<UploadState>(this, progressHandler, cancelToken);
                taskID = transferTask.ID;
            }

            string metadataJson = UploadMetadata.ToJson(customMetadata);

            var task = WebGLTaskManager.GetTask<StorageMetadata>();
            task.Task.ContinueWith(result =>
            {
                if (result.IsSuccess())
                    result.Result.Reference = this;
            });
            StoragePInvoke.StorageReferenceUploadBytes_WebGL(ID, taskID, task.Task.Id, rawBytes, rawBytes.Length, metadataJson,
                WebGLTaskManager.DequeueTask, TransferTaskProgressCallback_AOT);
            return task.Task;
        }
        /// <summary>
        /// Uploads a file to this reference's location, throws error if the filepath is invalid or file doesn't exist.
        /// </summary>
        /// <param name="filePath">Path of the file to upload.</param>
        /// <param name="customMetadata"><see cref="SettableMetadata"/> containing additional information (MIME type, etc.) about the object being uploaded.</param>
        /// <param name="progressHandler">Usually an instance of <see cref="StorageProgress{T}"/> that will receive periodic updates during the operation. This value can be null.</param>
        /// <param name="cancelToken">A CancellationToken to control the operation and possibly later cancel it. This value may be CancellationToken.None to indicate no value.</param>
        /// <returns>A task which can be used to monitor the upload.</returns>
        public Task<StorageMetadata> PutFileAsync(string filePath, UploadMetadata customMetadata = null,
            IProgress<UploadState> progressHandler = null, CustomCancellationToken cancelToken = null)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(filePath, nameof(filePath));

            //var task = WebGLTaskManager.GetTask<StorageMetadata, StorageException>();

            //string str = (!filePath.StartsWith("file://") ? filePath : new Uri(filePath).LocalPath);
            //if (!File.Exists(filePath))
            //{
            //    Exception exception = new FileNotFoundException($"{str} not found", filePath);
            //    task.SetException(new StorageException(exception));
            //}
            //else
            //{
            //    task.Task.ContinueWith(result =>
            //    {
                       //if (result.IsSuccess())
            //            result.Result.Reference = this;
            //    });
            //}
            return PutBytesAsync(File.ReadAllBytes(filePath), customMetadata, progressHandler, cancelToken);
        }
        /// <summary>
        /// Uploads a data stream to this reference's location, throws error if the stream is null or empty.
        /// </summary>
        /// <param name="stream">The stream from which to upload the data.</param>
        /// <param name="customMetadata"><see cref="SettableMetadata"/> containing additional information (MIME type, etc.) about the object being uploaded.</param>
        /// <param name="progressHandler">Usually an instance of <see cref="StorageProgress{T}"/> that will receive periodic updates during the operation. This value can be null.</param>
        /// <param name="cancelToken">A CancellationToken to control the operation and possibly later cancel it. This value may be CancellationToken.None to indicate no value.</param>
        /// <returns>A task which can be used to monitor the upload.</returns>
        public Task<StorageMetadata> PutStreamAsync(Stream stream, UploadMetadata customMetadata = null,
            IProgress<UploadState> progressHandler = null, CustomCancellationToken cancelToken = null)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream), "Stream is null!");
            if (stream.Length == 0)
                throw new InvalidOperationException("This stream has no data to upload.");

            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return PutBytesAsync(buffer, customMetadata, progressHandler, cancelToken);
        }

        /// <summary>
        /// Uploads string data to this reference's location.
        /// </summary>
        /// <param name="data">The string to upload.</param>
        /// <param name="stringFormat">The format of the string to upload.</param>
        /// <param name="customMetadata">Metadata for the newly uploaded object.</param>
        /// <param name="progressHandler"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public Task<StorageMetadata> PutStringAsync(string data, StringFormat stringFormat = StringFormat.BASE64, UploadMetadata customMetadata = null,
            IProgress<UploadState> progressHandler = null, CustomCancellationToken cancelToken = null)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(data, nameof(data));

            uint taskID = 0;
            if (progressHandler != null)
            {
                TransferTaskHandler<UploadState> transferTask = new TransferTaskHandler<UploadState>(this, progressHandler, cancelToken);
                taskID = transferTask.ID;
            }

            string metadataJson = UploadMetadata.ToJson(customMetadata);

            var task = WebGLTaskManager.GetTask<StorageMetadata>();
            task.Task.ContinueWith(result =>
            {
                if (result.IsSuccess())
                    result.Result.Reference = this;
            });
            StoragePInvoke.StorageReferenceUploadString_WebGL(ID, taskID, task.Task.Id,data, stringFormat.ToString().ToLower(), metadataJson,
                WebGLTaskManager.DequeueTask, TransferTaskProgressCallback_AOT);

            return task.Task;
        }

        /// <summary>
        /// Downloads the object from this <see cref="StorageReference"/> A byte array will be allocated large enough to hold the entire file in memory.
        /// Therefore, using this method will impact memory usage of your process.If you are downloading many large files, GetStream(StreamDownloadTask.StreamProcessor) may be a better option.
        /// </summary>
        /// <param name="maxDownloadBytes">The maximum allowed size in bytes that will be allocated. Set this parameter to prevent out of memory conditions from occurring. If the download exceeds this limit, the task will fail and an System.IndexOutOfRangeException will be returned.</param>
        /// <returns>A task which can be used to monitor the operation and obtain the result.</returns>
        public Task<byte[]> GetBytesAsync(long maxDownloadBytes)
        {
            return GetBytesAsync(maxDownloadBytes, null, new CustomCancellationToken());
        }
        /// <summary>
        /// Downloads the object from this StorageReference A byte array will be allocated large enough to hold the entire file in memory.
        /// Therefore, using this method will impact memory usage of your process.If you are downloading many large files, GetStream(StreamDownloadTask.StreamProcessor) may be a better option.
        /// </summary>
        /// <param name="maxDownloadBytes">The maximum allowed size in bytes that will be allocated. Set this parameter to prevent out of memory conditions from occurring. If the download exceeds this limit, the task will fail and an System.IndexOutOfRangeException will be returned.</param>
        /// <param name="progressHandler">Usually an instance of <see cref="StorageProgress{T}"/> that will receive periodic updates during the operation. This value can be null.</param>
        /// <param name="cancellationToken">A CancellationToken to control the operation and possibly later cancel it. This value may be CancellationToken.None to indicate no value.</param>
        /// <returns>A task which can be used to monitor the operation and obtain the result.</returns>
        public Task<byte[]> GetBytesAsync(long maxDownloadBytes, IProgress<DownloadState> progressHandler = null,CustomCancellationToken cancellationToken = null)
        {
            var task = WebGLTaskManager.GetTask<byte[]>();
            GetMetadataAsync().ContinueWith(metaDataTask =>
            {
                if (metaDataTask.IsFaulted)
                {
                    task.SetException(metaDataTask.Exception.InnerExceptions);
                    return;
                }
                else if (metaDataTask.IsCanceled)
                {
                    task.SetCanceled();
                    return;
                }
                var metadata = metaDataTask.Result;
                if (maxDownloadBytes > 0 && metadata.SizeBytes > maxDownloadBytes)
                {
                    task.SetException(new IndexOutOfRangeException($"File size {metadata.SizeBytes} is larger than the maximum download size {maxDownloadBytes}"));
                    return;
                }
                long lengthToDownload = maxDownloadBytes <= 0 || maxDownloadBytes >= metadata.SizeBytes ? metadata.SizeBytes : maxDownloadBytes;

                uint taskID = 0;
                if (progressHandler != null)
                {
                    TransferTaskHandler<DownloadState> transferTask = new TransferTaskHandler<DownloadState>(this, progressHandler, cancellationToken);
                    taskID = transferTask.ID;
                }
                StoragePInvoke.StorageReferenceDownloadBytes_WebGL(ID, taskID, task.Task.Id, WebGLTaskManager.DequeueTask, TransferTaskProgressCallback_AOT);

            });
            return task.Task;
        }

        /// <summary>
        /// Updates the metadata for the object at this location, if one exists.
        /// </summary>
        /// <param name="metadata">The new metadata. Setting a property to 'null' removes it on the server, while leaving a property as 'undefined' has no effect.</param>
        /// <returns>A task that resolves with the full updated metadata or rejects if the updated failed, including if the object did not exist.</returns>
        public Task<StorageMetadata> UpdateMetadataAsync(SettableMetadata  metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata), "The metadata object is null!");

            var task = WebGLTaskManager.GetTask<StorageMetadata>();
            task.Task.ContinueWith(result =>
            {
                if (result.IsSuccess())
                    result.Result.Reference = this;
            });
            StoragePInvoke.UpdateStorageReferenceMetadata_WebGL(ID, task.Task.Id, JsonConvert.SerializeObject(metadata), WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// Returns a gs:// URL for this object in the form gs://bucket/path/to/object
        /// </summary>
        /// <returns>The gs:// URL.</returns>
        public override string ToString()
        {
            return StoragePInvoke.StorageReferenceToString_WebGL(ID);
        }

        public void Dispose()
        {
            StoragePInvoke.ReleaseStorageReferense_WebGL(ID);
        }

        public override bool Equals(object obj)
        {
            return obj is StorageReference storageReference && storageReference.ToString().Equals(this.ToString());
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        public static StorageReference Create(FirebaseStorage storage, string path)
        {
            return new StorageReference(storage, path);
        }
        public static StorageReference GetExistingReference(uint ID)
        {
            if (References.TryGetValue(ID, out StorageReference reference))
                return reference;
            return null;
        }
        [AOT.MonoPInvokeCallback(typeof(TransferTaskProgressHandlerWebGL))]
        static void TransferTaskProgressCallback_AOT(uint taskID, string progressJson)
        {
            if (TransferTasks.TryGetValue(taskID, out ITransferTaskHandler task))
                task.NotifyProgress(progressJson);
        }
    }
}
