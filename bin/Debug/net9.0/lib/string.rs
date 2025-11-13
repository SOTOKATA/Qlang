class String:
    $value = ""

    function setValue($str):
        $value = $str

    function getValue():
        return $value

    function append($str2):
        $value = $value + $str2

    function getLength($str1):
        return csharp("string.getlength=" + $value)