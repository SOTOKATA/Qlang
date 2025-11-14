include "@lib/throw"

class Math:
    function sum(let num, let num2):
        return num + num2

    function sub(let num, let num2):
        return num - num2
        
    function mult(let num, let num2):
        return num * num2

    function div(let num, let num2):
        if num2 == 0:
            Throw.exception("Can't divide by 0")
        return num / num2