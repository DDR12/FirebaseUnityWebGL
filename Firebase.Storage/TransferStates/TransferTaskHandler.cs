using Newtonsoft.Json;
using System;

namespace Firebase.Storage
{
    public sealed partial class StorageReference
    {
        private class TransferTaskHandler<TProgress> : ITransferTaskHandler, IDisposable
        {
            public uint ID { get; }
            IProgress<TProgress> progressHandler;
            readonly CustomCancellationToken cancellationToken;
            readonly StorageReference m_Reference = null;
            public TransferTaskHandler(StorageReference m_Reference, IProgress<TProgress> progressHandler, CustomCancellationToken cancellationToken)
            {
                this.ID = TasksIDs++;
                this.progressHandler = progressHandler;
                this.cancellationToken = cancellationToken;
                if (cancellationToken != null)
                {
                    cancellationToken.OnCancelRequested += CancellationToken_OnCancelRequested;
                }
                TransferTasks.Add(ID, this);
                this.m_Reference = m_Reference;
            }
            ~TransferTaskHandler()
            {
                Dispose();
            }

            private void CancellationToken_OnCancelRequested()
            {
                StoragePInvoke.CancelTransferTask_WebGL(ID);
            }

            public void NotifyProgress(string json)
            {
                if (progressHandler != null)
                {
                    TProgress data = default;
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        data = JsonConvert.DeserializeObject<TProgress>(json);
                        if (data is BaseTransferState transferState && transferState != null)
                        {
                            transferState.Reference = m_Reference;
                        }
                    }
                    progressHandler.Report(data);

                }
            }

            public void Dispose()
            {
                TransferTasks.Remove(ID);
            }
        }
    }
}
