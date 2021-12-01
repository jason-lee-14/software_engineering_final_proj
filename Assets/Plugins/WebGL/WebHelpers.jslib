mergeInto(LibraryManager.library, {

    GetOrigin: function () {
        var origin = window.location.origin;
        var bufferSize = lengthBytesUTF8(origin) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(origin, buffer, bufferSize);
        return buffer;
    },

});
