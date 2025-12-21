include "$lib/base"

// Class to make string operations
class String: {
    private let _value = "";

    // overriding functions 
    function toString(): {
        return Object.toString(_value);
    }
    
    // Parse object to String
    function ___create_from___(const obj):
        return String.new(obj);

    // Parse additional operations
    function ___operator_plus___(const obj1, const obj2):
        return String.new(obj1._value + obj2._value);

    // Parse multiplication operations
    function ___operator_star___(const obj1, const obj2): {
        let val = "";

        for let i = 0; i < obj2._value; i = i + 1:
            val = val + obj1;

        return String.new(val);
    }

    // Parse division operations
    function ___operator_slash___(const obj1, const obj2): {
        let val = "";

        const index = obj1.length() / obj2._value;

        for let i = 0; i < index; i = i + 1:
            val = val + obj1.charAt(i);

        return String.new(val);
    }

    // Parse '==' operations
    function ___operator_equal_equal___(const obj1, const obj2):
        return obj1._value == obj2._value;

    // Parse '!=' operations
    function ___operator_not_equal___(const obj1, const obj2):
        return obj1._value == obj2._value;

    // Parse '>=' operations
    function ___operator_greater_equal___(const obj1, const obj2): 
        return obj1.length() >= obj2.length();

    // Parse '<=' operations
    function ___operator_less_equal___(const obj1, const obj2): 
        return obj1.length() <= obj2.length();

    // Parse '>' operations
    function ___operator_greater___(const obj1, const obj2): 
        return obj1.length() > obj2.length();

    // Parse '<' operations
    function ___operator_less___(const obj1, const obj2): 
        return obj1.length() < obj2.length();

    function new(const<String> input): {
        if String.isString(input):
            _value = input.toString();
        else:
            _value = input;
    }

    function new(const<String> char, const<Number> count): {
        if (Number.isNumber(count) == false):
            Throw.exception("Param 'count' must be number");

        if count < 0: 
            Throw.exception("Param 'count' must be more than 0");

        if String.new(char).length() < 0:
            Throw.exception("Length of string must be more than 0");

        _value = _native("lib.string.create", char, count);
    }

    function getPrimitive(const strOrPrimite, let allowOther = false): {
        if (Object.isNull(strOrPrimite)): {
            Throw.exception("Object is null");
        }

        if (String.isString(strOrPrimite)): {
            
            return strOrPrimite.toString();
        }

        if (String.isPrimitive(strOrPrimite)): {
            return strOrPrimite;
        }

        if allowOther: {
            return strOrPrimite;
        }

        Throw.exception("Object is not string or primitive");
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
        return String.new(_native("lib.string.to_lower", _str(_value)));
    }

    function toUpper(): {
        return String.new(_native("lib.string.to_upper", _str(_value)));
    }

    function isString(let value): {
        value =  _native("lib.string.is_str", value);
        return value;
    }

    function isPrimitive(let value): {
        return _native("lib.string.is_primitive", value);
    }

    function split(let<String> pattern): {
        return Array.new(_native("lib.string.split", _str(_value), _str(pattern)));
    }

    function charAt(const<Number> index): {
        if length() <= index:
            Throw.exception("The index must be less than the length of the string.");

        return _native("lib.string.at", _str(_value), index);
    }

    function setAt(const<Number> index, const<String> replaceValue): {
        if length() <= index:
            Throw.exception("The index must be less than the length of the string.");

        _value = _native("lib.string.set_at", _str(_value), _str(replaceValue), index);
    }

    function join(let strArr, let<String> pattern): {
        if (Array.isCollection(strArr) == false) && (Array.isArray(strArr) == false):
            Throw.exception("argument is not collection or array");

        if Array.isArray(strArr):
            strArr = strArr.getCollection();

        return String.new(_native("lib.string.join", strArr, pattern));
    }

    // Get length of string
    function length():
        return _native("lib.string.length", _str(_value));

    // Check if string is empty or null
    function isNullOrEmpty(let<String> str): {
        if (String.isPrimitive(str) == false) && (String.isString(str) == false):
            Throw.exception("Param must be string class or primitive");

        if String.isString(str):
            str = str.toString();

        return _native("lib.string.is_null_or_empty", _str(str));
    }
    
    // Check if string is white space or null
    function isNullOrWhitespace(let<String> str): {
        if (String.isPrimitive(str) == false) && (String.isString(str) == false):
            Throw.exception("Param must be string class or primitive");

        if String.isString(str): 
            str = str.toString();

        return _native("lib.string.is_null_or_whitespace", _str(str));
    }

    // Trim string
    function trim():
        return String.new(_native("lib.string.trim", _str(_value)));

    // Trim start string
    function trimStart():
        return String.new(_native("lib.string.trim_start", _str(_value)));

    // Trim end string
    function trimEnd():
        return String.new(_native("lib.string.trim_end", _str(_value)));

    // Cut string by 'startPos' and 'length'
    function subString(let<Number> startPos, let<Number> length): {
        if Number.isNumber(startPos) == false:
            Throw.exception("subString error: startPos must be number");

        if Number.isNumber(length) == false:
            Throw.exception("subString error: length must be number");

        if length() <= length:
            Throw.exception("Value 'length' can't be more than string length");

        return String.new(_native("lib.string.substring", _str(_value), startPos, length));
    }
}