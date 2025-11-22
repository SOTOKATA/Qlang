include "@lib/throw"

// Class to make string operations
class String:
    private let _value = ""

    function new(let input):
        _value = input

    function toString():
        return _value

    // Get new c# string (internal)
    private function getStr(let str):
        return "new string(" + _str(str) + ")"

    // Append two strings
    function append(let str):
        return _value + str

    // Get length of string
    function length():
        return _csharp(getStr(_value) + ".Length")

    // Check if string is empty or null
    function isNullOrEmpty(let str):
        return _csharp("string.IsNullOrEmpty(" + _str(str) + ")")

    // Check if string is white space or null
    function isNullOrWhiteSpace(let str):
        return _csharp("string.IsNullOrWhiteSpace(" + _str(str) + ")")

    // Trim string
    function trim():
        return _csharp(getStr(str) + ".Trim()")

    // Trim start string
    function trimStart():
        return _csharp(getStr(_value) + ".TrimStart()")

    // Trim end string
    function trimEnd():
        return _csharp(getStr(_value) + ".TrimEnd()")

    // Cut string by 'startPos' and 'length'
    function subString(let startPos, let length):
        if Number.isNumber(startPos) == false:
            Throw.exception("subString error: startPos must be number")

        if Number.isNumber(length) == false:
            Throw.exception("subString error: length must be number")

        return _csharp(getStr(_value) + ".Substring(" + startPos + "," + length + ")")