var FirebaseRemoteConfigJavascriptLib =
{
    $RemoteConfigWebGL:
    {

    },
    GetFetchTimeoutMillis_WebGL: function (appNamePtr)
    {
        return firebase.remoteConfig(_GetApp(appNamePtr)).settings.fetchTimeoutMillis;
    },
    SetConfigFetchTimeoutMillis_WebGL: function (appNamePtr, milliseconds)
    {
        var app = _GetApp(appNamePtr);
        firebase.remoteConfig(app).settings.fetchTimeoutMillis = milliseconds;
    },

    GetMinimumFetchInterval_WebGL: function (appNamePtr)
    {
        return firebase.remoteConfig(_GetApp(appNamePtr)).settings.minimumFetchIntervalMillis;
    },
    SetConfigFetchIntervalMillis_WebGL: function (appNamePtr, milliseconds)
    {
        var app = _GetApp(appNamePtr);
        firebase.remoteConfig(app).settings.minimumFetchIntervalMillis = milliseconds;
    },

    GetRemoteConfigFetchTime_WebGL: function (appNamePtr)
    {
        var app = _GetApp(appNamePtr);
        return firebase.remoteConfig(app).fetchTimeMillis;
    },
    GetLastFetchStatus_WebGL: function (appNamePtr)
    {
        var app = _GetApp(appNamePtr);
        return _AllocateString(firebase.remoteConfig(app).lastFetchStatus);
    },

    RemoteConfigActivateFetched_WebGL: function (promiseID, appNamePtr, callback)
    {
        firebase.remoteConfig(_GetApp(appNamePtr)).activate().then(function (activated)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, activated, null);
        }).catch(function (error)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    EnsureConfigInitialized_WebGL: function (promiseID, appNamePtr, callback)
    {
        firebase.remoteConfig(_GetApp(appNamePtr)).ensureInitialized().then(function ()
        {
            _SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            _SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    RemoteConfigFetch_WebGL: function (promiseID, appNamePtr, callback)
    {
        firebase.remoteConfig(_GetApp(appNamePtr)).fetch().then(function ()
        {
            _SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            _SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    RemoteConfigFetchAndActivate_WebGL: function (promiseID, appNamePtr, callback)
    {
        firebase.remoteConfig(_GetApp(appNamePtr)).fetchAndActivate().then(function (activated)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, activated, null);
        }).catch(function (error)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    GetKeysList_WebGL: function (appNamePtr)
    {
        var map = firebase.remoteConfig(_GetApp(appNamePtr)).getAll();
        var keys = Object.keys(map);
        return _AllocateString(JSON.stringify(keys));
    },
    GetDefaultConfig_WebGL: function (appNamePtr)
    {
        return _AllocateString(JSON.stringify(firebase.remoteConfig(_GetApp(appNamePtr)).defaultConfig));
    },
    GetAllConfig_WebGL: function (appNamePtr)
    {
        var all = firebase.remoteConfig(_GetApp(appNamePtr)).getAll();
        return _AllocateString(JSON.stringify(all));
    },
    GetRemotConfigValue_WebGL: function (appNamePtr, keyPtr)
    {
        var all = firebase.remoteConfig(_GetApp(appNamePtr)).getAll();
        return _AllocateString(JSON.stringify(all[Pointer_stringify(keyPtr)]));
    },
    SetRemoteConfigDefaults_WebGL: function (appNamePtr, defaultsPtr)
    {
        var app = _GetApp(appNamePtr);
        var defaults = JSON.parse(Pointer_stringify(defaultsPtr));
        firebase.remoteConfig(app).defaultConfig = defaults;
    },
};
autoAddDeps(FirebaseRemoteConfigJavascriptLib, '$RemoteConfigWebGL');
mergeInto(LibraryManager.library, FirebaseRemoteConfigJavascriptLib);