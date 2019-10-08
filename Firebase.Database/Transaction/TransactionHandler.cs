using Newtonsoft.Json;
using System;

namespace Firebase.Database
{
    internal class TransactionHandler : ITransactionHandler
    {
        DatabaseReference m_Reference = null;
        Func<MutableData, TransactionResult> m_Transaction = null;
        public uint ID { get; }
        public TransactionHandler(uint id, DatabaseReference databaseReference, Func<MutableData, TransactionResult> transaction)
        {
            ID = id;
            m_Reference = databaseReference;
            m_Transaction = transaction;
        }

        public TransactionResult Invoke(string json)
        {
            TransactionResult result = TransactionResult.Abort();
            try
            {
                MutableData value = null;
                try
                {
                    value = JsonConvert.DeserializeObject<MutableData>(json);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log(ex.Message);
                }
                if (value == null)
                    value =new MutableData();
                result = m_Transaction(value);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log($"Exception in transaction delegate, aborting transaction\n{ex}");
            }
            return result;
        }
     
    }
}
