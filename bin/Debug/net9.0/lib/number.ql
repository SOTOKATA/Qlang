class Number:
    function getMinValue():
        return csharp("number.getminvalue")

    function getMaxValue():
        return csharp("number.getmaxvalue")

    function isNumber($var):
        return csharp("number.isnumber=" + $var)