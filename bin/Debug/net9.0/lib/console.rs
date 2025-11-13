class Console: 
    $defFColor = "gray"
    $defBColor = "black"

    function print($message): 
        csharp("term.print=" + $message)
    
    function println($message):
        print($message + "\n")

    function readln():
        return csharp("term.read")

    function setForeColor($color):
        csharp("term.fcolor=" + $color)
    
    function setBackColor($color):
        csharp("term.bcolor=" + $color)

    function resetColors():
        setForeColor($defFColor)
        setBackColor($defBColor)