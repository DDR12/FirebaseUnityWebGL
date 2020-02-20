var FirebaseAppJS =
{
    $AppWebGL:
    {
        AllocateString: function (str) {
            if (str) {
                var length = lengthBytesUTF8(str) + 1;
                var buff = _malloc(length);

                stringToUTF8Array(str, HEAPU8, buff, length);

                return buff;
            }
            return 0;
        },
    },
    InitializeFirebaseApp_WebGL: function (appNamePtr, optionsPtr)
    {
        var appName = Pointer_stringify(appNamePtr);
        var options = JSON.parse(Pointer_stringify(optionsPtr));
        try
        {
            var app = firebase.initializeApp(options, appName);
            return app != null;
        }
        catch (e)
        {
            consol.log("Error initializing " + appName + ": " + e.message);
            return false;
        }
    },
    
};
autoAddDeps(FirebaseAppJS, '$AppWebGL');
mergeInto(LibraryManager.library, FirebaseAppJS);