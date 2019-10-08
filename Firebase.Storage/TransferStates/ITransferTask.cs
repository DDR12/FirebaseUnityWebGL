namespace Firebase.Storage
{
    public sealed partial class StorageReference
    {
        private interface ITransferTaskHandler
        {
            void NotifyProgress(string json);
        }
    }
}
