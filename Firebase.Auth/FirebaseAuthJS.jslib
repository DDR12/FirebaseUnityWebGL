var FirebaseAuthJS =
{
    $AuthWebGL:
    {
        authChangeListeners: {},
        usersInstances: {},
        usersID: 0,
        AddUserFromCredentials: function (userCredentials)
        {
            var id = usersID++;
            AuthWebGL.usersInstances[id] = userCredentials.user;
            var wrapper = { nativeLibID: id };
            userCredentials.User = wrapper;
            return userCredentials;
        },
        AddUser: function (user) // Caches a native user reference and returns a wrapper.
        {
            var id = usersID++;
            AuthWebGL.usersInstances[id] = user;
            var userRef = { nativeLibID: id };
            return userRef;
        },
        RemoveUser: function (id)
        {
            delete AuthWebGL.usersInstances[id];
        },
        GetUser: function (id, logIfNotFound)
        {
            var user = AuthWebGL.usersInstances[id];
            if (logIfNotFound && !user)
            {
                console.log("The underlying object of the FirebaseUser was lost.");
            }
            return user;
        },
        UnSubscribeAuthListener: function (listenerID)
        {
            var unsubscribe = AuthWebGL.authChangeListeners[listenerID];
            if (unsubscribe)
            {
                unsubscribe();
                delete AuthWebGL.authChangeListeners[listenerID];
            }
        },

        GetOrCreateAuthProviderRecaptcha: function (parametersJson)
        {
            var parameters = JSON.parse(parametersJson);

            var elementName = 'recaptcha-container';
            var div = document.getElementById(elementName);
            if (div == null || div == undefined)
            {
                div = document.createElement('div');
                div.setAttribute('id', elementName);
                document.body.appendChild(div);
            }
            return new firebase.auth.RecaptchaVerifier(elementName, parameters);
        },
        SendAuthEventCallback: function (appName, data, error, callback)
        {
            var dataBytes = this.AllocateString(data ? JSON.stringify(data) : null);
            var appNameBytes = this.AllocateString(appName);
            var errorBytes = this.AllocateString(error ? JSON.stringify(error) : null);
            Runtime.dynCall('viii', callback, [appNameBytes, dataBytes, errorBytes]);
            if (dataBytes != 0)
                _free(dataBytes);
            if (appNameBytes != 0)
                _free(appNameBytes);
            if (errorBytes != 0)
                _free(errorBytes);
        },

        GetAuthProviderObjectFromJsonValue: function (providerJson)
        {
            var providerObject = JSON.parse(providerJson);
            /*
             * {
             * providerId: string,
             * scopes: string[],
             * customParameters: Dictionary<string,string>,
             * }
             */
            var authProvider = null;
            switch (providerObject.providerId)
            {
                case firebase.auth.PhoneAuthProvider.PROVIDER_ID:
                    authProvider = new firebase.auth.PhoneAuthProvider();
                    break;
                case firebase.auth.EmailAuthProvider.PROVIDER_ID:
                    authProvider = new firebase.auth.EmailAuthProvider();
                    break;
                case firebase.auth.GithubAuthProvider.PROVIDER_ID:
                    authProvider = new firebase.auth.GithubAuthProvider();
                    break;
                case firebase.auth.GoogleAuthProvider.PROVIDER_ID:
                    authProvider = new firebase.auth.GoogleAuthProvider();
                    break;
                case firebase.auth.TwitterAuthProvider.PROVIDER_ID:
                    authProvider = new firebase.auth.TwitterAuthProvider();
                    break;
                case firebase.auth.FacebookAuthProvider.PROVIDER_ID:
                    authProvider = new firebase.auth.FacebookAuthProvider();
                    break;
                default:
                    authProvider = new firebase.auth.OAuthCredential(providerObject.providerId);
            }
            // Email and phone providers have no scopes or custom paremeters.
            // twitter provider can only use custom parameters.
            switch (providerObject.providerId)
            {
                case firebase.auth.PhoneAuthProvider.PROVIDER_ID:
                case firebase.auth.EmailAuthProvider.PROVIDER_ID:
                    break;
                case firebase.auth.TwitterAuthProvider.PROVIDER_ID:
                    if (providerObject.customParameters)
                        authProvider.setCustomParameters(providerObject.customParameters);
                    break;
                default:
                    if (providerObject.customParameters)
                        authProvider.setCustomParameters(providerObject.customParameters);
                    if (providerObject.scopes)
                    {
                        for (i in scopes)
                        {
                            authProvider.addScope(scopes[i]);
                        }
                    }
            }
            return authProvider;
        },
        GetActionCodeSettingsFromJson: function (jsonPtr)
        {
            var actionCodeSettingsJson = Pointer_stringify(jsonPtr);
            var plainActionCodeObject = JSON.parse(actionCodeSettingsJson);
            var actionCodeSettings = Object.assign(new firebase.auth.ActionCodeSettings(), plainActionCodeObject);
            return actionCodeSettings;
        },
        GetMissingUserError: function (userID)
        {
            var error =
            {
                code: 404,
                message: "The underlying reference for the FirebaseUser was lost.",
            };
            return error;
        },

        AllocateString: function (str)
        {
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
        SendByteArrayPromiseCallback: function (promiseID, callback, byteArray, error) {
            var errorBytes = this.AllocateString(error ? JSON.stringify(error) : null);
            var bytes = byteArray ? byteArray : 0;
            var length = bytes ? bytes.length : 0;

            var buffer = _malloc(length);
            HEAPU8.set(bytes, buffer);

            Runtime.dynCall('viiii', callback, [promiseID, buffer, length, errorBytes]);

            _free(buffer);

            if (errorBytes != 0)
                _free(errorBytes);
        },

        ByteArrayToString: function (data) {
            var count = data.length;
            var result = "";
            for (var i = 0; i < count; i++) {
                result += String.fromCharCode(data[i]);
            }
            return result;
        },
    },
    DeleteUser_WebGL: function (promiseID, userID, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, AuthWebGL.GetMissingUserError(userID));
            return;
        }
        user.delete().then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    GetIdToken_WebGL: function (promiseID, userID, forceRefresh, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, AuthWebGL.GetMissingUserError(userID));
            return;
        }
        user.getIdToken(forceRefresh).then(function (idToken)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, idToken, null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    GetIdTokenResult_WebGL: function (promiseID, userID, forceRefresh, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, AuthWebGL.GetMissingUserError(userID));
            return;
        }
        user.getIdTokenResult(forceRefresh).then(function (tokenResult)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, tokenResult, null);

        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    LinkWithCredential_WebGL: function (promiseID, userID, credentialJsonPtr, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, AuthWebGL.GetMissingUserError(userID));
            return;
        }

        var credentialJson = Pointer_stringify(credentialJsonPtr);
        user.linkWithCredential(firebase.auth.AuthCredential.fromJSON(credentialJson)).then(function (userCredentials)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), error);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    LinkWithPhoneNumber_WebGL: function (promiseID, userID, phoneNumberPtr, recaptchaParametersJsonPtr, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, AuthWebGL.GetMissingUserError(userID));
            return;
        }

        var recaptchaVerifier = AuthWebGL.GetOrCreateAuthProviderRecaptcha(Pointer_stringify(recaptchaParametersJsonPtr));

        user.linkWithPhoneNumber(Pointer_stringify(phoneNumberPtr), recaptchaVerifier).then(function (confirmationResult)
        {
            var verificationCode = window.prompt('Please enter the verification code that was sent to your mobile device.');
            confirmationResult.confirm(verificationCode).then(function (userCredentials)
            {
                AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), null);
            }).then(function (error)
            {
                AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
            });
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        }).finally(function ()
        {
            if (recaptchaVerifier)
                recaptchaVerifier.clear();
        });
    },
    LinkWithPopup_WebGL: function (promiseID, providerJsonPtr, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, AuthWebGL.GetMissingUserError(userID));
            return;
        }

        var authProvider = AuthWebGL.GetAuthProviderObjectFromJsonValue(Pointer_stringify(providerJsonPtr));
        user.linkWithPopup(authProvider).then(function (userCredentials)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });

    },
    UpdateProfile_WebGL: function (promiseID, userID, updatesJsonPtr, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, AuthWebGL.GetMissingUserError(userID));
            return;
        }
        var updates = JSON.parse(Pointer_stringify(updatesJsonPtr));
        user.updateProfile(updates).then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    LinkUserWithRedirect_WebGL: function (promiseID, userID, providerJsonPtr, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, AuthWebGL.GetMissingUserError(userID));
            return;
        }

        var authProvider = AuthWebGL.GetAuthProviderObjectFromJsonValue(Pointer_stringify(providerJsonPtr));
        user.linkWithRedirect(authProvider).then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    ReauthenticateWithCredential_WebGL: function (promiseID, userID, credentialJsonPtr, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, AuthWebGL.GetMissingUserError(userID));
            return;
        }
        var credential = firebase.auth.AuthCredential.fromJSON(Pointer_stringify(credentialJsonPtr));
        user.reauthenticateWithCredential(credential).then(function (userCredentials)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    ReauthenticateWithPhoneNumber_WebGL: function (promiseID, userID, phoneNumberPtr, recaptchaParametersJsonPtr, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, AuthWebGL.GetMissingUserError(userID));
            return;
        }
        var recaptchaVerifier = AuthWebGL.GetOrCreateAuthProviderRecaptcha(Pointer_stringify(recaptchaParametersJsonPtr));
        user.reauthenticateWithPhoneNumber(Pointer_stringify(phoneNumberPtr), recaptchaVerifier).then(function (confirmationResult)
        {
            var verificationCode = window.prompt('Please enter the verification code that was sent to your mobile device.');
            confirmationResult.confirm(verificationCode).then(function (userCredentials)
            {
                AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), error);
            }).then(function (error)
            {
                AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
            });
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        }).finally(function ()
        {
            if (recaptchaVerifier)
                recaptchaVerifier.clear();
        });
    },
    ReauthenticateWithPopup_WebGL: function (promiseID, userID, providerJsonPtr, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, AuthWebGL.GetMissingUserError(userID));
            return;
        }

        var authProvider = AuthWebGL.GetAuthProviderObjectFromJsonValue(Pointer_stringify(providerJsonPtr));
        user.reauthenticateWithPopup(authProvider).then(function (userCredentials)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    ReauthenticateWithRedirect_WebGL: function (promiseID, userID, providerJsonPtr, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, AuthWebGL.GetMissingUserError(userID));
            return;
        }

        var authProvider = AuthWebGL.GetAuthProviderObjectFromJsonValue(Pointer_stringify(providerJsonPtr));
        user.reauthenticateWithRedirect(authProvider).then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    ReloadUser_WebGL: function (promiseID, userID, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, AuthWebGL.GetMissingUserError(userID));
            return;
        }

        user.reload().then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    SendEmailVerification_WebGL: function (promiseID, userID, actionCodeSettingsPtr, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, AuthWebGL.GetMissingUserError(userID));
            return;
        }
        var actionCodeSettings = AuthWebGL.GetActionCodeSettingsFromJson(actionCodeSettingsPtr);
        user.sendEmailVerification(actionCodeSettings).then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    UnlinkUser_WebGL: function (promiseID, userID, providerIDPtr, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, AuthWebGL.GetMissingUserError(userID));
            return;
        }

        var providerID = Pointer_stringify(providerIDPtr);
        user.unlink(providerID).then(function (user)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUser(user), null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, user, null);
        });
    },
    UpdateEmail_WebGL: function (promiseID, userID, newEmailPtr, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, AuthWebGL.GetMissingUserError(userID));
            return;
        }

        user.updateEmail(Pointer_stringify(newEmailPtr)).then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    UpdatePassword_WebGL: function (promiseID, userID, newPasswordPtr, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, AuthWebGL.GetMissingUserError(userID));
            return;
        }
        user.updatePassword(Pointer_stringify(newPasswordPtr)).then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    UpdatePhoneNumber_WebGL: function (promiseID, userID, credentialJsonPtr, callback)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, AuthWebGL.GetMissingUserError(userID));
            return;
        }
        var credential = firebase.auth.AuthCredential.fromJSON(Pointer_stringify(credentialJsonPtr));
        user.updatePhoneNumber(credential).then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    GetUserMetadata_WebGL: function (id)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            return null;
        }
        return AuthWebGL.AllocateString(JSON.stringify(user.metadata));
    },

    GetUserProviderID_WebGL: function (id)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            return null;
        }
        return AuthWebGL.AllocateString(user.providerId);
    },
    GetUserPhotoUrl_WebGL: function (id)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            return null;
        }
        return AuthWebGL.AllocateString(user.photoURL);
    },
    GetUserEmail_WebGL: function (id)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            return null;
        }
        return AuthWebGL.AllocateString(user.email);
    },
    GetUserDisplayName_WebGL: function (id)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            return null;
        }
        return AuthWebGL.AllocateString(user.displayName);
    },
    GetUserID_WebGL: function (id)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            return null;
        }

        return AuthWebGL.AllocateString(user.uid);
    },
    GetUserRefreshToken_WebGL: function (id)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            return null;
        }

        return AuthWebGL.AllocateString(user.refreshToken);
    },
    GetUserProviderData_WebGL: function (id)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            return null;
        }
        return AuthWebGL.AllocateString(JSON.stringify(user.providerData));
    },
    GetUserPhoneNumber_WebGL: function (id)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            return null;
        }
        return AuthWebGL.AllocateString(user.phoneNumber);
    },
    GetIsUserAnnonymous_WebGL: function (id)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            return false;
        }
        return user.isAnonymous;
    },
    GetIsUserEmailVerified_WebGL: function (id)
    {
        var user = AuthWebGL.GetUser(userID, true);
        if (!user)
        {
            return false;
        }
        return user.emailVerified;
    },
    GetCurrentAuthUser_WebGL: function (appNamePtr)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        var currentUser = firebase.auth(app).currentUser;
        if (!currentUser)
            return null;

        var wrapper = {};
        for (var index in AuthWebGL.usersInstances)
        {
            if (AuthWebGL.usersInstances[index] == currentUser)
            {
                wrapper.nativeLibID = index;
                break;
            }
        }
        if (!wrapper.nativeLibID)
            wrapper = AuthWebGL.AddUser(currentUser);

        return AuthWebGL.AllocateString(JSON.stringify(wrapper));
    },
    ReleaseUser_WebGL: function (id)
    {
        AuthWebGL.RemoveUser(id);
    },
    AuthUseDeviceLanguage_WebGL: function (appNamePtr)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        firebase.auth(app).useDeviceLanguage();
    },
    IsSignedInWithEmailLink_WebGL: function (appNamePtr, emailLinkPtr)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        var emailLink = Pointer_stringify(emailLinkPtr);
        return firebase.auth(app).isSignInWithEmailLink(emailLink);
    },
    FetchSignInMethodsForEmail_WebGL: function (promiseID, appNamePtr, emailPtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        var email = Pointer_stringify(emailPtr);

        firebase.auth(app).fetchSignInMethodsForEmail(email).then(function (methods)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, methods, null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    CreateUserWithEmailAndPassword_WebGL: function (promiseID, appNamePtr, emailPtr, passwordPtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        firebase.auth(app).createUserWithEmailAndPassword(Pointer_stringify(emailPtr), Pointer_stringify(passwordPtr))
            .then(function (userCredentials)
            {
                AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), null);
            }).catch(function (error)
            {
                AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
            });
    },
    ConfirmPasswordReset_WebGL: function (promiseID, appNamePtr, codePtr, newPasswordPtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        firebase.auth(app).confirmPasswordReset(Pointer_stringify(codePtr), Pointer_stringify(newPasswordPtr)).then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    CheckAuthActionCode_WebGL: function (promiseID, appNamePtr, codePtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        firebase.auth(app).checkActionCode(Pointer_stringify(codePtr)).then(function (actionCodeInfo)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, actionCodeInfo, null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    ApplyAuthActionCode_WebGL: function (promiseID, appNamePtr, codePtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        firebase.auth(app).applyActionCode(Pointer_stringify(codePtr)).then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    GetAuthRedirectResult_WebGL: function (promiseID, appNamePtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        firebase.auth(app).getRedirectResult().then(function (userCredentials)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    SendPasswordResetEmail_WebGL: function (promiseID, appNamePtr, emailPtr, actionCodeSettingsPtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        var email = Pointer_stringify(emailPtr);
        var actionCodeSettings = AuthWebGL.GetActionCodeSettingsFromJson(actionCodeSettingsPtr);
        firebase.auth(app).sendPasswordResetEmail(email, actionCodeSettings).then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    SendSignInLinkToEmail_WebGL: function (promiseID, appNamePtr, emailPtr, actionCodeSettingsPtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        var email = Pointer_stringify(emailPtr);
        var actionCodeSettings = AuthWebGL.GetActionCodeSettingsFromJson(actionCodeSettingsPtr);
        firebase.auth(app).sendSignInLinkToEmail(email, actionCodeSettings).then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    SetAuthPersistence_WebGL: function (promiseID, appNamePtr, persistencePtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        var persistence = Pointer_stringify(persistencePtr);
        firebase.auth(app).setPersistence(persistence).then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    SignInAnonymously_WebGL: function (promiseID, appNamePtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        firebase.auth(app).signInAnonymously().then(function (userCredentials)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    SignInWithCredential_WebGL: function (promiseID, appNamePtr, credentialObjectJsonPtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        var credential = firebase.auth.AuthCredential.fromJSON(Pointer_stringify(credentialObjectJsonPtr));
        firebase.auth(app).signInWithCredential(credential).then(function (userCredentials)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    SignInWithCustomToken_WebGL: function (promiseID, appNamePtr, token, callback)
    {
        firebase.auth(firebase.app(Pointer_stringify(appNamePtr))).signInWithCustomToken(Pointer_stringify(token)).then(function (userCredentials)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    SignInWithEmailAndPassword_WebGL: function (promiseID, appNamePtr, emailPtr, passwordPtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        firebase.auth(app).signInWithEmailAndPassword(Pointer_stringify(emailPtr), Pointer_stringify(passwordPtr)).then(function (userCredentials)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    SignInWithEmailLink_WebGL: function (promiseID, appNamePtr, emailPtr, callback, emailLinkPtr)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        firebase.auth(app).signInWithEmailLink(Pointer_stringify(emailPtr), Pointer_stringify(emailLinkPtr)).then(function (userCredentials)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    SignInWithPhoneNumber_WebGL: function (promiseID, appNamePtr, phoneNumberPtr, recaptchaParametersPtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        var recaptchaVerifier = AuthWebGL.GetOrCreateAuthProviderRecaptcha(Pointer_stringify(recaptchaParametersPtr));

        firebase.auth(app).signInWithPhoneNumber(Pointer_stringify(phoneNumberPtr), recaptchaVerifier).then(function (confirmationResult)
        {
            var verificationCode = window.prompt('Please enter the verification code that was sent to your mobile device.');
            confirmationResult.confirm(verificationCode).then(function (userCredentials)
            {
                AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), null);
            }).then(function (error)
            {
                AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
            });
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        }).finally(function ()
        {
            if (recaptchaVerifier)
                recaptchaVerifier.clear();
        });
    },
    SignInWithPopup_WebGL: function (promiseID, appNamePtr, providerJson, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        var authProvider = AuthWebGL.GetAuthProviderObjectFromJsonValue(Pointer_stringify(providerJson));
        firebase.auth(app).signInWithPopup(authProvider).then(function (userCredentials)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });

    },
    SignInWithRedirect_WebGL: function (promiseID, appNamePtr, providerJson, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        var authProvider = AuthWebGL.GetAuthProviderObjectFromJsonValue(Pointer_stringify(providerJson));
        firebase.auth(app).signInWithRedirect(authProvider).then(function (userCredentials)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, AuthWebGL.AddUserFromCredentials(userCredentials), null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    SignOut_WebGL: function (promiseID, appNamePtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        firebase.auth(app).signOut().then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    UpdateCurrentAuthUser_WebGL: function (promiseID, appNamePtr, nativeLibUserID, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        var user = AuthWebGL.GetU(nativeLibUserID);
        firebase.auth(app).updateCurrentUser(user).then(function ()
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, null);
        }).catch(function (error)
        {
            AuthWebGL.SendVoidPromiseCallback(promiseID, callback, error);
        });
    },
    VerifyPasswordResetCode_WebGL: function (promiseID, appNamePtr, codePtr, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        var code = Pointer_stringify(codePtr);
        firebase.auth(app).verifyPasswordResetCode(code).then(function (email)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, email, null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        });
    },
    SubscribeToAuthChange_WebGL: function (appNamePtr, listenerID, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));

        var unsubscribe = firebase.auth(app).onAuthStateChanged(function (user)
        {
            var wrapper = AuthWebGL.AddUser(user);
            AuthWebGL.SendAuthEventCallback(app.name, wrapper, null, callback);
        },
            function (error)
            {
                AuthWebGL.SendAuthEventCallback(app.name, null, error, callback);
            });
        AuthWebGL.authChangeListeners[listenerID] = unsubscribe;
    },
    UnsubscribeToAuthChange_WebGL: function (listenerID)
    {
        AuthWebGL.UnSubscribeAuthListener(listenerID);
    },
    SubscribeToIdTokenChange_WebGL: function (appNamePtr, listenerID, callback)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        var unsubscribe = firebase.auth(app).onIdTokenChanged(function (user)
        {
            var wrapper = AuthWebGL.AddUser(user);
            AuthWebGL.SendAuthEventCallback(app.name, wrapper, null, callback);
        }, function (error)
        {
            AuthWebGL.SendAuthEventCallback(app.name, null, error, callback);
        });
        AuthWebGL.authChangeListeners[listenerID] = unsubscribe;
    },
    UnSubscribeToIdTokenChange_WebGL: function (listenerID)
    {
        AuthWebGL.UnSubscribeAuthListener(listenerID);
    },
    GetLanguageCode_WebGL: function (appNamePtr)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        return AuthWebGL.AllocateString(firebase.auth(app).languageCode);
    },
    SetLanguageCode_WebGL: function (appNamePtr, languageCodePtr)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        firebase.auth(app).languageCode = Pointer_stringify(languageCodePtr);
    },
    GetAppVerificationDisabledForTesting_WebGL: function (appNamePtr)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        return firebase.auth(app).settings.appVerificationDisabledForTesting;
    },
    SetAppVerificationDisabledForTesting_WebGL: function (appNamePtr, disabled)
    {
        var app = firebase.app(Pointer_stringify(appNamePtr));
        firebase.auth(app).settings.appVerificationDisabledForTesting = disabled;
    },
    GetEmailProviderID_WebGL: function ()
    {
        return AuthWebGL.AllocateString(firebase.auth.EmailAuthProvider.PROVIDER_ID);
    },
    GetEmailCredential_WebGL: function (emailPtr, passwordPtr)
    {
        var credential = firebase.auth.EmailAuthProvider.credential(Pointer_stringify(emailPtr), Pointer_stringify(passwordPtr));
        return AuthWebGL.AllocateString(JSON.stringify(credential));
    },
    GetEmailLinkSignInMethod_WebGL: function ()
    {
        return AuthWebGL.AllocateString(firebase.auth.EmailAuthProvider.EMAIL_LINK_SIGN_IN_METHOD);
    },
    GetEmailPasswordSignInMethod_WebGL: function ()
    {
        return AuthWebGL.AllocateString(firebase.auth.EmailAuthProvider.EMAIL_PASSWORD_SIGN_IN_METHOD);
    },
    GetEmailLinkCredential_WebGL: function (emailPtr, emailLinkPtr)
    {
        var credential = firebase.auth.EmailAuthProvider.credentialWithLink(Pointer_stringify(emailPtr), Pointer_stringify(emailLinkPtr));
        return AuthWebGL.AllocateString(JSON.stringify(credential));
    },
    GetFacebookCredential_WebGL: function (tokenPtr)
    {
        var credential = firebase.auth.FacebookAuthProvider.credential(Pointer_stringify(tokenPtr));
        return AuthWebGL.AllocateString(JSON.stringify(credential));
    },
    GetFacebookProviderID_WebGL: function ()
    {
        return AuthWebGL.AllocateString(firebase.auth.FacebookAuthProvider.PROVIDER_ID);
    },
    GetFacebookSignInMethod_WebGL: function ()
    {
        return AuthWebGL.AllocateString(firebase.auth.FacebookAuthProvider.FACEBOOK_SIGN_IN_METHOD);
    },
    GetGithubProviderID_WebGL: function ()
    {
        return AuthWebGL.AllocateString(firebase.auth.GithubAuthProvider.PROVIDER_ID);
    },
    GetGithubSignInMethod_WebGL: function ()
    {
        return AuthWebGL.AllocateString(firebase.auth.GithubAuthProvider.GITHUB_SIGN_IN_METHOD);
    },
    GetGithubCredential_WebGL: function (tokenPtr)
    {
        var credential = firebase.auth.GithubAuthProvider.credential(Pointer_stringify(tokenPtr));
        return AuthWebGL.AllocateString(JSON.stringify(credential));
    },
    GetGoogleProviderID_WebGL: function ()
    {
        return AuthWebGL.AllocateString(firebase.auth.GoogleAuthProvider.PROVIDER_ID);
    },
    GetGoogleSignInMethod_WebGL: function ()
    {
        return AuthWebGL.AllocateString(firebase.auth.GoogleAuthProvider.GOOGLE_SIGN_IN_METHOD);
    },
    GetGoogleCredential_WebGL: function (idTokenPtr, accessTokenPtr)
    {
        var idToken = Pointer_stringify(idTokenPtr);
        var accessToken = Pointer_stringify(accessTokenPtr);
        var credential = firebase.auth.GoogleAuthProvider.credential(idToken, accessToken);
        return AuthWebGL.AllocateString(JSON.stringify(credential));
    },
    GetOAuthCredential_WebGL: function (providerIdPtr, idTokenPtr, accessTokenPtr)
    {
        var idToken = Pointer_stringify(idTokenPtr);
        var accessToken = Pointer_stringify(accessTokenPtr);
        var providerId = Pointer_stringify(providerIdPtr);
        var provider = new firebase.auth.OAuthProvider(providerId);
        var credential = provider.credential(idToken, accessToken);
        return AuthWebGL.AllocateString(JSON.stringify(credential));
    },
    PhoneProviderGetProviderID_WebGL: function ()
    {
        return AuthWebGL.AllocateString(firebase.auth.PhoneAuthProvider.PROVIDER_ID);
    },
    PhoneProviderGetSignInMethod_WebGL: function ()
    {
        return AuthWebGL.AllocateString(firebase.auth.PhoneAuthProvider.PHONE_SIGN_IN_METHOD);
    },
    PhoneAuthProviderVerifyPhoneNumber_WebGL: function (promiseID, phoneNumberPtr, recaptchaParametersJsonPtr, callback)
    {
        var phoneNumber = Pointer_stringify(phoneNumberPtr);
        var recaptchaVerifier = AuthWebGL.GetOrCreateAuthProviderRecaptcha(Pointer_stringify(recaptchaParametersJsonPtr));

        var providerInstance = new firebase.auth.PhoneAuthProvider();
        providerInstance.verifyPhoneNumber(phoneNumber, recaptchaVerifier).then(function (verificationID)
        {
            var verificationCode = window.prompt('Please enter the verification code that was sent to your mobile device.');
            var data =
            {
                verificationID: verificationID,
                verificationCode: verificationCode,
            };
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, data, null);
        }).catch(function (error)
        {
            AuthWebGL.SendOneParameterPromiseCallback(promiseID, callback, null, error);
        }).finally(function ()
        {
            if (recaptchaVerifier)
                recaptchaVerifier.clear();
        });
    },
    PhoneProviderGetCredential_WebGL: function (verificationIdPtr, verificationCodePtr)
    {
        var verificationId = Pointer_stringify(verificationIdPtr);
        var verificationCode = Pointer_stringify(verificationCodePtr);
        var credential = firebase.auth.PhoneAuthProvider.credential(verificationId, verificationCode);
        return AuthWebGL.AllocateString(JSON.stringify(credential));
    },
    GetTwitterProviderID_WebGL: function ()
    {
        return AuthWebGL.AllocateString(firebase.auth.TwitterAuthProvider.PROVIDER_ID);
    },
    GetTwitterSignInMethod_WebGL: function ()
    {
        return AuthWebGL.AllocateString(firebase.auth.TwitterAuthProvider.TWITTER_SIGN_IN_METHOD);
    },
    TwitterProviderGetCredentials_WebGL: function (tokenPtr, secretPtr)
    {
        var token = Pointer_stringify(tokenPtr);
        var secret = Pointer_stringify(secretPtr);
        var credential = firebase.auth.TwitterAuthProvider.credential(token, secret);
        return AuthWebGL.AllocateString(JSON.stringify(credential));
    },
    GetActionCodeStringFromEnumIndex_WebGL: function (enumIndex)
    {
        switch (enumIndex)
        {
            case 0:
                return AuthWebGL.AllocateString(firebase.auth.ActionCodeInfo.Operation.PASSWORD_RESET);
            case 1:
                return AuthWebGL.AllocateString(firebase.auth.ActionCodeInfo.Operation.VERIFY_EMAIL);
            case 2:
                return AuthWebGL.AllocateString(firebase.auth.ActionCodeInfo.Operation.RECOVER_EMAIL);
            case 3:
                return AuthWebGL.AllocateString(firebase.auth.ActionCodeInfo.Operation.EMAIL_SIGNIN);
            default:
                return AuthWebGL.AllocateString("Unknown_Action");
        }
    },
};
autoAddDeps(FirebaseAuthJS, '$AuthWebGL');
mergeInto(LibraryManager.library, FirebaseAuthJS);