include "$lib/base"

class Parser: {
    static function asInt(let object): {
        return _native("lib.parser.int", object);
    }

    static function asFloat(let object): {
        return _native("lib.parser.float", object);
    }

    static function asString(let object): {
        return _native("lib.parser.string", object);
    }
    
    static function asNumber(let object): {
        return _native("lib.parser.number", object);
    }
}