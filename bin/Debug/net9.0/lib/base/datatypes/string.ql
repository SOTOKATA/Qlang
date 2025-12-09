include "$lib/base"

// Class to make string operations
class String: {
    private let _value = "";

    function toString(): {
        return _value;
    }

    function new(let input): {
        _value = input;
    }

    function getPrimitive(let strOrPrimite): {
        if (Object.isNull(strOrPrimite)): {
            Throw.exception("Object is null");
        }

        if (String.isString(strOrPrimite)): {
            return strOrPrimite.str();
        }

        if (String.isPrimitive(strOrPrimite)): {
            return strOrPrimite;
        }

        Throw.exception("Object is not string or primitive");
    }

    function str(): {
        return _value;
    }

    private function _checkIsCollection(let value): {
        if Array.isCollection(value) == false: {
            Throw.parceException("argument is not collection");
        }
    }

    private function _checkIsArray(let value): {
        if Array.isArray(value) == false: {
            Throw.parceException("argument is not array");
        }
    }

    // Append two strings
    function append(let collection): {
        _checkIsCollection(collection);

        let result = "";

        let arr = Array.new(collection);

        for let i = 0; i < arr.length(); i = i + 1: {
            result = result + arr.at(i);
        }

        return String.new(result);
    }

    function toLower(): {
        return String.new(_native("str_to_lower", _str(_value)));
    }

    function toUpper(): {
        return String.new(_native("str_to_upper", _str(_value)));
    }

    function isString(let value): {
        return _native("str_is_str", value);
    }

    function isPrimitive(let value): {
        return _native("str_is_primitive", value);
    }

    function split(let pattern): {
        return Array.new(_native("str_split", _str(_value), pattern));
    }

    function join(let strArr, let pattern): {
        if (Array.isCollection(strArr) == false) && (Array.isArray(strArr) == false): {
            Throw.exception("argument is not collection or array");
        }   


        if Array.isArray(strArr) == true: {
            strArr = strArr.getCollection();
        }

        return String.new(_native("str_join", strArr, pattern));
    }

    // Get length of string
    function length(): {
        return _native("str_length", _str(_value));
    }

    // Check if string is empty or null
    function isNullOrEmpty(let str): {
        if (String.isPrimitive(str) == false) && (String.isString(str) == false): {
            Throw.exception("Param must be string class or primitive");
        }

        if String.isString(str): {
            str = str.str();
        }

        return _native("str_null_or_empty", _str(str));
    }
    
    // Check if string is white space or null
    function isNullOrWhitespace(let str): {
        if (String.isPrimitive(str) == false) && (String.isString(str) == false): {
            Throw.exception("Param must be string class or primitive");
        }

        if String.isString(str): {
            str = str.str();
        }

        return _native("str_null_or_white_space", _str(str));
    }

    // Trim string
    function trim(): {
        return String.new(_native("str_trim", _str(_value)));
    }

    // Trim start string
    function trimStart(): {
        return String.new(_native("str_trim_start", _str(_value)));
    }

    // Trim end string
    function trimEnd(): {
        return String.new(_native("str_trim_end", _str(_value)));
    }

    // Cut string by 'startPos' and 'length'
    function subString(let startPos, let length): {
        if Number.isNumber(startPos) == false: {
            Throw.exception("subString error: startPos must be number");
        }

        if Number.isNumber(length) == false: {
            Throw.exception("subString error: length must be number");
        }

        if length() <= length: {
            Throw.exception("Value 'length' can't be more than string length");
        }

        return String.new(_native("str_sub_string", _str(_value), startPos, length));
    }
}