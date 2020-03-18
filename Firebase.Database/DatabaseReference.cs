using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firebase.Database
{
    /// <summary>
    /// A Firebase reference represents a particular location in your <see cref="FirebaseDatabase"/> and can be used for reading or writing data to that <see cref="FirebaseDatabase"/> location.
    /// </summary>
    public sealed class DatabaseReference : Query
    {
        static uint ReferenceIDs = 0;
        static Dictionary<uint, DatabaseReference> References = new Dictionary<uint, DatabaseReference>();
        static uint TransactionHandlersIDs = 0;
        static IDictionary<uint, ITransactionHandler> TransctionHandlers = new Dictionary<uint, ITransactionHandler>();

        /// <summary>
        /// The last token in the location pointed to by this reference.
        /// </summary>
        public string Key => DatabasePInvoke.GetDatabaseReferenceKey_WebGL(RefID);

        /// <summary>
        /// Used to create underlying reference objects using full key by using firebase.database.ref(fullpath);
        /// </summary>
        internal string FullPath => DatabasePInvoke.GetDatabaseReferenceFullPath_WebGL(RefID);

        DatabaseReference m_Parent = null;
        /// <summary>
        /// A <see cref="DatabaseReference"/> to the parent location, or null if this instance references the root location.
        /// </summary>
        public DatabaseReference Parent
        {
            get
            {
                if(m_Parent == null)
                {
                    string parentPath =DatabasePInvoke.GetDatabaseReferenceParentPath_WebGL(RefID);
                    if (!string.IsNullOrWhiteSpace(parentPath))
                        m_Parent = Create(Database, parentPath);
                }
                return m_Parent;
            }
        }

        /// <summary>
        /// A reference to the root location of this <see cref="FirebaseDatabase"/>.
        /// </summary>
        public DatabaseReference Root => Database.RootReference;
        /// <summary>
        /// Gets the Database instance associated with this reference.
        /// </summary>
        public FirebaseDatabase Database { get; }

        private DatabaseReference(FirebaseDatabase database, string path) 
        {
            RefID = ReferenceIDs++;
            Database = database;
            DatabasePInvoke.CreateDatabaseReference_WebGL(RefID, database.App.Name, path);
            References.Add(RefID, this);
        }

        /// <summary>
        /// Get a reference to location relative to this one.
        /// </summary>
        /// <param name="path">The relative path from this reference to the new one that should be created</param>
        /// <returns>A new <see cref="DatabaseReference"/> to the given path</returns>
        public DatabaseReference Child(string path)
        {
            return new DatabaseReference(Database, $"{FullPath}/{path}");
        }

        /// <summary>
        /// Returns true if both objects reference the same database path.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is DatabaseReference reference && reference != null && reference.ToString().Equals(this.ToString());
        }
        /// <summary>
        /// A hash code based on the string path of the reference.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        /// <summary>
        /// Provides access to disconnect operations at this location.
        /// </summary>
        /// <returns>An object for managing disconnect operations at this location.</returns>
        public OnDisconnect OnDisconnect()
        {
            return new OnDisconnect(this);
        }
        /// <summary>
        /// Create a reference to an auto-generated child location.
        /// The child key is generated client-side and incorporates an estimate of the server's time for sorting purposes.
        /// Locations generated on a single client will be sorted in the order that they are created, and will be sorted approximately in order across all clients.
        /// </summary>
        /// <returns>A <see cref="DatabaseReference"/> pointing to the new location</returns>
        public DatabaseReference Push()
        {
            return Create(Database, DatabasePInvoke.GetPushReferenceFullPath_WebGL(RefID));
        }

        /// <summary>
        /// Set the value at this location to 'null'
        /// </summary>
        /// <returns>The task for this operation.</returns>
        public Task RemoveValueAsync()
        {
            var task = WebGLTaskManager.GetTask();
            DatabasePInvoke.DatabaseReferenceRemoveValue_WebGL(RefID, task.Promise.Id, WebGLTaskManager.DequeueTask);
            return task.Promise;
        }

        /// <summary>
        /// Run a transaction on the data at this location.
        /// A transaction is a data transformation function that is continually attempted until the <see cref="DatabaseReference"/> location remains unchanged during the operation.
        /// </summary>
        /// <param name="transaction">A function to perform the transaction and return a result</param>
        /// <returns></returns>
        public Task<DataSnapshot> RunTransaction(Func<MutableData, TransactionResult> transaction)
        {
            return RunTransaction(transaction, true);
        }

        /// <summary>
        /// Run a transaction on the data at this location.
        /// A transaction is a data transformation function that is continually attempted until the <see cref="DatabaseReference"/> location remains unchanged during the operation.
        /// </summary>
        /// <typeparam name="T">Type of the at this location.</typeparam>
        /// <param name="transaction">A function to perform the transaction and return a result</param>
        /// <param name="fireLocalEvents">Defaults to true. If set to false, events will only be fired for the final result state of the transaction, and not for any intermediate states</param>
        /// <returns></returns>
        public Task<DataSnapshot> RunTransaction(Func<MutableData, TransactionResult> transaction, bool fireLocalEvents)
        {
            TransactionHandler transactionHandler = new TransactionHandler(TransactionHandlersIDs++, this, transaction);
            var task = WebGLTaskManager.GetTask<DataSnapshot>();
            DatabasePInvoke.RunTransaction_WebGL(RefID, task.Promise.Id, transactionHandler.ID, fireLocalEvents, OnTransactionUpdateCallback, WebGLTaskManager.DequeueTask);
            return task.Promise;
        }

        /// <summary>
        /// Set a priority for the data at this <see cref="FirebaseDatabase"/> location.
        /// Priorities can be used to provide a custom ordering for the children at a location (if no priorities are specified, the children are ordered by key).
        /// You cannot set a priority on an empty location. For this reason setValue(data, priority) should be used when setting initial data with a specific priority and setPriority should be used when updating the priority of existing data.
        /// </summary>
        /// <param name="priority">The priority to set at the specified location.</param>
        /// <return>The task for this operation.</returns>
        public Task SetPriorityAsync(string priority)
        {
            var task = WebGLTaskManager.GetTask();
            DatabasePInvoke.SetDatabaseReferencePriority_WebGL(RefID, task.Promise.Id, priority, WebGLTaskManager.DequeueTask);
            return task.Promise;
        }
        /// <summary>
        /// Set a priority for the data at this <see cref="FirebaseDatabase"/> location.
        /// Priorities can be used to provide a custom ordering for the children at a location (if no priorities are specified, the children are ordered by key).
        /// You cannot set a priority on an empty location. For this reason setValue(data, priority) should be used when setting initial data with a specific priority and setPriority should be used when updating the priority of existing data.
        /// </summary>
        /// <param name="priority">The priority to set at the specified location.</param>
        /// <return>The task for this operation.</returns>
        public Task SetPriorityAsync(double priority)
        {
            return SetPriorityAsync(priority.ToString());
        }

        /// <summary>
        /// Set the data at this location to the given string json represenation.
        /// </summary>
        /// <param name="jsonValue">The value to set at this location.</param>
        /// <returns></returns>
        public Task SetRawJsonValueAsync(string jsonValue)
        {
            var task = WebGLTaskManager.GetTask();
            DatabasePInvoke.SetDatabaseReferenceValue_WebGL(RefID, task.Promise.Id, jsonValue, WebGLTaskManager.DequeueTask);
            return task.Promise;
        }
        /// <summary>
        /// Set the data and priority to the given values.
        /// </summary>
        /// <param name="jsonValue">The value to set at this location.</param>
        /// <param name="priority">The priority to set at this location</param>
        /// <returns></returns>
        public Task SetRawJsonValueAsync(string jsonValue, object priority)
        {
            var task = WebGLTaskManager.GetTask();
            DatabasePInvoke.SetDatabaseReferenceValueWithPriority_WebGL(RefID, task.Promise.Id, jsonValue, priority.ToString(), WebGLTaskManager.DequeueTask);
            return task.Promise;

        }
       
        /// <summary>
        /// Set the data at this location to the given value. Passing null to SetValue() will delete the data at the specified location.
        /// </summary>
        /// <param name="value">The value to set at this location.</param>
        /// <returns></returns>
        public Task SetValueAsync(object value)
        {
            return SetRawJsonValueAsync(JsonConvert.SerializeObject(value));
        }
        /// <summary>
        /// Set the data and priority to the given values. Passing null to setValue() will delete the data at the specified location.
        /// </summary>
        /// <param name="value">The value to set at this location.</param>
        /// <param name="priority">The priority to set at this location</param>
        /// <returns></returns>
        public Task SetValueAsync(object value, object priority)
        {
            return SetRawJsonValueAsync(JsonConvert.SerializeObject(value), priority);
        }
  
        /// <summary>
        /// The full location url for this reference.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DatabasePInvoke.GetDatabaseReferenceToString_WebGL(RefID);
        }
        /// <summary>
        /// Update the specific child keys to the specified values. Passing null in a map to UpdateChildrenAsync() will Remove the value at the specified location.
        /// </summary>
        /// <param name="update">The paths to update and their new values</param>
        /// <returns>The Task{TResult} for this operation.</returns>
        public Task UpdateChildrenAsync(IDictionary<string, object> update)
        {
            string updateJson = update == null ? null : JsonConvert.SerializeObject(update);
            var task = WebGLTaskManager.GetTask();
            DatabasePInvoke.UpdateDatabaseReferenceChildren_WebGL(RefID, task.Promise.Id, updateJson, WebGLTaskManager.DequeueTask);
            return task.Promise;
        }

        internal static DatabaseReference GetReference(uint id)
        {
            if (References.TryGetValue(id, out DatabaseReference reference))
                return reference;
            return null;
        }

        /// <summary>
        /// Manually disconnect the <see cref="FirebaseDatabase"/> client from the server and disable automatic reconnection.
        /// Note: Invoking this method will impact all <see cref="FirebaseDatabase"/> connections.
        /// </summary>
        public static void GoOffline()
        {
            FirebaseDatabase.AnyInstance.GoOffline();
        }
        /// <summary>
        /// Manually reestablish a connection to the <see cref="FirebaseDatabase"/> server and enable automatic reconnection. 
        /// Note: Invoking this method will impact all <see cref="FirebaseDatabase"/> connections.
        /// </summary>
        public static void GoOnline()
        {
            FirebaseDatabase.AnyInstance.GoOnline();
        }

        [AOT.MonoPInvokeCallback(typeof(TransactionHandlerCallback))]
        static void OnTransactionUpdateCallback(uint handlerID, string rawData)
        {
            bool success = false;
            if (TransctionHandlers.TryGetValue(handlerID, out ITransactionHandler handler))
            {
                TransactionResult result = handler.Invoke(rawData);
                success = result.IsSuccess;
                rawData = result.RawData;
            }
            DatabasePInvoke.CommunicateTransactionResult_WebGL(handlerID, rawData, success);
        }
       
        public static DatabaseReference Create(FirebaseDatabase database, string path)
        {
            return new DatabaseReference(database, path);
        }
    }
}
