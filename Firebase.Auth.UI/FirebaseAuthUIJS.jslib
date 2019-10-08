var FirebaseAuthUIJS =
{
    $AuthUIWebGL:
    {
        authUIs: {},
        AddAuthUI: function (id, data)
        {
            this.authUIs[id] = data;
        },
        RemoveAuthUI: function (id)
        {
            delete this.authUIs[id];
        },
        GetAuthUI: function (id, logIfNotFound)
        {
            var data = this.authUIs[id];
            if (logIfNotFound && (!data || !data.ui))
            {
                console.log('The underlying object of the AuthUI was lost.');
            }
            return this.authUIs[id];
        },

        AllocateString: function (str) {
            if (str) {
                var length = lengthBytesUTF8(str) + 1;
                var buff = _malloc(length);

                stringToUTF8Array(str, HEAPU8, buff, length);

                return buff;
            }
            return 0;
        },

        SendOneParameterPromiseCallback: function (promiseID, callback, rawData, error) {
            var dataBytes = this.AllocateString(rawData ? JSON.stringify(rawData) : null);
            var errorBytes = this.AllocateString(error ? JSON.stringify(error) : null);

            Runtime.dynCall('viii', callback, [promiseID, dataBytes, errorBytes]);

            if (dataBytes != 0)
                _free(dataBytes);
            if (errorBytes != 0)
                _free(errorBytes);
        },
        SendVoidPromiseCallback: function (promiseID, callback, error) {
            var errorBytes = this.AllocateString(error ? JSON.stringify(error) : null);
            Runtime.dynCall('vii', callback, [promiseID, errorBytes]);
            if (errorBytes != 0)
                _free(errorBytes);
        },
    },
    GetAuthUIIsPendingRedirect_WebGL: function (id)
    {
        var ui = AuthUIWebGL.GetAuthUI(id, true);
        if (!ui)
        {
            return false;
        }
        return ui.ui.isPendingRedirect();
    },
    CreateNewAuthUI_WebGL: function (id, appNamePtr, visibilityCallback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        var data = {
            ui: new firebaseui.auth.AuthUI(app.auth()),
            visibilityCallback: visibilityCallback,
        };
        AuthUIWebGL.AddAuthUI(id, data);
    },
    DisableAuthUIAutoSignIn_WebGL: function (id)
    {
        var ui = AuthUIWebGL.GetAuthUI(id, true);
        if (!ui)
        {
            return;
        }
        ui.ui.disableAutoSignIn();
    },
    StartAuthUI_WebGL: function (uiID, promiseID, configJsonPtr, callback)
    {
        var ui = AuthUIWebGL.GetAuthUI(id, true);
        if (!ui)
        {
            console.log('Trying to start a non existent auth ui with id ' + uiID + ' the promise will reject immediately.');
            var error =
            {
                code: 404,
                message: "Failed to show Auth UI with id " + uiID + " doesn't exist.",
            };
            AuthUIWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
            return;
        }

        var configJson = Pointer_stringify(configJsonPtr);
        var config = JSON.parse(configJson);

        var modalID = "authModal";
        var modalDiv = document.getElementById(modalID);
        if (!modalDiv)
        {
            modalDiv = document.createElement('div');
            modalDiv.setAttribute('id', modalID);
            document.body.appendChild(modalDiv);
            modalDiv.style.position = 'fixed';
            modalDiv.style.zIndex = "1";
            modalDiv.style.left = "0";
            modalDiv.style.top = "0";
            modalDiv.style.width = "100%";
            modalDiv.style.height = "100%";
            modalDiv.style.overflow = "auto";
            modalDiv.style.backgroundColor = "rgb(0,0,0)";
            modalDiv.style.backgroundColor = "rgba(0,0,0,0.4)";
            modalDiv.style.display = "block";
        }
        else
        {
            modalDiv.style.display = "block";
        }
        var authContainerID = "firebaseui-auth-container";
        var authContainer = document.getElementById(authContainerID);
        if (!authContainer)
        {
            authContainer = document.createElement('div');
            authContainer.setAttribute('id', authContainerID);
            modalDiv.appendChild(authContainer);
            authContainer.style.position = "absolute";
            authContainer.style.left = "calc(50vw - 200px)";
            authContainer.style.top = "calc(50vh - 200px)";
            authContainer.style.backgroundColor = "rgb(255,255,255)";
        }
        config.callbacks =
            {
                signInSuccessWithAuthResult: function (userCredential, redirectUrl)
                {
                    if (modalDiv)
                        modalDiv.style.display = "none";
                    AuthUIWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredential), null);
                    return redirectUrl && redirectUrl != 0 && redirectUrl.length > 0;
                },
                signInFailure: function (error)
                {
                    if (modalDiv)
                        modalDiv.style.display = "none";

                    AuthUIWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
                },
                uiShown: function ()
                {
                    var visibilityCallback = ui.visibilityCallback;
                    if (visibilityCallback)
                        Runtime.dynCall('vii', visibilityCallback, [uiID, true]);
                },
            };
        ui.ui.start("#" + authContainerID, config);
    },
    AuthUISignIn_WebGL: function (id)
    {
        var ui = AuthUIWebGL.GetAuthUI(id, true);
        if (!ui)
        {
            return;
        }
        ui.ui.signIn();
    },

    ResetAuthUI_WebGL: function (id)
    {
        var ui = AuthUIWebGL.GetAuthUI(id, true);
        if (!ui)
        {
            return;
        }
        ui.ui.reset();
    },
    DeleteAuthUI_WebGL: function (promiseID, uiID, callback)
    {
        var ui = AuthUIWebGL.GetAuthUI(id, true);
        if (!ui)
        {
            var error =
            {
                code: 404,
                message: 'The underlying object of the AuthUI was lost.',
            };
            _SendVoidPromiseCallback(promiseID, callback, error);
            return;
        }

        ui.ui.delete().then(function ()
        {
            AuthUIWebGL.SendVoidPromiseCallback(promiseID, callback, null);
            var visibilityCallback = ui.visibilityCallback;
            if (visibilityCallback)
                Runtime.dynCall('vii', visibilityCallback, [uiID, false]);
        }).catch(function (error)
        {
            AuthUIWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    GetCredentialHelperString_WebGL: function(enumIndex)
    {
        switch (enumIndex)
        {
            case 0:
                return AuthUIWebGL.AllocateString(firebaseui.auth.CredentialHelper.NONE);
            case 1:
                return AuthUIWebGL.AllocateString(firebaseui.auth.CredentialHelper.GOOGLE_YOLO);
            default:
                return AuthUIWebGL.AllocateString(firebaseui.auth.CredentialHelper.ACCOUNT_CHOOSER_COM);
        }
    },
    ReleaseAuthUI_WebGL: function (id)
    {
        AuthWebGL.RemoveAuthUI(id);
    },
    GetAnonymousAuthProviderID_WebGL: function ()
    {
        return AuthUIWebGL.AllocateString(firebaseui.auth.AnonymousAuthProvider.PROVIDER_ID);
    },

};
autoAddDeps(FirebaseAuthUIJS, '$AuthUIWebGL');
mergeInto(LibraryManager.library, FirebaseAuthUIJS);