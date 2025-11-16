include "@lib/console"
include "@lib/throw"

class Number:
    function getUsings():
        return usings

    private let usings = "using System; using System.Globalization; "
        
    function getMinValue():
        return csharp(usings + "double.MinValue.ToString(CultureInfo.InvariantCulture)")

    function getMaxValue():
        return csharp(usings + "double.MaxValue.ToString(CultureInfo.InvariantCulture)")

    function isNumber(let var):
        return csharp(usings + "double.TryParse(\"" + var + "\", NumberStyles.Float, CultureInfo.InvariantCulture, out var _)")

    function randInt(let min, let max):
        if min >= max:
            Throw.exception("Minimum can't be more than maximum")
        return csharp(usings + "new Random().Next(" + min + "," + max + ")")

    function toFixed(let number, let pattern):
        return csharp(usings + number + ".ToString(\"" + pattern + "\")")