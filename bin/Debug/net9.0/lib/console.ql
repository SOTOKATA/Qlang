class Console: 
    let<String> defFColor = "gray"
    let<String> defBColor = "black"

    function print(let<String> message): 
        csharp("term.print=" + message)
    
    function println(let<String> message):
        print(message + "\n")

    function readln():
        return csharp("term.read")

    function setForeColor(let<String> color):
        csharp("term.fcolor=" + color)
    
    function setBackColor(let<String> color):
        csharp("term.bcolor=" + color)

    function resetColors():
        setForeColor(defFColor)
        setBackColor(defBColor)