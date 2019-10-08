4var WebGLTaskJS =
{
    $PromisesWebGL:
    {

    },
    AllocateString: function (str)
    {
        if (str)
        {
            var length = lengthBytesUTF8(str) + 1;
            var buff = _malloc(length);

            stringToUTF8Array(str, HEAPU8, buff, length);

            return buff;
        }
        return 0;
    },

    SendOneParameterPromiseCallback: function (promiseID, callback, rawData, error)
    {
        var dataBytes = _AllocateString(rawData ? JSON.stringify(rawData) : null);
        var errorBytes = _AllocateString(error ? JSON.stringify(error) : null);

        Runtime.dynCall('viii', callback, [promiseID, dataBytes, errorBytes]);

        if (dataBytes != 0)
            _free(dataBytes);
        if (errorBytes != 0)
            _free(errorBytes);
    },
    SendVoidPromiseCallback: function (promiseID, callback, error)
    {
        var errorBytes = _AllocateString(error ? JSON.stringify(error) : null);
        Runtime.dynCall('vii', callback, [promiseID, errorBytes]);
        if (errorBytes != 0)
            _free(errorBytes);
    },
    SendByteArrayPromiseCallback: function (promiseID, callback, byteArray, error)
    {
        var errorBytes = _AllocateString(error ? JSON.stringify(error) : null);
        var bytes = byteArray ? byteArray : 0;
        var length = bytes ? bytes.length : 0;

        var buffer = _malloc(length);
        HEAPU8.set(bytes, buffer);

        Runtime.dynCall('viiii', callback, [promiseID, buffer, length, errorBytes]);

        _free(buffer);

        if (errorBytes != 0)
            _free(errorBytes);
    },

    ByteArrayToString: function (data)
    {
        var count = data.length;
        var result = "";
        for (var i = 0; i < count; i++)
        {
            result += String.fromCharCode(data[i]);
        }
        return result;
    },
};
autoAddDeps(WebGLTaskJS, '$PromisesWebGL');
mergeInto(LibraryManager.library, WebGLTaskJS);
