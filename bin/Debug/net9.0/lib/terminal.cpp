#csharp
class Console:
    static function print($message):
        csharp("console.func.print=" + $color)
        
    static function println($message):
        print($message + "\n")

    static function readln():
        return csharp("console.func.read")
    static function setForeColor($color):
        csharp("console.param.fcolor=" + $color)

    static function setBackColor($color):
        csharp("console.param.bcolor=" + $color)
        
    static function resetColor():
        csharp("console.func.resetcolor")