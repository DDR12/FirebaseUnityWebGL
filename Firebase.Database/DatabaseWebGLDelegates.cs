namespace Firebase.Database
{
    internal delegate void TransactionHandlerCallback(uint handlerID, string rawData);
    internal delegate void QueryEventCallbackWebGL(uint refID, string eventType, string snapshotJson, string errorJson, string prevChildName);
}
