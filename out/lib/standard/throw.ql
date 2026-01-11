include "$lib/standard"
include "$lib/core"

// Class to throw exceptions
namespace std:  {
    class Throw: {
        // Throw default exception
        function exception(const<String> message): {
            _native("lib.exception.throw", _str(message));
        }

        // Thow exception if function is not implement
        function nonImplementException(): {
            exception("This function or object is not implement");
        }

        // Thow exception when parse error
        function parseException(const<String> error): {
            exception("Parse error: " + error);
        }

        // Thow exception when object is null
        function nullException(): {
            exception("Object is null");
        }
    }
}