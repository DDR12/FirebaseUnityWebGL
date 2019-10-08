var FirebaseFunctionsJavascriptLib =
{
    $FunctionsWebGL:
    {
        httpCallablesRefs: {},
        AddHttpCallable: function (id, ref)
        {
            FunctionsWebGL.httpCallablesRefs[id] = ref;
        },
        GetHttpCallable: function (id)
        {
            return FunctionsWebGL.httpCallablesRefs[id];
        },
        RemoveHttpCallable: function (id)
        {
            delete FunctionsWebGL.httpCallablesRefs[id];
        },

        GetFunctionErrorCodeFromLiteralValue: function (literalValue)
        {
            var errors =
                [
                    'ok',
                    'cancelled',
                    'unknown',
                    'invalid-argument',
                    'deadline-exceeded',
                    'not-found',
                    'already-exists',
                    'permission-denied',
                    'resource-exhausted',
                    'failed-precondition',
                    'aborted',
                    'out-of-range',
                    'unimplemented',
                    'internal',
                    'unavailable',
                    'data-loss',
                    'unauthenticated',
                ];
            for (var i = 0; i < errors.length; i++)
            {
                if (errors[i] == literalValue)
                    return i;
            }
            return literalValue;
        },
    },
    
    UseFunctionsEmulator_WebGL: function (appNamePtr, originPtr)
    {
        var app = _GetApp(appNamePtr);
        firebase.functions(app).useFunctionsEmulator(Pointer_stringify(originPtr));
    },
    CreateCallableReference_WebGL: function (refID, appNamePtr, functionNamePtr, optionsJsonPtr)
    {
        var app = _GetApp(appNamePtr);

        var ref = firebase.functions(app).httpsCallable(Pointer_stringify(functionNamePtr), JSON.parse(Pointer_stringify(optionsJsonPtr)));
        FunctionsWebGL.AddHttpCallable(refID, ref);
    },
    ReleaseHttpCallableReference_WebGL: function (refID)
    {
        FunctionsWebGL.RemoveHttpCallable(refID);
    },
    CallFunction_WebGL: function (refID, promiseID, dataPtr, callback)
    {
        var ref = FunctionsWebGL.GetHttpCallable(refID);
        if (!ref)
        {
            var msg = "The underlying object of the HttpsCallableReference was lost.";
            console.log(msg);
            var error =
            {
                code: 404,
                message: msg,
            };
            _SendOneParameterPromiseCallback(promiseID, callback, null, error);
            return;
        } 
        var data = JSON.parse(Pointer_stringify(dataPtr));
        ref(data).then(function (result)
        {
            _SendOneParameterPromiseCallback(promiseID, callback, result, null);
        }).catch(function (error)
        {
            var functionsError =
            {
                code: FunctionsWebGL.GetFunctionErrorCodeFromLiteralValue(error.code),
                message: error.message,
            };
            _SendOneParameterPromiseCallback(promiseID, callback, null, functionsError);
        });
    },
};
autoAddDeps(FirebaseFunctionsJavascriptLib, '$FunctionsWebGL');
mergeInto(LibraryManager.library, FirebaseFunctionsJavascriptLib);