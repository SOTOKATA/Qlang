// Class to make operations with console
class Console: 
    private let usings = "using System; "
    private let defFColor = "gray"
    private let defBColor = "black"

    // Print text to console
    function print(let message): 
        _csharp(usings + "Console.Write(" + _str(message) + ")")
    
    // Print text to console with new line
    function println(let message):
        print(message + "\n")

    // Read line from console
    function readln():
        return _csharp(usings + "Console.ReadLine()")

    // Clear console
    function clear():
        _csharp(usings + "Console.Clear()")
    
    // Set cursor position in console
    function setCursorPosition(let x, let y):
        _csharp(usings + "Console.SetCursorPosition(" + x + "," + y + ")")

    // Set foreground color for console
    function setForeColor(let color):
        let line1 = "if (Enum.TryParse(" + _str(color) + ".ToString(), true, out ConsoleColor color))"
        let line2 = " Console.ForegroundColor = color;"
        _csharp(usings + line1 + line2)
    
    // Set background color for console
    function setBackColor(let color):
        let line1 = "if (Enum.TryParse(" + _str(color) + ".ToString(), true, out ConsoleColor color))"
        let line2 = " Console.BackgroundColor = color;"
        _csharp(usings + line1 + line2)

    // Set default colors for console
    function resetColors():
        setForeColor(defFColor)
        setBackColor(defBColor)