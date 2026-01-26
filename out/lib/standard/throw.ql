import "$lib/standard"
import "$lib/core"

// Class to throw exceptions
namespace std:  {
    class Throw: {
        // std::Throw default exception
        function exception(const<String> message): {
            _native("std.exception.throw", _str(message));
        }

        // Thow exception if function is not implement
        function notImplementedException(): {
            exception("This function is not implement");
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