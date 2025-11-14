include "@lib/console"

class Number:
    let value = 0

    function getMinValue():
        return csharp("number.getminvalue")

    function getMaxValue():
        return csharp("number.getmaxvalue")

    function isNumber(let<Number> var):
        let<String> method = "number.isnumber=" + var
        Console.println("call_method: " + method)
        return csharp(method)