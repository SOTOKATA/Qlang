import "$lib/core"

// Full static class
// Class to make operations with numbers
class Number: {
    // used string because of number structure with E+
    const MIN_VALUE = "-1.7976931348623157E+308";
    const MAX_VALUE = "1.7976931348623157E+308";

    // check if 'var' is number
    function isNumber(let var): {
        return _native("std.number.try_parse", var);
    }

    // Get int styled number ('3.421' to '3')
    function toFixedInt(let float): {
        return toFixed(float, "0");
    }

    // Change numeric style (ex.: '3.214' to '3.2' with pattern '0.0')
    function toFixed(let number, let pattern): {
        return _native("std.number.to_string", number, _str(pattern));
    }
}