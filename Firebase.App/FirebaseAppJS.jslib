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
    GetDefaultAppName_WebGL: function ()
    {
        return AppWebGL.AllocateString("[DEFAULT]");
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
            return false;
        }
    },
    
};
autoAddDeps(FirebaseAppJS, '$AppWebGL');
mergeInto(LibraryManager.library, FirebaseAppJS);