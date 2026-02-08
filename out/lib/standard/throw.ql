import "$lib/standard"
import "$lib/core"

// Class to throw exceptions
namespace std:  {
    class Throw: {
        // std::Throw default exception
        function message(const<String> message): {
            _native("std", "exception", "throw", _str(message), true);
        }

        function exception(const exception): 
            _native("std", "exception", "throw", _str(exception.toString()), exception.printStackTrace);

        // Thow exception if function is not implement
        function notImplementedException(): {
            message("This function is not implement");
        }

        // Thow exception when parse error
        function parseException(const<String> error): {
            message("Parse error: " + error);
        }

        // Thow exception when object is null
        function nullException(): {
            message("Object is null");
        }
    }
}