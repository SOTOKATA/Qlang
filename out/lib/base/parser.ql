include "$lib/base"
include "$lib/core"

class Parser: {
    static function asInt(const object): {
        return _native("lib.parser.int", object);
    }

    static function asFloat(const object): {
        return _native("lib.parser.float", object);
    }

    static function asString(const object): {
        return _native("lib.parser.string", object);
    }
    
    static function asNumber(const object): {
        return _native("lib.parser.number", object);
    }
}