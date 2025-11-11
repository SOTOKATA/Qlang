#csharp
use exception
class Math:
    $MIN_VALUE=csharp("int.param.minvalue")
    $MAX_VALUE=csharp("int.param.maxvalue")

    static function isFloat($num1, $num2):
        if float.isFloat($num1) == false:
            throw exception("Argument " + $num1 + " is not a number")

        if float.isFloat($num2) == false:
            throw exception("Argument " + $num2 + " is not a number")

        return true

    static function sum($num1, $num2):
        if isFloat($num1, $num2) == false:
            return

        return $num1 + $num2
    
    static function sub($num1, $num2):
        if isFloat($num1, $num2) == false:
            return

        return $num1 - $num2
    
    static function mult($num1, $num2):
        if isFloat($num1, $num2) == false:
            return

        return $num1 * $num2

    static function div($num1, $num2):
        if isFloat($num1, $num2) == false:
            return

        if $num2 == 0:
            throw exception("Can't divide by 0")

        return $num1 / $num2