include "@lib/throw"

class Math:
    function sum(let<Number> num, let<Number> num2):
        return num + num2

    function sub(let<Number> num, let<Number> num2):
        return num - num2
        
    function mult(let<Number> num, let<Number> num2):
        return num * num2

    function div(let<Number> num, let<Number> num2):
        if num2 == 0:
            Throw.exception("Can't divide by 0")
        return num / num2