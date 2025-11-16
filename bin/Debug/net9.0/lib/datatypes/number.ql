class Number:
    let usings = "using System; using System.Globalization; "

    function getMinValue():
        return csharp(usings + "double.MinValue.ToString(CultureInfo.InvariantCulture)")

    function getMaxValue():
        return csharp(usings + "double.MaxValue.ToString(CultureInfo.InvariantCulture)")

    function isNumber(let var):
        return csharp(usings + "double.TryParse(\"" + var + "\", NumberStyles.Float, CultureInfo.InvariantCulture, out var _)")