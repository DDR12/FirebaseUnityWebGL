namespace  Firebase.Database
{
    internal interface ITransactionHandler
    {
        TransactionResult Invoke(string json);
    }
}
