class Parser: {
    static function asInt(let object): {
        return _native("parse_int", object);
    }

    static function asFloat(let object): {
        return _native("parse_float", object);
    }

    static function asString(let object): {
        return _native("parse_string", object);
    }
}