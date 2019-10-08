var FirebaseStorageJavascriptLib =
{
    $StorageWebGL:
    {
        sReferences: {},
        DownloadTasks: {},
        UploadTasks: {},
        maxDownloadRetryTimes: {},
        SetMaxDownloadRetryTime: function (appName, maxTime)
        {
            StorageWebGL.maxDownloadRetryTimes[appName] = maxTime;
        },
        GetMaxDownloadRetryTime: function (appName)
        {
            if (!StorageWebGL.maxDownloadRetryTimes[appName])
                return 1000;
            return StorageWebGL.maxDownloadRetryTimes[appName];
        },
        AddStorageReference: function (id, ref)
        {
            StorageWebGL.sReferences[id] = ref;
        },
        RemoveStorageReference: function (id)
        {
            delete StorageWebGL.sReferences[id];
        },
        GetStorageReference: function (id, logIfNotFound)
        {
            var ref = StorageWebGL.sReferences[id];
            if (logIfNotFound && !ref)
            {
                console.log("The underlying object of the StorageReference was lost.");
            }
            return ref;
        },
        AddUploadTask: function (taskID, task)
        {
            StorageWebGL.UploadTasks[taskID] = task;
        },
        RemoveUploadTask: function (taskID)
        {
            delete StorageWebGL.UploadTasks[taskID];
        },
        GetUploadTask: function (taskID)
        {
            return StorageWebGL.UploadTasks[taskID];
        },
        AddDownloadTask: function (taskID, http)
        {
            StorageWebGL.DownloadTasks[taskID] = http;
        },
        RemoveDownloadTask: function (taskID)
        {
            delete StorageWebGL.DownloadTasks[taskID];
        },
        GetDownloadTask: function (taskID)
        {
            return StorageWebGL.DownloadTasks[taskID];
        },
        ListResultToSerializableOBject: function (refID, ref, listResult)
        {
            let items = listResult.items.map(o => o.fullPath);
            let prefixes = listResult.prefixes.map(o => o.fullPath);
            var finalListResult =
            {
                items: items,
                prefixes: prefixes,
                refID: refID,
                appName: ref.storage.app.name,
                storageBucket: ref.storage.bucket,
                nextPageToken: listResult.nextPageToken,
            };
            return finalListResult;
        },
        GetMissingReferenceError: function (refID)
        {
            return { code: 404, message: "The underlying object of the StorageReference was lost." };
        },
    },
    SetMaxOperationRetryTime_WebGL: function (appNamePtr, maxTime)
    {
        firebase.storage(_GetApp(appNamePtr)).setMaxOperationRetryTime(maxTime);
    },
    GetMaxOperationRetryTime_WebGL: function (appNamePtr)
    {
        return firebase.storage(_GetApp(appNamePtr)).maxOperationRetryTime;
    },
    SetMaxUploadRetryTime_WebGL: function (appNamePtr, maxTime)
    {
        firebase.storage(_GetApp(appNamePtr)).setMaxUploadRetryTime(maxTime);
    },
    GetMaxUploadRetryTime_WebGL: function (appNamePtr)
    {
        return firebase.storage(_GetApp(appNamePtr)).maxUploadRetryTime;
    },
    SetMaxDownloadRetryTime_WebGL: function (appNamePtr, maxTime)
    {
        StorageWebGL.SetMaxDownloadRetryTime(Pointer_stringify(appNamePtr), maxTime);
    },
    GetMaxDownloadRetryTime_WebGL: function (appNamePtr)
    {
        return StorageWebGL.GetMaxDownloadRetryTime(Pointer_stringify(appNamePtr));
    },
    StorageReferenceFromURL_WebGL: function (appNamePtr, urlPtr)
    {
        var url = Pointer_stringify(urlPtr);
        var ref = firebase.storage(_GetApp(appNamePtr)).refFromURL(url);
        return _AllocateString(ref.fullPath);
    },
    StorageReferenceFromPath_WebGL: function (appNamePtr, pathPtr)
    {
        var path = Pointer_stringify(pathPtr);
        var ref = firebase.storage(_GetApp(appNamePtr)).ref(path);
        return _AllocateString(ref.fullPath);
    },
    CreateStorageReference_WebGL: function (refID, appNamePtr, pathPtr)
    {
        var app = _GetApp(appNamePtr);
        var path = Pointer_stringify(pathPtr);
        var ref = firebase.storage(app).ref(path);
        StorageWebGL.AddStorageReference(refID, ref);
    },
    ReleaseStorageReferense_WebGL: function (refID)
    {
        StorageWebGL.RemoveStorageReference(refID);
    },
    GetStorageReferenceParentPath_WebGL: function (refID)
    {
        var ref = StorageWebGL.GetStorageReference(refID, true);
        if (!ref)
        {
            return null;
        }
        var parent = ref.parent;
        return _AllocateString(parent ? parent.fullPath : null);
    },
    GetStorageReferenceBucketName_WebGL: function (refID)
    {
        var ref = StorageWebGL.GetStorageReference(refID, true);
        if (!ref)
        {
            return null;
        }

        return _AllocateString(ref.bucket);
    },
    GetStorageReferenceFullPath_WebGL: function (refID)
    {
        var ref = StorageWebGL.GetStorageReference(refID, true);
        if (!ref)
        {
            return null;
        }
        return _AllocateString(ref.fullPath);
    },
    GetStorageReferenceFileName_WebGL: function (refID)
    {
        var ref = StorageWebGL.GetStorageReference(refID, true);
        if (!ref)
        {
            return null;
        }
        return _AllocateString(ref.name);
    },
    StorageReferenceToString_WebGL: function (refID)
    {
        var ref = StorageWebGL.GetStorageReference(refID, true);
        if (!ref)
        {
            return null;
        }
        return _AllocateString(ref.toString());
    },
    DeleteStorageReferenceFile_WebGL: function (refID, promiseID, callback)
    {
        var ref = StorageWebGL.GetStorageReference(refID, true);
        if (!ref)
        {
            _SendVoidPromiseCallback(promiseID, callback, StorageWebGL.GetMissingReferenceError(refID));
            return;
        }
        ref.delete().then(function ()
        {
            _SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            _SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    GetStorageReferenceDownloadUrl_WebGL: function (refID, promiseID, callback)
    {
        var ref = StorageWebGL.GetStorageReference(refID, true);
        if (!ref)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, StorageWebGL.GetMissingReferenceError(refID));
            return;
        }
        ref.getDownloadURL().then(function (url)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, url, null);
        }).catch(function (error)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    GetStorageReferenceMetadata_WebGL: function (refID, promiseID, callback)
    {
        var ref = StorageWebGL.GetStorageReference(refID, true);
        if (!ref)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, StorageWebGL.GetMissingReferenceError(refID));
            return;
        }
        ref.getMetadata().then(function (metadata)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, metadata, null);
        }).catch(function (error)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    GetStorageReferenseListPartial_WebGL: function (refID, promiseID, optionsJsonPtr, callback)
    {
        var ref = StorageWebGL.GetStorageReference(refID, true);
        if (!ref)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, StorageWebGL.GetMissingReferenceError(refID));
            return;
        }
        var optionsJson = Pointer_stringify(optionsJsonPtr);
        var options = JSON.parse(optionsJson);
        ref.list(options).then(function (listResult)
        {
            var finalListResult = StorageWebGL.ListResultToSerializableOBject(refID, ref, listResult);
            _SendOneParameterPromiseCallback(promiseID, callback, finalListResult, null);

        }).catch(function (error)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    GetStorageReferenseListAll_WebGL: function (refID, promiseID, callback)
    {
        var ref = StorageWebGL.GetStorageReference(refID, true);
        if (!ref)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, StorageWebGL.GetMissingReferenceError(refID));
            return;
        }
        ref.listAll().then(function (listResult)
        {
            var finalListResult = StorageWebGL.ListResultToSerializableOBject(refID, ref, listResult);
            _SendOneParameterPromiseCallback(promiseID, callback, finalListResult, null);

        }).catch(function (error)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    UpdateStorageReferenceMetadata_WebGL: function (refID, promiseID, metadataPtr, callback)
    {
        var ref = StorageWebGL.GetStorageReference(refID, true);
        if (!ref)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, StorageWebGL.GetMissingReferenceError(refID));
            return;
        }
        var metadata = JSON.parse(Pointer_stringify(metadataPtr));
        ref.updateMetadata(metadata).then(function (newMetadata)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, newMetadata, error);
        }).catch(function (error)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    StorageReferenceUploadBytes_WebGL: function (refID, taskID, promiseID, data, dataLength, customMetadataPtr, promiseCallback, progressCallback)
    {
        var ref = StorageWebGL.GetStorageReference(refID, true);
        if (!ref)
        {
            _SendOneParameterPromiseCallback(promiseID, promiseCallback, null, StorageWebGL.GetMissingReferenceError(refID));
            return;
        }

        var customMetadata = JSON.parse(Pointer_stringify(customMetadataPtr));
        var bytes = new Uint8Array(dataLength);
        for (var i = 0; i < dataLength; i++)
        {
            bytes[i] = HEAPU8[data + i];
        }
        var task = ref.put(bytes, customMetadata);
        StorageWebGL.AddUploadTask(taskID, task);
        task.then(function (uploadSnapshot)
        {
            _SendOneParameterPromiseCallback(promiseID, promiseCallback, uploadSnapshot.metadata, null);
        }, function (error)
        {
            _SendOneParameterPromiseCallback(promiseID, promiseCallback, null, error);
        });
        if (taskID > 0)
        {
            task.on(firebase.storage.TaskEvent.STATE_CHANGED, function (snapshot)
            {
                _SendOneParameterPromiseCallback(taskID, progressCallback, snapshot, null);
            });
        }
    },
    StorageReferenceUploadString_WebGL: function (refID, taskID, promiseID, dataPtr, stringFormatPtr, customMetadataPtr, promiseCallback, progressCallback)
    {
        var ref = StorageWebGL.GetStorageReference(refID, true);
        if (!ref)
        {
            _SendOneParameterPromiseCallback(promiseID, promiseCallback, null, StorageWebGL.GetMissingReferenceError(refID));
            return;
        }
        var stringToUpload = Pointer_stringify(dataPtr);
        var stringFormat = Pointer_stringify(stringFormatPtr);
        var customMetadata = JSON.parse(Pointer_stringify(customMetadataPtr));
        var task = ref.putString(stringToUpload, stringFormat, customMetadata);
        StorageWebGL.AddUploadTask(taskID, task);
        task.then(function (uploadSnapshot)
        {
            _SendOneParameterPromiseCallback(promiseID, promiseCallback, uploadSnapshot.metadata, null);
        }, function (error)
        {
                _SendOneParameterPromiseCallback(promiseID, promiseCallback, null, error);
        });
        if (taskID > 0)
        {
            task.on(firebase.storage.TaskEvent.STATE_CHANGED, function (snapshot)
            {
                _SendOneParameterPromiseCallback(taskID, progressCallback, snapshot, null);
            });
        }
    },
    StorageReferenceDownloadBytes_WebGL: function (refID, taskID, promiseID, promiseCallback, progressCallback)
    {
        var ref = StorageWebGL.GetStorageReference(refID, true);
        if (!ref)
        {
            _SendByteArrayPromiseCallback(promiseID, promiseCallback, null, StorageWebGL.GetMissingReferenceError(refID));
            return;
        }

        var appName = ref.storage.app.name;

        ref.getDownloadURL().then(function (url)
        {
            var http = new XMLHttpRequest();
            http.open("Get", url, true);
            http.responseType = 'arraybuffer';
            http.timeout = StorageWebGL.maxDownloadRetryTimes[appName];
            StorageWebGL.AddDownloadTask(taskID, http);

            http.onload = function http_onload(e)
            {
                var response = 0;
                if (!!http.response)
                    response = http.response;
                var byteArray = new Uint8Array(response);
                if (http.status != 200)
                {
                    var errorJson = _ByteArrayToString(byteArray);
                    var jsonObj = JSON.parse(errorJson);
                    var error = jsonObj.error;
                    _SendByteArrayPromiseCallback(promiseID, promiseCallback, null, error);
                }
                else
                {
                    _SendByteArrayPromiseCallback(promiseID, promiseCallback, byteArray, null);
                }
            };
            http.onerror = function http_onerror(error)
            {
                console.log("Unaccounted error at: " + error.error);
                _SendByteArrayPromiseCallback(promiseID, promiseCallback, null, error.error);
            };
            http.ontimeout = function http_ontimeout(e)
            {
                var error =
                {
                    code: 408,
                    message = "Connection timed out.",
                };
                _SendByteArrayPromiseCallback(promiseID, promiseCallback, null, error);
            };
            http.onabort = function http_onAbort(e)
            {
                var error =
                {
                    code: 0,
                    message: "Download aborted.",
                };
                _SendByteArrayPromiseCallback(promiseID, promiseCallback, null, error);
            };
            http.onprogress = function http_onprogress(e)
            {
                if (e.lengthComputable)
                {
                    var snapshot =
                    {
                        bytesTransferred: e.loaded,
                        totalBytes: e.total,
                    };
                    _SendOneParameterPromiseCallback(taskID, progressCallback, snapshot, null);
                }
            };
            http.send();
        }).catch(function (error)
        {
            _SendByteArrayPromiseCallback(promiseID, promiseCallback, null, error);
        });
    },
    CancelTransferTask_WebGL: function (taskID)
    {
        var uploadTask = StorageWebGL.GetUploadTask(taskID);
        if (uploadTask)
        {
            return uploadTask.cancel();
        }
        var downloadTask = StorageWebGL.GetDownloadTask(taskID);
        if (downloadTask)
        {
            downloadTask.abort();
            return true;
        }
        return false;
    },
};
autoAddDeps(FirebaseStorageJavascriptLib, '$StorageWebGL');
mergeInto(LibraryManager.library, FirebaseStorageJavascriptLib);