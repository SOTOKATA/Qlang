class String:
    // private
    function getStr(let str):
        return "new string(\"" + str + "\")"

    function append(let str, let str2):
        return str + str2

    function getLength(let str):
        return csharp(getStr(str) + ".Length")

    function isNullOrEmty(let str):
        return csharp("string.IsNullOrEmpty(\"" + str + "\")")

    function isNullOrWhiteSpace(let str):
        return csharp("string.IsNullOrWhiteSpace(\"" + str + "\")")

    function trim(let str):
        return csharp(getStr(str) + ".Trim();")

    function trimStart(let str):
        return csharp(getStr(str) + ".TrimStart();")

    function trimEnd(let str):
        return csharp(getStr(str) + ".TrimEnd();")

    function subString(let str, let startPos, let length):
        return csharp(getStr(str) + ".Substring(" + startPos + "," + length + ")")