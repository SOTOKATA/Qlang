import "$lib/core"

// Class to make string operations
const string = new String(""); 
class String extends DataType: {
    // overriding functions 
    function<String> toString() => object.toString(_value);

    // Parse object to String
    function _createFrom(obj) => new String(obj);

    // Parse additional operations
    function _operatorAddition(obj1, obj2) => new String(obj1.getValue() + obj2.getValue());
        

    // Parse multiplication operations
    function _operatorMultiplication(obj1, let obj2): {
        let val = "";

        if obj2 is not Number:
            std::throw.message("Cannot apply multiplication");

        obj2 = <Number>obj2.getValue();

        for let i = 0; i < obj2; i++:
            val = val + obj1;

        return new String(val);
    }

    // Parse division operations
    function _operatorDivision(obj1, obj2): {
        let val = "";

        const index = obj1.length / obj2.getValue();

        for let i = 0; i < index; i++:
            val = val + obj1.charAt(i);

        return new String(val);
    }

    // Parse '==' operations
    function _operatorEqual(obj1, obj2) => obj1.getValue() == obj2.getValue();
        

    // Parse '!=' operations
    function _operatorNotEqual(obj1, obj2) => obj1.getValue() != obj2.getValue();

    // Parse '>=' operations
    function _operatorGreaterOrEqual(obj1, obj2): 
        return obj1.length >= obj2.length;

    // Parse '<=' operations
    function _operatorLessOrEqual(obj1, obj2): 
        return obj1.length <= obj2.length;

    // Parse '>' operations
    function _operatorGreater(obj1, obj2): 
        return obj1.length > obj2.length;

    // Parse '<' operations
    function _operatorLess(obj1, obj2): 
        return obj1.length < obj2.length;

    function new(input):
        _value = if input is String ? input.getValue() : str(input);

    function new(<String> char, let<Number> count): { 
        count = std::math.round(count, 0);
        
        if count < 0: 
            std::throw.message("Param 'count' must be more than 0");

        if new String(char).length < 0:
            std::throw.message("Length of string must be more than 0");

        _value = #std.String.Create(char, count);
    }

    function getPrimitive(strOrPrimite, let<Boolean> allowOther = false): {
        if strOrPrimite is null:
            std::throw.message("Object is null");
        else if strOrPrimite is String:
            return strOrPrimite.toString();

        if isPrimitive(strOrPrimite):
            return strOrPrimite;

        if allowOther:
            return strOrPrimite;

        std::throw.message("Object is not string or primitive");
    }

    // Append two strings
    function append(<Array|Collection> collection): {
        let result = "";

        let arr = new Array(collection);

        for let i = 0; i < arr.length; i++:
            result = result + arr.at(i);

        return new String(result);
    }

    function toLower() => new String(#std.String.ToLower(_str(_value)));

    function toUpper() => new String(#std.String.ToUpper(_str(_value)));

    function isPrimitive(let value) => #std.String.IsPrimitive(value);

    function split(let<String> pattern) => new Array(#std.String.Split(_str(_value), _str(pattern)));

    function charAt(<Number> index): {
        throwIfNotInRange(index);

        return #std.String.At(_str(_value), index);
    }

    function setAt(<Number> index, <String> replaceValue): {
        throwIfNotInRange(index);

        _value = #std.String.SetAt(_str(_value), _str(replaceValue), index);
    }

    function join(<Collection|Array> strArr, let<String> pattern)
    => new String(#std.String.Join(strArr.getCollection(), pattern));

    // Get length of string
    const length = field(_): {
        fn get() => #std.String.Length(_str(_value))
    };

    // Check if string is empty or null
    function isNullOrEmpty(<String|null> str)
        => if str is null ? true : #std.String.IsNullOrEmpty(_str(str));
    
    // Check if string is white space or null
    function isNullOrWhitespace(<String|null> str)
        => if str is null ? true : #std.String.IsNullOrWhitespace(_str(str));

    function<Boolean> startsWith(<String> str) => #std.String.StartsWith(_str(_value), str);

    function<Boolean> endsWith(<String> str) => #std.String.EndsWith(_str(_value), str);

    // Trim string
    function<String> trim(<String> str) => new String(#std.String.Trim_B(_str(_value), _str(str)));

    // Trim start string
    function<String> trimStart(<String> str) => new String(#std.String.TrimStart_B(_str(_value), _str(str)));

    // Trim end string
    function<String> trimEnd(<String> str) => new String(#std.String.TrimEnd_B(_str(_value), _str(str)));

    // Trim string
    function<String> trim() => new String(#std.String.Trim(_str(_value)));

    // Trim start string
    function<String> trimStart() => new String(#std.String.TrimStart(_str(_value)));

    // Trim end string
    function<String> trimEnd() => new String(#std.String.TrimEnd(_str(_value)));

    // Cut string by 'startPos' and 'length'
    function<String> subString(let<Number> startPos, let<Number> len): {
        throwIfNotInRange(startPos);
        throwIfNotInRange(startPos + len);

        return new String(#std.String.SubString(_str(_value), startPos, len));
    }

    // get index of first 'toFind'
    function<Number> indexOf(<String> toFind) => #std.String.IndexOf(_value, toFind);

    // get index of last 'toFind'
    function<Number> lastIndexOf(<String> toFind) => #std.String.LastIndexOf(_value, toFind);

    // Replace '{n}' to replacement
    function<String> format(<Collection|Array> replacement): {
        if replacement.length == 0:
            return _value;

        return #std.String.Format(_value, 
            replacement.select(
                fn(x) => x.toString()
            ).toArray().getCollection());
    }

    private function throwIfNotInRange(<Number> index):
        if index < 0 || index > length:
            std::throw.message(`Index '{index}' is out of range.`);
}