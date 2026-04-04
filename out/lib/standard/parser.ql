import "$lib/standard"
import "$lib/core"

namespace std:  {
    const parser = new Parser();
    private class Parser: {
        function asInt(object) => #std.Parser.Int(_str(object));

        function asFloat(object) => #std.Parser.Float(_str(object));

        function<String> asString(object) => #std.Parser.String(_str(object));
        
        function<Number> asNumber(object) => #std.Parser.Number(_str(object));
    }
}