include "@lib/throw"

class Math:
    function throwException(let num):
        if Number.isNumber(num) == false:
            Throw.exception("Parsing error: value \"" + num + "\" is not number")

    function getPI():
        return 3.14159265359

    function abs(let num):
        throwException(num)
        return 0 - num

    function sum(let num, let num2):
        throwException(num)
        return num + num2

    function sub(let num, let num2):
        throwException(num)
        return num - num2
        
    function mult(let num, let num2):
        throwException(num)
        return num * num2

    function div(let num, let num2):
        throwException(num)
        throwException(num2)
        if num2 == 0:
            Throw.exception("Can't divide by 0")
        return num / num2