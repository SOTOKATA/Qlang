import "$lib/core"

// Class to make string operations
class String extends DataType: {
    private let _value = "";

    // overriding functions 
    function toString(): {
        return Object.toString(_value);
    }

    // Parse object to String
    function _createFrom(const obj):
        return String.new(obj);

    // Parse additional operations
    function _operatorAddition(const obj1, const obj2):
        return String.new(obj1.getValue() + obj2.getValue());
        

    // Parse multiplication operations
    function _operatorMultiplication(const obj1, const obj2): {
        let val = "";

        for let i = 0; i < <Number>obj2.getValue(); i++:
            val = val + obj1;

        return String.new(val);
    }

    // Parse division operations
    function _operatorDivision(const obj1, const obj2): {
        let val = "";

        const index = obj1.length() / obj2.getValue();

        for let i = 0; i < index; i++:
            val = val + obj1.charAt(i);

        return String.new(val);
    }

    // Parse '==' operations
    function _operatorEqual(const obj1, const obj2):
        return obj1.getValue() == obj2.getValue();

    // Parse '!=' operations
    function _operatorNotEqual(const obj1, const obj2):
        return obj1.getValue() == obj2.getValue();

    // Parse '>=' operations
    function _operatorGreaterOrEqual(const obj1, const obj2): 
        return obj1.length() >= obj2.length();

    // Parse '<=' operations
    function _operatorLessOrEqual(const obj1, const obj2): 
        return obj1.length() <= obj2.length();

    // Parse '>' operations
    function _operatorGreater(const obj1, const obj2): 
        return obj1.length() > obj2.length();

    // Parse '<' operations
    function _operatorLess(const obj1, const obj2): 
        return obj1.length() < obj2.length();

    function _cast(const obj): {

    }

    function new(const input): {
        if String.isString(input):
            _value = input.getValue();
        else:
            _value = str(input);
    }

    function new(const<String> char, const<Number> count): {
        if count < 0: 
            std::Throw.message("Param 'count' must be more than 0");

        if String.new(char).length() < 0:
            std::Throw.message("Length of string must be more than 0");

        _value = _native("std", "string", "create", char, count);
    }

    function getPrimitive(const strOrPrimite, let allowOther = false): {
        if Object.isNull(strOrPrimite):
            std::Throw.message("Object is null");

        if String.isString(strOrPrimite):
            return strOrPrimite.toString();

        if String.isPrimitive(strOrPrimite):
            return strOrPrimite;

        if allowOther:
            return strOrPrimite;

        std::Throw.message("Object is not string or primitive");
    }

    private function _checkIsCollection(let value): {
        if Array.isCollection(value) == false: {
            std::Throw.parceException("argument is not collection");
        }
    }

    private function _checkIsArray(let value): {
        if typeof(value) != "Array": {
            std::Throw.parceException("argument is not array");
        }
    }

    // Append two strings
    function append(let collection): {
        _checkIsCollection(collection);

        let result = "";

        let arr = Array.new(collection);

        for let i = 0; i < arr.length(); i++: {
            result = result + arr.at(i);
        }

        return String.new(result);
    }

    function toLower(): {
        return String.new(_native("std", "string", "to_lower", _str(_value)));
    }

    function toUpper(): {
        return String.new(_native("std", "string", "to_upper", _str(_value)));
    }

    function isString(let value): {
        value =  _native("std", "string", "is_str", value);
        return value;
    }

    function isPrimitive(let value): {
        return _native("std", "string", "is_primitive", value);
    }

    function split(let<String> pattern): {
        return Array.new(_native("std", "string", "split", _str(_value), _str(pattern)));
    }

    function charAt(const<Number> index): {
        if length() <= index:
            std::Throw.message("The index must be less than the length of the string.");

        return _native("std", "string", "at", _str(_value), index);
    }

    function setAt(const<Number> index, const<String> replaceValue): {
        if length() <= index:
            std::Throw.message("The index must be less than the length of the string.");

        _value = _native("std", "string", "set_at", _str(_value), _str(replaceValue), index);
    }

    function join(let strArr, let<String> pattern): {
        if (Array.isCollection(strArr) == false) && (typeof(strArr) != "Array"):
            std::Throw.message("argument is not collection or array");

        if typeof(strArr) == "Array":
            strArr = strArr.getCollection();

        return String.new(_native("std", "string", "join", strArr, pattern));
    }

    // Get length of string
    function length():
        return _native("std", "string", "length", _str(_value));

    // Check if string is empty or null
    function isNullOrEmpty(let str): {
        if Object.isNull(str):
            return true;

        if (String.isPrimitive(str) == false) && (String.isString(str) == false):
            std::Throw.message("Param must be string class or primitive");

        if String.isString(str):
            str = str.toString();

        return _native("std", "string", "is_null_or_empty", _str(str));
    }
    
    // Check if string is white space or null
    function isNullOrWhitespace(let str): {
        if Object.isNull(str):
            return true;

        if (String.isPrimitive(str) == false) && (String.isString(str) == false):
            std::Throw.message("Param must be string class or primitive");

        if String.isString(str): 
            str = str.toString();

        return _native("std", "string", "is_null_or_whitespace", _str(str));
    }

    function startsWith(const<String> str):
        return _native("std", "string", "starts_with", _str(_value), str);

    function endsWith(const<String> str):
        return _native("std", "string", "ends_with", _str(_value), str);

    // Trim string
    function trim(const<String> str):
        return String.new(_native("std", "string", "trim_b", _str(_value), _str(str)));

    // Trim start string
    function trimStart(const<String> str):
        return String.new(_native("std", "string", "trim_start_b", _str(_value), _str(str)));

    // Trim end string
    function trimEnd(const<String> str):
        return String.new(_native("std", "string", "trim_end_b", _str(_value), _str(str)));

        // Trim string
    function trim():
        return String.new(_native("std", "string", "trim", _str(_value)));

    // Trim start string
    function trimStart():
        return String.new(_native("std", "string", "trim_start", _str(_value)));

    // Trim end string
    function trimEnd():
        return String.new(_native("std", "string", "trim_end", _str(_value)));

    // Cut string by 'startPos' and 'length'
    function subString(let<Number> startPos, let<Number> length): {
        if length() <= length:
            std::Throw.message("Value 'length' can't be more than string length");

        return String.new(_native("std", "string", "substring", _str(_value), startPos, length));
    }

    // get index of first 'toFind'
    function indexOf(const<String> toFind):
        return _native("std", "string", "index_of", _value, toFind);

    // get index of last 'toFind'
    function lastIndexOf(const<String> toFind):
        return _native("std", "string", "last_index_of", _value, toFind);

    // Replace '{n}' to replacement
    function format(const<Collection> replacement):
        return _native("std", "string", "format", _value, replacement);
}