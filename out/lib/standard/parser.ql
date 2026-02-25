import "$lib/standard"
import "$lib/core"

namespace std:  {
    class Parser: {
        function asInt(const object): {
            return _native("std", "parser", "int", object);
        }

        function asFloat(const object): {
            return _native("std", "parser", "float", object);
        }

        function<String> asString(const object): {
            return _native("std", "parser", "string", object);
        }
        
        function<Number> asNumber(const object): {
            return _native("std", "parser", "number", object);
        }
    }
}