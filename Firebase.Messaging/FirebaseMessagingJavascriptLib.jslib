var FirebaseMessagingJavascriptLib =
{
    $MessagingWebGL:
    {
        messagingEventListeners: {},
        TokenRegistrationOnInitEnabledKey: "FirebaseCloudMessagingTokenRegistrationOnInitEnabled",
        AddMessagingEventListener: function (id, listener)
        {
            MessagingWebGL.messagingEventListeners[id] = listener;
        },
        UnsubscribeEventListener: function (id)
        {
            var unsubscribe = MessagingWebGL.messagingEventListeners[id];
            if (unsubscribe)
            {
                unsubscribe();
                delete MessagingWebGL.messagingEventListeners[id];
            }
        },

        SendMessagingEventCallback: function (appName, data, error, callback)
        {
            var dataBytes = _AllocateString(data ? JSON.stringify(data) : null);
            var appNameBytes = _AllocateString(appName);
            var errorBytes = _AllocateString(error ? JSON.stringify(error) : null);
            Runtime.dynCall('viii', callback, [appNameBytes, dataBytes, errorBytes]);
            if (dataBytes != 0)
                _free(dataBytes);
            if (appNameBytes != 0)
                _free(appNameBytes);
            if (errorBytes != 0)
                _free(errorBytes);
        },
    },

    IsMessagingSupported_WebGL: function ()
    {
        return firebase.messaging.isSupported();
    },

    RequestNotificationPermission_WebGL: function (promiseID, callback)
    {
        Notification.requestPermission().then(function (permission)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, permission == 'granted', null);
        }).catch(function (error)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },

    SubscribeToOnMessage_WebGL: function (appNamePtr, listenerID, callback)
    {
        var appName = Pointer_stringify(appNamePtr);
        var messaging = firebase.messaging(_GetApp(appNamePtr));

        var unsubscribe = messaging.onMessage(function (payload)
        {
            MessagingWebGL.SendMessagingEventCallback(appName, payload, null, callback);
        },
            function (error)
            {
                MessagingWebGL.SendMessagingEventCallback(appName, null, error, callback);
            });
        MessagingWebGL.AddMessagingEventListener(listenerID, unsubscribe);
    },
    UnsubscribeToOnMessage_WebGL: function (listenerID)
    {
        MessagingWebGL.UnsubscribeEventListener(listenerID);
    },

    SubscribeToMessagingTokenReceived_WebGL: function (appNamePtr, listenerID, callback)
    {
        var appName = Pointer_stringify(appNamePtr);
        var messaging = firebase.messaging(_GetApp(appNamePtr));
        var unsubscribe = messaging.onTokenRefresh(function (token)
        {
            MessagingWebGL.SendMessagingEventCallback(appName, token, null, callback);
        }, function (error)
        {
                MessagingWebGL.SendMessagingEventCallback(appName, null, error, callback);
        });
        MessagingWebGL.AddMessagingEventListener(listenerID, unsubscribe);
    },
    UnsubscribeToMessagingTokenReceived_WebGL: function (listenerID)
    {
        MessagingWebGL.UnsubscribeEventListener(listenerID);
    },
    DeleteMessagingToken_WebGL: function (promiseID, appNamePtr, tokenPtr, callback)
    {
        var messaging = firebase.messaging(_GetApp(appNamePtr));
        messaging.deleteToken(Pointer_stringify(tokenPtr)).then(function (deleted)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, deleted, null);
        }).catch(function (error)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    GetMessagingToken_WebGL: function (promiseID, appNamePtr, callback)
    {
        var messaging = firebase.messaging(_GetApp(appNamePtr));
        messaging.getToken().then(function (token)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, token, null);
        }).catch(function (error)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    UseVapidKey_WebGL: function (appNamePtr, vapidKeyPtr)
    {
        var messaging = firebase.messaging(_GetApp(appNamePtr));
        messaging.usePublicVapidKey(Pointer_stringify(vapidKeyPtr));
    },
    GetTokenRegistrationOnInitEnabled_WebGL: function ()
    {
        var storage;
        try
        {
            storage = window['localStorage'];
            var enabled = storage.getItem(MessagingWebGL.TokenRegistrationOnInitEnabledKey);
            if (enabled == null || enabled == undefined)
                return true;
            else
                return enabled;
        }
        catch (e)
        {
            var supportsLocalStorage = e instanceof DOMException && (
                // everything except Firefox
                e.code === 22 ||
                // Firefox
                e.code === 1014 ||
                // test name field too, because code might not be present
                // everything except Firefox
                e.name === 'QuotaExceededError' ||
                // Firefox
                e.name === 'NS_ERROR_DOM_QUOTA_REACHED') &&
                // acknowledge QuotaExceededError only if there's something already stored
                (storage && storage.length !== 0);
            if (!supportsLocalStorage)
                console.log("This browser doesn't support local storage, we can't save the value of TokenRegistrationOnInitEnabled for cloud messaging, it will always be true");
            return true;
        }
    },
    SetTokenRegistrationOnInitEnabled_WebGL: function (newValue)
    {
        try
        {
            window.localStorage
            storage = window['localStorage'];
            storage.setItem(MessagingWebGL.TokenRegistrationOnInitEnabledKey, newValue);
        }
        catch (e)
        {
            var supportsLocalStorage = e instanceof DOMException && (
                // everything except Firefox
                e.code === 22 ||
                // Firefox
                e.code === 1014 ||
                // test name field too, because code might not be present
                // everything except Firefox
                e.name === 'QuotaExceededError' ||
                // Firefox
                e.name === 'NS_ERROR_DOM_QUOTA_REACHED') &&
                // acknowledge QuotaExceededError only if there's something already stored
                (storage && storage.length !== 0);
            if (!supportsLocalStorage)
                console.log("This browser doesn't support local storage, we can't save the value of TokenRegistrationOnInitEnabled for cloud messaging, it will always be true");
            return;
        }
    },
    SubscribeToTopic_WebGL: function (promiseID, topicNamePtr, sKeyPtr, userTokenPtr, callback)
    {
        var http = new XMLHttpRequest();
        var userToken = "";
        userToken = Pointer_stringify(userTokenPtr);
        var topicName = "";
        topicName = Pointer_stringify(topicNamePtr);
        http.setRequestHeader("key", Pointer_stringify(sKeyPtr));
        http.setRequestHeader("Accept", "application/json");
        http.open("POST", "https://iid.googleapis.com/iid/v1/" + userToken + "/rel/topics/" + topicName);
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
                _SendVoidPromiseCallback(promiseID, callback, error.error);
            }
            else
            {
                _SendVoidPromiseCallback(promiseID, callback, null);
            }
        };
        http.onerror = function http_onerror(error)
        {
            console.log("Unaccounted error at: " + error.error);
            _SendVoidPromiseCallback(promiseID, callback, error.error);
        };
        http.ontimeout = function http_ontimeout(e)
        {
            var error =
            {
                code: 408,
                message = "Connection timed out.",
            };
            _SendVoidPromiseCallback(promiseID, callback, error);
        };
        http.onabort = function http_onAbort(e)
        {
            var error =
            {
                code: 0,
                message: "Request aborted.",
            };
            _SendVoidPromiseCallback(promiseID, callback, error);
        };
        http.send();
    },
    Test: function ()
    {
        var messaging = firebase.messaging(_GetApp(appNamePtr));
        messaging.useServiceWorker
    }
};
autoAddDeps(FirebaseMessagingJavascriptLib, '$MessagingWebGL');
mergeInto(LibraryManager.library, FirebaseMessagingJavascriptLib);