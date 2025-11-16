class Console: 
    let usings = "using System; "
    let defFColor = "gray"
    let defBColor = "black"

    function print(let message): 
        csharp(usings + "Console.Write(\"" + message + "\")")
    
    function println(let message):
        print(message + "\n")

    function readln():
        return csharp(usings + "Console.ReadLine()")

    function clear():
        csharp(usings + "Console.Clear()")
    
    function setCursorPosition(let x, let y):
        csharp(usings + "Console.SetCursorPosition(" + x + "," + y + ")")

    function setForeColor(let color):
        let line1 = "if (Enum.TryParse(\"" + color + "\".ToString(), true, out ConsoleColor color))"
        let line2 = " Console.ForegroundColor = color;"
        csharp(usings + line1 + line2)
    
    function setBackColor(let color):
        let line1 = "if (Enum.TryParse(\"" + color + "\".ToString(), true, out ConsoleColor color))"
        let line2 = " Console.BackgroundColor = color;"
        csharp(usings + line1 + line2)

    function resetColors():
        setForeColor(defFColor)
        setBackColor(defBColor)