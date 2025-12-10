include "$lib/base"

class Parser: {
    static function asInt(let object): {
        return _native("lib.parse_int", object);
    }

    static function asFloat(let object): {
        return _native("lib.parse_float", object);
    }

    static function asString(let object): {
        return _native("lib.parse_string", object);
    }
    
    static function asNumber(let object): {
        return _native("lib.parse_number", object);
    }
}