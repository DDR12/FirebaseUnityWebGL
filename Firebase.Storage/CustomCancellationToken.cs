namespace Firebase.Storage
{
    public class CustomCancellationToken
    {
        public delegate void CancelRequestEventHandler();
        public event CancelRequestEventHandler OnCancelRequested;

        bool isCancelled = false;
        public bool IsCancellationRequested
        {
            get => isCancelled;
            set
            {
                if (isCancelled || !value)
                    return;
                isCancelled = value;
                OnCancelRequested?.Invoke();
            }
        }
    }
}
