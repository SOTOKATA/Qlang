class String:
    let value = ""

    function setValue(let<String> str):
        value = str

    function getValue():
        return value

    function append(let<String> str2):
        value = value + str2

    function getLength():
        return csharp("string.getlength=" + value)