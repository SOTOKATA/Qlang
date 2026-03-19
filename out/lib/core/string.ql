import "$lib/core"

// Class to make string operations
const string = new String("");
class String extends DataType: {
    // overriding functions 
    function<String> toString() => object.toString(_value);

    function<String> toNumberFormat(const<String> format) => _native("std", "number", "to_string", <Number>_value, format.getValue());

    // Parse object to String
    function _createFrom(const obj) => new String(obj);

    // Parse additional operations
    function _operatorAddition(const obj1, const obj2) => new String(obj1.getValue() + obj2.getValue());
        

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
    function _operatorEqual(const obj1, const obj2) => obj1.getValue() == obj2.getValue();
        

    // Parse '!=' operations
    function _operatorNotEqual(const obj1, const obj2) => obj1.getValue() != obj2.getValue();

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

    function new(const input):
        _value = if typeof(input) == "String" ? input.getValue() : str(input);

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

        if typeof(strOrPrimite) == "String":
            return strOrPrimite.toString();

        if isPrimitive(strOrPrimite):
            return strOrPrimite;

        if allowOther:
            return strOrPrimite;

        std::throw.message("Object is not string or primitive");
    }

    // Append two strings
    function append(const<Array|Collection> collection): {
        let result = "";

        let arr = new Array(collection);

        for let i = 0; i < arr.length(); i++:
            result = result + arr.at(i);

        return new String(result);
    }

    function toLower() => new String(_native("std", "string", "toLower", _str(_value)));

    function toUpper() => new String(_native("std", "string", "toUpper", _str(_value)));

    function isPrimitive(let value) => _native("std", "string", "isPrimitive", value);

    function split(let<String> pattern) => new Array(_native("std", "string", "split", _str(_value), _str(pattern)));

    function charAt(const<Number> index): {
        throwIfNotInRange(index);

        return _native("std", "string", "at", _str(_value), index);
    }

    function setAt(const<Number> index, const<String> replaceValue): {
        throwIfNotInRange(index);

        _value = _native("std", "string", "setAt", _str(_value), _str(replaceValue), index);
    }

    function join(const<Collection|Array> strArr, let<String> pattern)
    => new String(_native("std", "string", "join", strArr.getCollection(), pattern));

    // Get length of string
    function length() => _native("std", "string", "length", _str(_value));

    // Check if string is empty or null
    function isNullOrEmpty(const<String|null> str): {
        if object.isNull(str):
            return true;

        return _native("std", "string", "isNullOrEmpty", _str(str));
    }
    
    // Check if string is white space or null
    function isNullOrWhitespace(const<String|null> str): {
        if object.isNull(str):
            return true;

        return _native("std", "string", "isNullOrWhitespace", _str(str));
    }

    function<Boolean> startsWith(const<String> str) => _native("std", "string", "startsWith", _str(_value), str);

    function<Boolean> endsWith(const<String> str) => _native("std", "string", "endsWith", _str(_value), str);

    // Trim string
    function<String> trim(const<String> str) => new String(_native("std", "string", "trim_b", _str(_value), _str(str)));

    // Trim start string
    function<String> trimStart(const<String> str) => new String(_native("std", "string", "trimStart_b", _str(_value), _str(str)));

    // Trim end string
    function<String> trimEnd(const<String> str) => new String(_native("std", "string", "trimEnd_b", _str(_value), _str(str)));

    // Trim string
    function<String> trim() => new String(_native("std", "string", "trim", _str(_value)));

    // Trim start string
    function<String> trimStart() => new String(_native("std", "string", "trimStart", _str(_value)));

    // Trim end string
    function<String> trimEnd() => new String(_native("std", "string", "trimEnd", _str(_value)));

    // Cut string by 'startPos' and 'length'
    function<String> subString(let<Number> startPos, let<Number> length): {
        throwIfNotInRange(startPos);
        throwIfNotInRange(length);

        return new String(_native("std", "string", "subString", _str(_value), startPos, length));
    }

    // get index of first 'toFind'
    function<String> indexOf(const<String> toFind) => _native("std", "string", "indexOf", _value, toFind);

    // get index of last 'toFind'
    function<String> lastIndexOf(const<String> toFind) => _native("std", "string", "lastIndexOf", _value, toFind);

    // Replace '{n}' to replacement
    function<String> format(const<Collection|Array> replacement): {
        if replacement.length() == 0:
            return _value;

        return _native("std", "string", "format", _value, 
            replacement.select(
                fn(const x) => x.toString()
            ).getCollection());
    }

    private function<Boolean> throwIfNotInRange(const<Number> index):
        if index < 0 && index >= length():
            std::throw.message(`Index '{index}' is out of range.`);
}