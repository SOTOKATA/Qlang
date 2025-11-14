include "@lib/console"

class Number:
    function getMinValue():
        return csharp("number.getminvalue")

    function getMaxValue():
        return csharp("number.getmaxvalue")

    function isNumber(let var):
        return csharp("number.isnumber=" + var)