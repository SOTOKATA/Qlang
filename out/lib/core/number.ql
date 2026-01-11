include "$lib/core"

// Full static class
// Class to make operations with numbers
class Number: {
    // used string because of number structure with E+
    const MIN_VALUE = "-1.7976931348623157E+308";
    const MAX_VALUE = "1.7976931348623157E+308";

    // check if 'var' is numbers
    function isNumber(let var): {
        return _native("lib.number.try_parse", var);
    }


    // get random number with range 'min' to 'max'
    function randInt(let min, let max): {
        if (isNumber(min) == false) || (isNumber(max) == false): {
            Throw.exception("One or two input is not a number");
        }

        // Convert '3.42...' to '3'
        min = Parser.asInt(min);
        max = Parser.asInt(max);

        if min >= max: {
            Throw.exception("Minimum can't be more than maximum");
        }

        return Parser.asNumber(_native("lib.number.random", min, max));
    }

    // Get int styled number ('3.421' to '3')
    function toFixedInt(let float): {
        return toFixed(float, "0");
    }

    // Change numeric style (ex.: '3.214' to '3.2' with pattern '0.0')
    function toFixed(let number, let pattern): {
        return _native("lib.number.to_string", number, _str(pattern));
    }
}