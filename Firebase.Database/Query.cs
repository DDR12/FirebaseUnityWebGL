using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firebase.Database
{
    /// <summary>
    /// The <see cref="Query"/> class (and its subclass, <see cref="DatabaseReference"/> ) are used for reading data.
    /// </summary>
    public class Query
    {
        private event EventHandler<ChildChangedEventArgs> ChildAddedEvent;
        /// <summary>
        /// Event raised when children nodes are added relative to this location.
        /// </summary>
        public event EventHandler<ChildChangedEventArgs> ChildAdded
        {
            add
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.SubscribeToEvent(ChildAddedEvent, value);
                if (id != null)
                {
                    DatabasePInvoke.ListenToDatabaseReferenceEventOfType_WebGL(RefID, id.Value, ChildAdded_Event, GetQueriesListJson(), OnQueryEventCallback_AOT);
                }
            }
            remove
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.UnsubscribeFromEvent(ChildAddedEvent, value);
                if (id != null)
                {
                    DatabasePInvoke.UnlistenToDatabaseReferenceEventOfType_WebGL(RefID, id.Value, ChildAdded_Event, GetQueriesListJson());
                }
            }
        }
        private event EventHandler<ChildChangedEventArgs> ChildChangedEvent;
        /// <summary>
        /// Event raised when children nodes are changed relative to this location.
        /// </summary>
        public event EventHandler<ChildChangedEventArgs> ChildChanged
        {
            add
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.SubscribeToEvent(ChildChangedEvent, value);
                if(id != null)
                {
                    DatabasePInvoke.ListenToDatabaseReferenceEventOfType_WebGL(RefID, id.Value, ChildChanged_Event, GetQueriesListJson(), OnQueryEventCallback_AOT);
                }
            }
            remove
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.UnsubscribeFromEvent(ChildChangedEvent, value);
                if(id != null)
                {
                    DatabasePInvoke.UnlistenToDatabaseReferenceEventOfType_WebGL(RefID, id.Value, ChildChanged_Event, GetQueriesListJson());
                }
            }
        }
        private event EventHandler<ChildChangedEventArgs> ChildMovedEvent;
        /// <summary>
        /// Event raised when children nodes are moved relative to this location.
        /// </summary>
        public event EventHandler<ChildChangedEventArgs> ChildMoved
        {
            add
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.SubscribeToEvent(ChildMovedEvent, value);
                if(id != null)
                {
                    DatabasePInvoke.ListenToDatabaseReferenceEventOfType_WebGL(RefID, id.Value, ChildMoved_Event, GetQueriesListJson(), OnQueryEventCallback_AOT);
                }
            }
            remove
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.UnsubscribeFromEvent(ChildMovedEvent, value);
                if(id != null)
                {
                    DatabasePInvoke.UnlistenToDatabaseReferenceEventOfType_WebGL(RefID, id.Value, ChildMoved_Event, GetQueriesListJson());
                }
            }

        }
        private event EventHandler<ChildChangedEventArgs> ChildRemovedEvent;
        /// <summary>
        /// Event raised when children nodes are removed relative to this location.
        /// </summary>
        public event EventHandler<ChildChangedEventArgs> ChildRemoved
        {
            add
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.SubscribeToEvent(ChildRemovedEvent, value);
                if(id != null)
                {
                    DatabasePInvoke.ListenToDatabaseReferenceEventOfType_WebGL(RefID, id.Value, ChildRemoved_Event, GetQueriesListJson(), OnQueryEventCallback_AOT);
                }
            }
            remove
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.UnsubscribeFromEvent(ChildRemovedEvent, value);
                if(id != null)
                {
                    DatabasePInvoke.UnlistenToDatabaseReferenceEventOfType_WebGL(RefID, id.Value, ChildRemoved_Event, GetQueriesListJson());
                }
            }
        }
        private event EventHandler<ValueChangedEventArgs> ValueChangedEvent;
        /// <summary>
        /// Event for changes in the data at this location.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.SubscribeToEvent(ValueChangedEvent, value);
                if(id != null)
                {
                    DatabasePInvoke.ListenToDatabaseReferenceEventOfType_WebGL(RefID, id.Value, ValueChanged_Event, GetQueriesListJson(), OnQueryEventCallback_AOT);
                }
            }
            remove
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.UnsubscribeFromEvent(ValueChangedEvent, value);
                if(id != null)
                {
                    DatabasePInvoke.UnlistenToDatabaseReferenceEventOfType_WebGL(RefID, id.Value, ValueChanged_Event, GetQueriesListJson());
                }
            }

        }

        /// <summary>
        /// A <see cref="DatabaseReference"/> to this location
        /// </summary>
        [JsonIgnore]
        public DatabaseReference Reference => DatabaseReference.GetReference(RefID);

        [JsonIgnore]
        private Query parentQuery = null;


        internal uint RefID { get; set; }

        [JsonProperty("kind")]
        QueryKind Kind { get; set; }
        [JsonProperty("value")]
        object Value { get; set; }
        [JsonProperty("keyValue")]
        string KeyValue { get; set; }

        private Query() { }
        /// <summary>
        /// Create a query constrained to only return child nodes with a value less than or equal to the given value, using the given orderBy directive or priority as default.
        /// </summary>
        /// <param name="value">The value to end at, inclusive</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query EndAt(string value)
        {
            return EndAt(value, null);
        }

        /// <summary>
        /// Create a query constrained to only return child nodes with a value less than or equal to the given value, using the given orderBy directive or priority as default.
        /// </summary>
        /// <param name="value">The value to end at, inclusive</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query EndAt(double value)
        {
            return EndAt(value, null);
        }

        /// <summary>
        /// Create a query constrained to only return child nodes with a value less than or equal to the given value, using the given orderBy directive or priority as default.
        /// </summary>
        /// <param name="value">The value to end at, inclusive</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query EndAt(bool value)
        {
            return EndAt(value, null);
        }
        /// <summary>
        /// Create a query constrained to only return child nodes with a value less than or equal to the given value, using the given orderBy directive or priority as default, and additionally only child nodes with a key key less than or equal to the given key.
        /// </summary>
        /// <param name="value">The value to end at, inclusive</param>
        /// <param name="key">The key to end at, inclusive</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query EndAt(string value, string key)
        {
            return EndAtInternal(value, key);
        }
        /// <summary>
        /// Create a query constrained to only return child nodes with a value less than or equal to the given value, using the given orderBy directive or priority as default, and additionally only child nodes with a key less than or equal to the given key.
        /// </summary>
        /// <param name="value">The value to end at, inclusive</param>
        /// <param name="key">The key to end at, inclusive</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query EndAt(double value, string key)
        {
            return EndAtInternal(value, key);
        }
        /// <summary>
        /// Create a query constrained to only return child nodes with a value less than or equal to the given value, using the given orderBy directive or priority as default, and additionally only child nodes with a key less than or equal to the given key.
        /// </summary>
        /// <param name="value">The value to end at, inclusive</param>
        /// <param name="key">The key to end at, inclusive</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query EndAt(bool value, string key)
        {
            return EndAtInternal(value, key);
        }
        private Query EndAtInternal(object value, string key)
        {
            return Add(new Query()
            {
                Kind = QueryKind.EndAt,
                Value = value,
                KeyValue = key,
            });
        }
        /// <summary>
        /// Create a query constrained to only return child nodes with the given value.
        /// </summary>
        /// <param name="value">The value to query for</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query EqualTo(string value)
        {
            return EqualTo(value, null);
        }

        /// <summary>
        /// Create a query constrained to only return child nodes with the given value.
        /// </summary>
        /// <param name="value">The value to query for</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query EqualTo(double value)
        {
            return EqualTo(value, null);
        }

        /// <summary>
        /// Create a query constrained to only return child nodes with the given value.
        /// </summary>
        /// <param name="value">The value to query for</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query EqualTo(bool value)
        {
            return EqualTo(value, null);
        }
        /// <summary>
        /// Create a query constrained to only return the child node with the given key and value. 
        /// Note that there is at most one such child as names are unique.
        /// </summary>
        /// <param name="value">The value to query for</param>
        /// <param name="key">The key of the child</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query EqualTo(string value, string key)
        {
            return EqualToInternal(value, key);
        }
        /// <summary>
        /// Create a query constrained to only return the child node with the given key and value. 
        /// Note that there is at most one such child as names are unique.
        /// </summary>
        /// <param name="value">The value to query for</param>
        /// <param name="key">The key of the child</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query EqualTo(double value, string key)
        {
            return EqualToInternal(value, key);
        }

        /// <summary>
        /// Create a query constrained to only return the child node with the given key and value. 
        /// Note that there is at most one such child as names are unique.
        /// </summary>
        /// <param name="value">The value to query for</param>
        /// <param name="key">The key of the child</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query EqualTo(bool value, string key)
        {
            return EqualToInternal(value, key);
        }
        private Query EqualToInternal(object value, string key)
        {
            return Add(new Query()
            {
                Value = value,
                KeyValue = key,
                Kind = QueryKind.EqualTo,
            });
        }
        /// <summary>
        /// Gets data at this location, or null if doesn't exist.
        /// </summary>
        /// <returns></returns>
        public Task<DataSnapshot> GetValueAsync()
        {
            var task = WebGLTaskManager.GetTask<DataSnapshot>();
            DatabasePInvoke.GetQueryValue_WebGL(RefID, task.Task.Id, GetQueriesListJson(), WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        
        /// <summary>
        /// Create a query with limit and anchor it to the start of the window.
        /// </summary>
        /// <param name="limit">The maximum number of child nodes to return</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query LimitToFirst(int limit)
        {
            return Add(new Query()
            {
                Value = limit,
                Kind = QueryKind.LimitToFirst,
            });
        }

        /// <summary>
        /// Create a query with limit and anchor it to the end of the window
        /// </summary>
        /// <param name="limit">The maximum number of child nodes to return</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query LimitToLast(int limit)
        {
            return Add(new Query()
            {
                Value = limit,
                Kind = QueryKind.LimitToLast,
            });
        }

        /// <summary>
        /// Create a query in which child nodes are ordered by the values of the specified path.
        /// </summary>
        /// <param name="path">The path to the child node to use for sorting</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query OrderByChild(string path)
        {
            return Add(new Query()
            {
                Value = path,
                Kind = QueryKind.OrderByChild,
            });
        }
        /// <summary>
        /// Create a query in which child nodes are ordered by their keys.
        /// </summary>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query OrderByKey()
        {
            return Add(new Query()
            {
                Kind = QueryKind.OrderByKey,
            });
        }

        /// <summary>
        /// Create a query in which child nodes are ordered by their priorities.
        /// </summary>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query OrderByPriority()
        {
            return Add(new Query()
            {
                Kind = QueryKind.OrderByPriority,
            });
        }

        /// <summary>
        /// Create a query in which nodes are ordered by their value.
        /// </summary>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query OrderByValue()
        {
            return Add(new Query()
            {
                Kind = QueryKind.OrderByValue,
            });
        }

        /// <summary>
        /// Create a query constrained to only return child nodes with a value greater than or equal to the given value, using the given orderBy directive or priority as default.
        /// </summary>
        /// <param name="value">The value to start at, inclusive</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query StartAt(string value)
        {
            return StartAt(value, null);
        }

        /// <summary>
        /// Create a query constrained to only return child nodes with a value greater than or equal to the given value, using the given orderBy directive or priority as default.
        /// </summary>
        /// <param name="value">The value to start at, inclusive</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query StartAt(double value)
        {
            return StartAt(value, null);
        }
        /// <summary>
        /// Create a query constrained to only return child nodes with a value greater than or equal to the given value, using the given orderBy directive or priority as default.
        /// </summary>
        /// <param name="value">The value to start at, inclusive</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query StartAt(bool value)
        {
            return StartAt(value, null);
        }
        /// <summary>
        /// Create a query constrained to only return child nodes with a value greater than or equal to the given value, using the given orderBy directive or priority as default, and additionally only child nodes with a key greater than or equal to the given key.
        /// </summary>
        /// <param name="value">The priority to start at, inclusive</param>
        /// <param name="key">The key to start at, inclusive</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query StartAt(string value, string key)
        {
            return StartAtInternal(value, key);
        }

        /// <summary>
        /// Create a query constrained to only return child nodes with a value greater than or equal to the given value, using the given orderBy directive or priority as default, and additionally only child nodes with a key greater than or equal to the given key.
        /// </summary>
        /// <param name="value">The priority to start at, inclusive</param>
        /// <param name="key">The key to start at, inclusive</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query StartAt(double value, string key)
        {
            return StartAtInternal(value, key);
        }
        /// <summary>
        /// Create a query constrained to only return child nodes with a value greater than or equal to the given value, using the given orderBy directive or priority as default, and additionally only child nodes with a key greater than or equal to the given key.
        /// </summary>
        /// <param name="value">The priority to start at, inclusive</param>
        /// <param name="key">The key to start at, inclusive</param>
        /// <returns>A <see cref="Query"/> with the new constraint</returns>
        public Query StartAt(bool value, string key)
        {
            return StartAtInternal(value, key);
        }
        /// <summary>
        /// Keep this reference synced.
        /// </summary>
        /// <param name="keepSynced"></param>
        public void KeepSynced(bool keepSynced)
        {
            PlatformHandler.NotifyWebGLFeatureDoesntHaveAMatch();
        }
        private Query StartAtInternal(object value, string key)
        {
            return Add(new Query()
            {
                Value = value,
                KeyValue = key,
                Kind = QueryKind.StartAt,
            });
        }
        private Query Add(Query query)
        {
            query.RefID = RefID;
            query.parentQuery = this;
            return query;
        }
        private void NotifyEventOccured(string eventType, string snapshotJson, string errorJson, string prevChildName)
        {
            DatabaseError error = DatabaseError.FromError(FirebaseError.FromJson(errorJson));
            DataSnapshot snapshot = string.IsNullOrWhiteSpace(snapshotJson) ? null : JsonConvert.DeserializeObject<DataSnapshot>(snapshotJson);
            switch (eventType)
            {
                case ChildAdded_Event:
                    if (error != null)
                        ChildAddedEvent?.Invoke(this, new ChildChangedEventArgs(error));
                    else
                        ChildAddedEvent?.Invoke(this, new ChildChangedEventArgs(snapshot, prevChildName));
                    break;
                case ChildChanged_Event:
                    if (error != null)
                        ChildChangedEvent?.Invoke(this, new ChildChangedEventArgs(error));
                    else
                        ChildChangedEvent?.Invoke(this, new ChildChangedEventArgs(snapshot, prevChildName));
                    break;
                case ChildMoved_Event:
                    if (error != null)
                        ChildMovedEvent?.Invoke(this, new ChildChangedEventArgs(error));
                    else
                        ChildMovedEvent?.Invoke(this, new ChildChangedEventArgs(snapshot, prevChildName));
                    break;
                case ChildRemoved_Event:
                    if (error != null)
                        ChildRemovedEvent?.Invoke(this, new ChildChangedEventArgs(error));
                    else
                        ChildRemovedEvent?.Invoke(this, new ChildChangedEventArgs(snapshot, prevChildName));
                    break;
                case ValueChanged_Event:
                    if (error != null)
                        ValueChangedEvent?.Invoke(this, new ValueChangedEventArgs(error));
                    else
                        ValueChangedEvent?.Invoke(this, new ValueChangedEventArgs(snapshot));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), $"Event type: {eventType} is not a valid event Type");
            }
        }
        /// <summary>
        /// Creates a flat list of the queries linked list leading to this top node.
        /// </summary>
        /// <returns></returns>
        private IList<Query> GetQueries()
        {
            IList<Query> queries = new List<Query>();
            var currentQuery = this;
            while (currentQuery != null && currentQuery is DatabaseReference == false)
            {
                queries.Insert(0, currentQuery);
                currentQuery = currentQuery.parentQuery;
            }
            return queries;
        }
        /// <summary>
        /// Serializes the queries into a json chain to be used on the other side.
        /// </summary>
        /// <returns></returns>
        private string GetQueriesListJson()
        {
            return JsonConvert.SerializeObject(GetQueries(), FirebaseJsonSettings.Settings);
        }
        [AOT.MonoPInvokeCallback(typeof(QueryEventCallbackWebGL))]
        private static void OnQueryEventCallback_AOT(uint refID, string eventType, string snapshotJson, string errorJson, string prevChildName)
        {
            var reference = DatabaseReference.GetReference(refID);
            if (reference != null)
                reference.NotifyEventOccured(eventType, snapshotJson, errorJson, prevChildName);
            
        }
        private enum QueryKind
        {
            EndAt,
            EqualTo,
            LimitToFirst,
            LimitToLast,
            OrderByChild,
            OrderByKey,
            OrderByPriority,
            OrderByValue,
            StartAt,

        }

        const string ChildAdded_Event= "child_added";
        const string ChildChanged_Event= "child_changed";
        const string ChildMoved_Event= "child_moved";
        const string ChildRemoved_Event= "child_removed";
        const string ValueChanged_Event= "value";
    }
}
