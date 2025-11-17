include "@lib/throw"

// Class to make operations with numbers
class Number:
    // Basic c# usings for this class
    private let usings = "using System; using System.Globalization; "

    // used string because of number structure with E+
    const let MIN_VALUE = "-1.7976931348623157E+308"
    const let MAX_VALUE = "1.7976931348623157E+308"

    // check if 'var' is number
    function isNumber(let var):
        return _csharp(usings + "double.TryParse(" + _str(var) + ", NumberStyles.Float, CultureInfo.InvariantCulture, out var _)")

    // get random number with range 'min' to 'max'
    function randInt(let min, let max):
        if min >= max:
            Throw.exception("Minimum can't be more than maximum")

        // Convert '3.42...' to '3'
        min = toInt(min)
        max = toInt(max)

        return _csharp(usings + "new Random().Next(" + min + "," + max + ")")

    // Get int styled number ('3.421' to '3')
    function toInt(let float):
        return toFixed(float, "0")

    // Change numeric style (ex.: '3.214' to '3.2' with pattern '0.0')
    function toFixed(let number, let pattern):
        return _csharp(usings + number + ".ToString(" + _str(pattern) + ")")