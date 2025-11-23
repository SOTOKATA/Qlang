// Class to make string operations
class String: {
    private let _value = "";

    function new(let input): {
        _value = input;
    }

    function toString(): {
        return _value;
    }

    // Get new c# string (internal)
    private function _str(let str): {
        return "new string(" + _str(str) + ")";
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
        return _csharp(_str(_value) + ".Length");
    }

    // Check if string is empty or null
    function isNullOrEmpty(let str): {
        return _csharp("string.IsNullOrEmpty(" + _str(str) + ")");
    }

    // Check if string is white space or null
    function isNullOrWhiteSpace(let str): {
        return _csharp("string.IsNullOrWhiteSpace(" + _str(str) + ")");
    }

    // Trim string
    function trim(): {
        return _csharp(_str(str) + ".Trim()");
    }

    // Trim start string
    function trimStart(): {
        return _csharp(_str(_value) + ".TrimStart()");
    }

    // Trim end string
    function trimEnd(): {
        return _csharp(_str(_value) + ".TrimEnd()");
    }

    // Cut string by 'startPos' and 'length'
    function subString(let startPos, let length): {
        if Number.isNumber(startPos) == false: {
            Throw.exception("subString error: startPos must be number");
        }

        if Number.isNumber(length) == false: {
            Throw.exception("subString error: length must be number");
        }

        return _csharp(_str(_value) + ".Substring(" + startPos + "," + length + ")");
    }
}