import "$lib/standard"
import "$lib/core"

namespace std:  {
    const parser = new Parser();
    private class Parser: {
        function asInt(const object) => _native("std", "parser", "int", _str(object));

        function asFloat(const object) => _native("std", "parser", "float", _str(object));

        function<String> asString(const object) => _native("std", "parser", "string", object);
        
        function<Number> asNumber(const object) => _native("std", "parser", "number", _str(object));
    }
}