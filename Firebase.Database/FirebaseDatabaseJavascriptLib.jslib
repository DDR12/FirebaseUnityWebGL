var FirebaseDatabaseJavascriptLib =
{
    $DatabaseWebGL:
    {
        dbReferences: {},
        transactionsResults: {},
        eventListeners: {},

        AddDatabaseReference: function (reference)
        {
            this.dbReferences[reference.id] = reference.ref;
        },
        RemoveDatabaseReference: function (id)
        {
            delete this.dbReferences[id];
        },
        GetDatabaseReference: function (id, logIfNotFound)
        {
            var ref = this.dbReferences[id].ref;
            if (logIfNotFound && !ref)
            {
                console.log("The underlying object of the DatabaseReference was lost.");
            }
            return ref;
        },
        AddEventListener: function(id, listener)
        {
            this.eventListeners[id] = listener;
        },
        RemoveEventListener: function (id)
        {
            delete this.eventListeners[id];
        },
        GetEventListener: function (id)
        {
            return this.eventListeners[id];
        },
        GetDatabaseReferenceFullKey: function (ref)
        {
            return ref.toString().substring(ref.root.toString().length - 1);
        },
        CreateSerializableSnapshotObject: function (refID, snapshot)
        {
            var snapshotObj =
            {
                refID: refID,
                key: snapshot ? snapshot.key : null,
                priority: snapshot ? snapshot.getPriority() : null,
                snapshotRawJson: snapshot ? snapshot.toJSON() : null,
            };
            return snapshotObj;
        },
        CreateQueryOnReferenceFromQueryList(dbRef, queries)
        {
            //enum QueryKind
            //{
            //    EndAt = 0,
            //    EqualTo = 1,
            //    LimitToFirst = 2,
            //    LimitToLast = 3,
            //    OrderByChild = 4,
            //    OrderByKey = 5,
            //    OrderByPriority = 6,
            //    OrderByValue = 7,
            //    StartAt = 8,
            //}
            var query = null;
            for (i = 0; i < queries.length; i++)
            {
                var queryData = queries[i];
                var kind = queryData.kind;
                var value = queryData.value;
                var key = queryData.keyValue ? queryData.keyValue : null;

                if (!query)
                {
                    switch (kind)
                    {
                        case 0:
                            query = dbRef.endAt(value, key);
                        case 1:
                            query = dbRef.equalTo(value, key);
                        case 2:
                            query = dbRef.limitToFirst(value);
                        case 3:
                            query = dbRef.limitToLast(value);
                        case 4:
                            query = dbRef.orderByChild(value);
                        case 5:
                            query = dbRef.orderByKey();
                        case 6:
                            query = dbRef.orderByPriority();
                        case 7:
                            query = dbRef.orderByValue();
                        case 8:
                            query = dbRef.startAt(value, key);
                    }
                }
                else
                {
                    switch (kind)
                    {
                        case 0:
                            query = query.endAt(value, key);
                        case 1:
                            query = query.equalTo(value, key);
                        case 2:
                            query = query.limitToFirst(value);
                        case 3:
                            query = query.limitToLast(value);
                        case 4:
                            query = query.orderByChild(value);
                        case 5:
                            query = query.orderByKey();
                        case 6:
                            query = query.orderByPriority();
                        case 7:
                            query = query.orderByValue();
                        case 8:
                            query = query.startAt(value, key);
                    }
                }
            }
            return query; 
        },
        GetMissingDatabaseReferenceError: function ()
        {
            return { code: 404, message: "The underlying object of the DatabaseReference was lost."};
        },
    },

    DatabaseGoOffline_WebGL: function (appNamePtr)
    {
        firebase.database(_GetApp(appNamePtr)).goOffline();
    },
    DatabaseGoOnline_WebGL: function (appNamePtr)
    {
        firebase.database(_GetApp(appNamePtr)).goOnline();
    },
    CreateDatabaseReference_WebGL: function (refID, appNamePtr, pathPtr)
    {
        var path = Pointer_stringify(pathPtr);
        var ref = firebase.database(_GetApp(appNamePtr)).ref(path);
        DatabaseWebGL.AddDatabaseReference({ id: refID, ref: ref });
    },
    DatabaseReferenceFromPath_WebGL: function (appNamePtr, pathPtr)
    {
        var app = _GetApp(appNamePtr);
        var path = Pointer_stringify(pathPtr);
        var ref = firebase.database(app).ref(path);
        return _AllocateString(DatabaseWebGL.GetDatabaseReferenceFullKey(ref));
    },
    DatabaseReferenceFromURL_WebGL: function (appNamePtr, urlPtr)
    {
        var app = _GetApp(appNamePtr);

        var url = Pointer_stringify(urlPtr);
        var ref = firebase.database(app).refFromURL(url);
        return _AllocateString(DatabaseWebGL.GetDatabaseReferenceFullKey(ref));
    },
   
    CancelOnDisconnectForReference_WebGL: function (refID, promiseID, callback)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            _SendVoidPromiseCallback(promiseID, callback, DatabaseWebGL.GetMissingDatabaseReferenceError());
            return;
        }
        ref.onDisconnect().cancel().then(function ()
        {
            _SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            _SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    RemoveValueOnDisconnectForReference_WebGL: function (refID, promiseID, callback)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            _SendVoidPromiseCallback(promiseID, callback, DatabaseWebGL.GetMissingDatabaseReferenceError());
            return;
        }
        ref.onDisconnect().remove().then(function ()
        {
            _SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            _SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    SetValueOnDisconnectForReference_WebGL: function (refID, promiseID, valueJsonPtr, callback)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            _SendVoidPromiseCallback(promiseID, callback, DatabaseWebGL.GetMissingDatabaseReferenceError());
            return;
        }
        var valueJson = Pointer_stringify(valueJsonPtr);
        ref.onDisconnect().set(valueJson).then(function ()
        {
            _SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            _SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    SetValueOnDisconnectWithProirity_WebGL: function (refID, promiseID, valueJsonPtr, priorityPtr, callback)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            _SendVoidPromiseCallback(promiseID, callback, DatabaseWebGL.GetMissingDatabaseReferenceError());
            return;
        }
        var valueJson = Pointer_stringify(valueJsonPtr);
        var priority = Pointer_stringify(priorityPtr);
        ref.onDisconnect().setWithPriority(valueJson, priority).then(function ()
        {
            _SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            _SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    UpdateChildrenOnDisconnectForReference_WebGL: function (refID, promiseID, updatesPtr, callback)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            _SendVoidPromiseCallback(promiseID, callback, DatabaseWebGL.GetMissingDatabaseReferenceError());
            return;
        }
        var updates = Pointer_stringify(updatesPtr);
        ref.onDisconnect().update(updates).then(function ()
        {
            _SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            _SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    GetDatabaseReferenceKey_WebGL: function (refID)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            return null;
        }
        return _AllocateString(ref.key);
    },
    GetDatabaseReferenceParentPath_WebGL: function (refID)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            return null;
        }
        if (ref.parent)
        {
            var parentFullPath = DatabaseWebGL.GetDatabaseReferenceFullKey(ref.parent);
            return _AllocateString(parentFullPath);
        }
        return null;
    },
    GetDatabaseReferenceRootPath_WebGL: function (refID)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            return null;
        }

        return _AllocateString(ref.root.key);
    },
    GetDatabaseReferenceFullPath_WebGL: function (refID)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            return null;
        }
        return _AllocateString(DatabaseWebGL.GetDatabaseReferenceFullKey(ref));
    },
    GetPushReferenceFullPath_WebGL: function (refID)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            return null;
        }
        return _AllocateString(DatabaseWebGL.GetDatabaseReferenceFullKey(ref.push()));
    },
    DatabaseReferenceRemoveValue_WebGL: function (refID, promiseID, callback)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            _SendVoidPromiseCallback(promiseID, callback, DatabaseWebGL.GetMissingDatabaseReferenceError());
            return;
        }
        
        ref.remove().then(function ()
        {
            _SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            _SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    RunTransaction_WebGL: function (refID, promiseID, transcationHandlerID, fireLocalEvents, transactionUpdateCallback, transactionCompleteCallback)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            _SendOneParameterPromiseCallback(promiseID, transactionCompleteCallback, null, DatabaseWebGL.GetMissingDatabaseReferenceError());
            return;
        }

        ref.transaction(
            function (data)
            {
                var mutableData =
                {
                    key = ref.key,
                    value = JSON.stringify(data),
                    priority = data.priority,
                };
                var dataPtr = _AllocateString(JSON.stringify(mutableData));
                Runtime.dynCall('vii', transactionUpdateCallback, [transcationHandlerID, dataPtr]);
                if (!DatabaseWebGL.transactionsResults[transcationHandlerID])
                    return false;
                if (!DatabaseWebGL.transactionsResults[transcationHandlerID].success)
                    return false;
                return DatabaseWebGL.transactionsResults[transcationHandlerID].data;
            },
            function (error, committed, snapshot)
            {
                if (error)
                    _SendOneParameterPromiseCallback(promiseID, transactionCompleteCallback, null, error);
                else
                {
                    var snapshotObj = DatabaseWebGL.CreateSerializableSnapshotObject(refID, snapshot);
                    _SendOneParameterPromiseCallback(promiseID, transactionCompleteCallback, snapshotObj, null);
                }
            }, fireLocalEvents);
    },
    CommunicateTransactionResult_WebGL: function (transactionHandlerID, rawDataJson, success)
    {
        DatabaseWebGL.transactionsResults[transactionHandlerID].data = Pointer_stringify(rawDataJson);
        DatabaseWebGL.transactionsResults[transactionHandlerID].success = success;
    },
    SetDatabaseReferencePriority_WebGL: function (refID, promiseID, priorityPtr, callback)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            _SendVoidPromiseCallback(promiseID, callback, DatabaseWebGL.GetMissingDatabaseReferenceError());
            return;
        }
        ref.setPriority(Pointer_stringify(priorityPtr)).then(function ()
        {
            _SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            _SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    SetDatabaseReferenceValue_WebGL: function (refID, promiseID, valueJsonPtr, callback)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            _SendVoidPromiseCallback(promiseID, callback, DatabaseWebGL.GetMissingDatabaseReferenceError());
            return;
        }
        var valueJson = Pointer_stringify(valueJsonPtr);
        ref.set(valueJson).then(function ()
        {
            _SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            _SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    SetDatabaseReferenceValueWithPriority_WebGL: function (refID, promiseID, valueJsonPtr, priorityPtr, callback)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            _SendVoidPromiseCallback(promiseID, callback, DatabaseWebGL.GetMissingDatabaseReferenceError());
            return;
        }
        var valueJson = Pointer_stringify(valueJsonPtr);
        var priority = Pointer_stringify(priorityPtr);
        ref.setWithPriority(valueJson, priority).then(function ()
        {
            _SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            _SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    GetDatabaseReferenceToString_WebGL: function (refID)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            return null;
        }
        return _AllocateString(ref.toString());
    },
    UpdateDatabaseReferenceChildren_WebGL: function (refID, promiseID, updatePtr, callback)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            _SendVoidPromiseCallback(promiseID, callback, DatabaseWebGL.GetMissingDatabaseReferenceError());
            return;
        }

        var update = Pointer_stringify(updatePtr);
        ref.update(update).then(function ()
        {
            _SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            _SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    GetQueryValue_WebGL: function (refID, promiseID, queryListPtr, callback)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, DatabaseWebGL.GetMissingDatabaseReferenceError());
            return;
        }

        var queries = JSON.parse(Pointer_stringify(queryListPtr));

        var targetObject = null;

        if (!queries || queries.length == 0)
            targetObject = ref;
        else
            targetObject = DatabaseWebGL.CreateQueryOnReferenceFromQueryList(ref, queries);

        targetObject.once('value').then(function (snapshot)
        {
            var snapshotObj = DatabaseWebGL.CreateSerializableSnapshotObject(refID, snapshot);
            _SendOneParameterPromiseCallback(promiseID, callback, snapshotObj, null);
        }).catch(function (error)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    ListenToDatabaseReferenceEventOfType_WebGL: function (refID, listenerID, eventTypePtr, queryListPtr, callback)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            return;
        }

        var queries = JSON.parse(Pointer_stringify(queryListPtr));
        var eventName = Pointer_stringify(eventTypePtr);

        var targetObject = null;
        if (!queries || queries.length == 0)
            targetObject = ref;
        else
            targetObject = DatabaseWebGL.CreateQueryOnReferenceFromQueryList(ref, queries);

        var listener = function (snapshot, prevChildName)
        {
            var snapshotObj = DatabaseWebGL.CreateSerializableSnapshotObject(refID, snapshot);
            var snapshotBytes = _AllocateString(JSON.stringify(snapshotObj));
            var childNameBytes = _AllocateString(prevChildName);
            var eventNameBytes = _AllocateString(eventName);

            Runtime.dynCall('viiiii', callback, [refID, eventNameBytes, snapshotBytes, 0, childNameBytes]);

            if (snapshotBytes != 0)
                _free(snapshotBytes);
            if (childNameBytes != 0)
                _free(childNameBytes);
            if (eventNameBytes != 0)
                _free(eventNameBytes);

        };
        DatabaseWebGL.AddEventListener(listenerID, listener);
        targetObject.on(eventName, listener, function (error)
        {
            var errorBytes = _AllocateString(JSON.stringify(error));
            var eventNameBytes = _AllocateString(eventName);

            Runtime.dynCall('viiiii', callback, [refID, eventNameBytes, 0, errorBytes, 0]);

            if (errorBytes != 0)
                _free(errorBytes);
            if (eventNameBytes != 0)
                _free(eventNameBytes);

        });
    },
    UnlistenToDatabaseReferenceEventOfType_WebGL: function (refID, listenerID, eventTypePtr, queryListPtr)
    {
        var ref = DatabaseWebGL.GetDatabaseReference(refID, true);
        if (!ref)
        {
            return;
        }
        var listener = DatabaseWebGL.GetEventListener(listenerID);
        if (!listener)
        {
            console.log("Listener with id " + listenerID + " doesn't exist, no need to unsubscribe it.");
            return;
        }
        var queries = JSON.parse(Pointer_stringify(queryListPtr));
        var eventName = Pointer_stringify(eventTypePtr);
        var targetObject = null;
        if (!queries || queries.length == 0)
            targetObject = ref;
        else
            targetObject = DatabaseWebGL.CreateQueryOnReferenceFromQueryList(ref, queries);

        targetObject.off(eventName, listener);
    },

};
autoAddDeps(FirebaseDatabaseJavascriptLib, '$DatabaseWebGL');
mergeInto(LibraryManager.library, FirebaseDatabaseJavascriptLib);