include "@lib/throw"

// Class to make string operations
class String:
    // Get new c# string (internal)
    private function getStr(let str):
        return "new string(" + _str(str) + ")"

    // Append two strings
    function append(let str, let str2):
        return str + str2

    // Get length of string
    function getLength(let str):
        return _csharp(getStr(str) + ".Length")

    // Check if string is empty or null
    function isNullOrEmpty(let str):
        return _csharp("string.IsNullOrEmpty(" + _str(str) + ")")

    // Check if string is white space or null
    function isNullOrWhiteSpace(let str):
        return _csharp("string.IsNullOrWhiteSpace(" + _str(str) + ")")

    // Trim string
    function trim(let str):
        return _csharp(getStr(str) + ".Trim()")

    // Trim start string
    function trimStart(let str):
        return _csharp(getStr(str) + ".TrimStart()")

    // Trim end string
    function trimEnd(let str):
        return _csharp(getStr(str) + ".TrimEnd()")

    // Cut string by 'startPos' and 'length'
    function subString(let str, let startPos, let length):
        if Number.isNumber(startPos) == false:
            Throw.exception("subString error: startPos must be number")

        if Number.isNumber(length) == false:
            Throw.exception("subString error: length must be number")

        return _csharp(getStr(str) + ".Substring(" + startPos + "," + length + ")")