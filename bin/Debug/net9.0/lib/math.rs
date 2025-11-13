class Math:
    function sum($num, $num2):
        return $num + $num2

    function sub($num, $num2):
        return $num - $num2
        
    function mult($num, $num2):
        return $num * $num2

    function div($num, $num2):
        if $num2 == 0:
            Throw.exception("Can't divide by 0")
        return $num / $num2