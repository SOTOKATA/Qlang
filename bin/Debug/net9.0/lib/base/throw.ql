// Class to throw exceptions
class Throw: {
    // Throw default exception
    function exception(let message): {
        _native("lib.exception.throw", _str(message));
    }

    // Thow exception if function is not implement
    function nonImplementException(): {
        exception("This function or object is not implement");
    }

    // Thow exception when parse error
    function parseException(let error): {
        exception("Parse error: " + error);
    }

    // Thow exception when object is null
    function nullException(): {
        exception("Object is null");
    }
}