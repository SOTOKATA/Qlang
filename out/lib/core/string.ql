import "$lib/core"

// Class to make string operations
const string = new String("");
class String extends DataType: {
    // overriding functions 
    function<String> toString():
        return object.toString(_value);

    function<String> toNumberFormat(const<String> format):
        return _native("std", "number", "to_string", <Number>_value, format.getValue());

    // Parse object to String
    function _createFrom(const obj):
        return new String(obj);

    // Parse additional operations
    function _operatorAddition(const obj1, const obj2):
        return new String(obj1.getValue() + obj2.getValue());
        

    // Parse multiplication operations
    function _operatorMultiplication(const obj1, let obj2): {
        let val = "";

        obj2 = <Number>obj2.getValue();

        for let i = 0; i < obj2; i++:
            val = val + obj1;

        return new String(val);
    }

    // Parse division operations
    function _operatorDivision(const obj1, const obj2): {
        let val = "";

        const index = obj1.length() / obj2.getValue();

        for let i = 0; i < index; i++:
            val = val + obj1.charAt(i);

        return new String(val);
    }

    // Parse '==' operations
    function _operatorEqual(const obj1, const obj2):
        return obj1.getValue() == obj2.getValue();
        

    // Parse '!=' operations
    function _operatorNotEqual(const obj1, const obj2):
        return obj1.getValue() != obj2.getValue();

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

    function new(const input): {
        if isString(input):
            _value = input.getValue();
        else:
            _value = str(input);
    }

    function new(const<String> char, let<Number> count): {
        count = std::math.round(count, 0);
        
        if count < 0: 
            std::throw.message("Param 'count' must be more than 0");

        if (new String(char)).length() < 0:
            std::throw.message("Length of string must be more than 0");

        _value = _native("std", "string", "create", char, count);
    }

    function getPrimitive(const strOrPrimite, let allowOther = false): {
        if object.isNull(strOrPrimite):
            std::throw.message("Object is null");

        if isString(strOrPrimite):
            return strOrPrimite.toString();

        if isPrimitive(strOrPrimite):
            return strOrPrimite;

        if allowOther:
            return strOrPrimite;

        std::throw.message("Object is not string or primitive");
    }

    private function _checkIsCollection(let value): {
        if !array.isCollection(value): {
            std::throw.parceException("argument is not collection");
        }
    }

    private function _checkIsArray(let value): {
        if typeof(value) != "Array": {
            std::throw.parceException("argument is not array");
        }
    }

    // Append two strings
    function append(let collection): {
        _checkIsCollection(collection);

        let result = "";

        let arr = new Array(collection);

        for let i = 0; i < arr.length(); i++: {
            result = result + arr.at(i);
        }

        return new String(result);
    }

    function toLower(): {
        return new String(_native("std", "string", "toLower", _str(_value)));
    }

    function toUpper(): {
        return new String(_native("std", "string", "toUpper", _str(_value)));
    }

    function isString(let value): {
        value =  _native("std", "string", "isStr", value);
        return value;
    }

    function isPrimitive(let value): {
        return _native("std", "string", "isPrimitive", value);
    }

    function split(let<String> pattern): {
        return new Array(_native("std", "string", "split", _str(_value), _str(pattern)));
    }

    function charAt(const<Number> index): {
        if length() <= index:
            std::throw.message("The index must be less than the length of the ");

        return _native("std", "string", "at", _str(_value), index);
    }

    function setAt(const<Number> index, const<String> replaceValue): {
        if length() <= index:
            std::throw.message("The index must be less than the length of the ");

        _value = _native("std", "string", "setAt", _str(_value), _str(replaceValue), index);
    }

    function join(let strArr, let<String> pattern): {
        if !array.isCollection(strArr) && (typeof(strArr) != "Array"):
            std::throw.message("argument is not collection or array");

        if typeof(strArr) == "Array":
            strArr = strArr.getCollection();

        return new String(_native("std", "string", "join", strArr, pattern));
    }

    // Get length of string
    function length():
        return _native("std", "string", "length", _str(_value));

    // Check if string is empty or null
    function isNullOrEmpty(let str): {
        if object.isNull(str):
            return true;

        if !isPrimitive(str) && !isString(str):
            std::throw.message("Param must be string class or primitive");

        if isString(str):
            str = str.toString();

        return _native("std", "string", "isNullOrEmpty", _str(str));
    }
    
    // Check if string is white space or null
    function isNullOrWhitespace(let str): {
        if object.isNull(str):
            return true;

        if !isPrimitive(str) && !isString(str):
            std::throw.message("Param must be string class or primitive");

        if isString(str): 
            str = str.toString();

        return _native("std", "string", "isNullOrWhitespace", _str(str));
    }

    function startsWith(const<String> str):
        return _native("std", "string", "startsWith", _str(_value), str);

    function endsWith(const<String> str):
        return _native("std", "string", "endsWith", _str(_value), str);

    // Trim string
    function trim(const<String> str):
        return new String(_native("std", "string", "trim_b", _str(_value), _str(str)));

    // Trim start string
    function trimStart(const<String> str):
        return new String(_native("std", "string", "trimStart_b", _str(_value), _str(str)));

    // Trim end string
    function trimEnd(const<String> str):
        return new String(_native("std", "string", "trimEnd_b", _str(_value), _str(str)));

        // Trim string
    function trim():
        return new String(_native("std", "string", "trim", _str(_value)));

    // Trim start string
    function trimStart():
        return new String(_native("std", "string", "trimStart", _str(_value)));

    // Trim end string
    function trimEnd():
        return new String(_native("std", "string", "trimEnd", _str(_value)));

    // Cut string by 'startPos' and 'length'
    function subString(let<Number> startPos, let<Number> length): {
        if length() <= length:
            std::throw.message("Value 'length' can't be more than string length");

        return new String(_native("std", "string", "subString", _str(_value), startPos, length));
    }

    // get index of first 'toFind'
    function indexOf(const<String> toFind):
        return _native("std", "string", "indexOf", _value, toFind);

    // get index of last 'toFind'
    function lastIndexOf(const<String> toFind):
        return _native("std", "string", "lastIndexOf", _value, toFind);

    // Replace '{n}' to replacement
    function format(const<Collection> replacement): {
        if replacement.length() == 0:
            return _value;

        return _native("std", "string", "format", _value, 
            replacement.select(
                fn(const x) => x.toString()
            ).getCollection());
    }
}