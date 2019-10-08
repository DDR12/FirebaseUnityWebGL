namespace Firebase.Database
{
    /// <summary>
    /// Instances of this class represent the desired outcome of a single Run of a transaction.
    /// </summary>
    public sealed class TransactionResult
    {
        /// <summary>
        /// Whether or not this result is a success.
        /// </summary>
        public bool IsSuccess { get; private set; }

        private MutableData Data = null;
        /// <summary>
        /// Raw JSON representation of the data at the location of this transaction.
        /// </summary>
        internal string RawData
        {
            get
            {
                if (Data == null)
                    return null;
                return Data.RawJsonValue;
            }
        }

        internal TransactionResult(bool success)
        {
            this.IsSuccess = success;
        }

        /// <summary>
        /// Aborts the transaction run with <see cref="DatabaseReference.RunTransaction"/> and returns an aborted <see cref="TransactionResult"/> which can be returned from RunTransaction.
        /// </summary>
        /// <returns></returns>
        public static TransactionResult Abort()
        {
            return new TransactionResult(false);
        }

        /// <summary>
        /// Builds a successful result to be returned from the handler passed to <see cref="DatabaseReference.RunTransaction"/>
        /// </summary>
        /// <param name="resultData">The desired data to be stored at the location.</param>
        /// <returns>A <see cref="TransactionResult"/> indicating the new data to be stored at the location.</returns>
        public static TransactionResult Success(MutableData resultData)
        {
            return new TransactionResult(true)
            {
                Data = resultData,
            };
        }
    }
}
