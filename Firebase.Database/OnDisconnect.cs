using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Firebase.Database
{
    /// <summary>
    /// The `onDisconnect` class allows you to write or clear data when your client
    /// disconnects from the Database server. These updates occur whether your
    /// client disconnects cleanly or not, so you can rely on them to clean up data
    /// even if a connection is dropped or a client crashes.
    /// The `onDisconnect` class is most commonly used to manage presence in
    /// applications where it is useful to detect how many clients are connected and
    /// when other clients disconnect. See https://firebase.google.com/docs/database/web/offline-capabilities
    /// To avoid problems when a connection is dropped before the requests can be
    /// transferred to the Database server, these functions should be called before
    /// writing any data.
    /// Note that `onDisconnect` operations are only triggered once. If you want an
    /// operation to occur each time a disconnect occurs, you'll need to re-establish
    /// the `onDisconnect` operations each time you reconnect.
    /// </summary>
    public sealed class OnDisconnect
    {
        DatabaseReference m_Reference = null;
        public OnDisconnect(DatabaseReference reference)
        {
            m_Reference = reference;
        }

        /// <summary>
        /// Cancel any disconnect operations that are queued up at this location.
        /// </summary>
        /// <returns>The task for this operation.</returns>
        public Task Cancel()
        {
            var task = WebGLTaskManager.GetTask();
            DatabasePInvoke.CancelOnDisconnectForReference_WebGL(m_Reference.RefID, task.Promise.Id, WebGLTaskManager.DequeueTask);
            return task.Promise;
        }

        /// <summary>
        /// Remove the value at this location when the client disconnects.
        /// </summary>
        /// <returns>The task for this operation.</returns>
        public Task RemoveValue()
        {
            var task = WebGLTaskManager.GetTask();
            DatabasePInvoke.RemoveValueOnDisconnectForReference_WebGL(m_Reference.RefID, task.Promise.Id, WebGLTaskManager.DequeueTask);
            return task.Promise;
        }

        /// <summary>
        /// Ensure the data at this location is set to the specified value when the client is disconnected (due to closing the browser, navigating to a new page, or network issues).
        /// This method is especially useful for implementing "presence" systems, where a value should be changed or cleared when a user disconnects so that they appear "offline" to other users.
        /// </summary>
        /// <param name="value">The value to be set when a disconnect occurs</param>
        /// <returns>The task for this operation.</returns>
        public Task SetValue(object value)
        {
            var task = WebGLTaskManager.GetTask();
            DatabasePInvoke.SetValueOnDisconnectForReference_WebGL(m_Reference.RefID, task.Promise.Id, JsonConvert.SerializeObject(value), WebGLTaskManager.DequeueTask);
            return task.Promise;
        }
        /// <summary>
        /// Ensure the data at this location is set to the specified value and priority when the client is disconnected (due to closing the browser, navigating to a new page, or network issues).
        /// This method is especially useful for implementing "presence" systems, where a value should be changed or cleared when a user disconnects so that they appear "offline" to other users.
        /// </summary>
        /// <param name="value">The value to be set when a disconnect occurs</param>
        /// <param name="priority">The priority to be set when a disconnect occurs</param>
        /// <returns>The task for this operation.</returns>
        public Task SetValue(object value, string priority)
        {
            var task = WebGLTaskManager.GetTask();
            DatabasePInvoke.SetValueOnDisconnectWithProirity_WebGL(m_Reference.RefID, task.Promise.Id, JsonConvert.SerializeObject(value), priority, WebGLTaskManager.DequeueTask);
            return task.Promise;
        }

        /// <summary>
        /// Ensure the data at this location is set to the specified value and priority when the client is disconnected (due to closing the browser, navigating to a new page, or network issues).
        /// This method is especially useful for implementing "presence" systems, where a value should be changed or cleared when a user disconnects so that they appear "offline" to other users.
        /// </summary>
        /// <param name="value">The value to be set when a disconnect occurs.</param>
        /// <param name="priority">The priority to be set when a disconnect occurs.</param>
        /// <returns>The task for this operation.</returns>
        public Task SetValue(object value, double priority)
        {
            return SetValue(value, priority.ToString());
        }

        /// <summary>
        /// Ensure the data has the specified child values updated when the client is disconnected.
        /// </summary>
        /// <param name="update">The paths to update, along with their desired values</param>
        /// <returns>The task for this operation.</returns>
        public Task UpdateChildren(IDictionary<string, object> update)
        {
            var task = WebGLTaskManager.GetTask();
            DatabasePInvoke.UpdateChildrenOnDisconnectForReference_WebGL(m_Reference.RefID, task.Promise.Id, JsonConvert.SerializeObject(update), WebGLTaskManager.DequeueTask);
            return task.Promise;
        }
    }
}
