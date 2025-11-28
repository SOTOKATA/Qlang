// Class to make string operations
class String: {
    private let _value = "";

    function new(let input): {
        _value = input;
    }

    function toString(): {
        return _value;
    }

    // Append two strings
    function append(let collection): {
        if Array.isArray(collection) == false: {
            Throw.parceException("argument is not collection");
        }

        let result = "";

        let arr = Array.new(collection);

        for let i = 0; i < arr.length(); i = i + 1: {
            result = result + arr.at(i);
        }

        return result;
    }

    // Get length of string
    function length(): {
        Console.print(_value);
        return _native("str_length", _str(_value));
    }

    // Check if string is empty or null
    function isNullOrEmpty(let str): {
        return _native("str_null_or_empty", _str(str));
    }
    
    // Check if string is white space or null
    function isNullOrWhitespace(let str): {
        return _native("str_null_or_white_space", _str(str));
    }

    // Trim string
    function trim(): {
        return _native("str_trim", _str(_value));
    }

    // Trim start string
    function trimStart(): {
        return _native("str_trim_start", _str(_value));
    }

    // Trim end string
    function trimEnd(): {
        return _native("str_trim_end", _str(_value));
    }

    // Cut string by 'startPos' and 'length'
    function subString(let startPos, let length): {
        if Number.isNumber(startPos) == false: {
            Throw.exception("subString error: startPos must be number");
        }

        if Number.isNumber(length) == false: {
            Throw.exception("subString error: length must be number");
        }

        return _native("str_sub_string", _str(_value), startPos, length);
    }
}