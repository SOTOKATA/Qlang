import "$lib/standard"
import "$lib/core"

namespace std:  {
    class Parser: {
        static function asInt(const object): {
            return _native("std.parser.int", object);
        }

        static function asFloat(const object): {
            return _native("std.parser.float", object);
        }

        static function asString(const object): {
            return _native("std.parser.string", object);
        }
        
        static function asNumber(const object): {
            return _native("std.parser.number", object);
        }
    }
}