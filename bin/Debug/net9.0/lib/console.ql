class Console: 
    let defFColor = "gray"
    let defBColor = "black"

    function print(let message): 
        csharp("term.print=" + message)
    
    function println(let message):
        print(message + "\n")

    function readln():
        return csharp("term.read")

    function setForeColor(let color):
        csharp("term.fcolor=" + color)
    
    function setBackColor(let color):
        csharp("term.bcolor=" + color)

    function resetColors():
        setForeColor(defFColor)
        setBackColor(defBColor)