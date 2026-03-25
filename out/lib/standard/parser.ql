import "$lib/standard"
import "$lib/core"

namespace std:  {
    const parser = new Parser();
    private class Parser: {
        function asInt(object) => _native("std", "parser", "int", _str(object));

        function asFloat(object) => _native("std", "parser", "float", _str(object));

        function<String> asString(object) => _native("std", "parser", "string", object);
        
        function<Number> asNumber(object) => _native("std", "parser", "number", _str(object));
    }
}