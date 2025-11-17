include "@lib/throw"

// Class to make Math operations
class Math:
    // Function to throw exception variable is not number (internal)
    private function throwException(let num):
        if Number.isNumber(num) == false:
            Throw.exception("Parsing error: value \"" + num + "\" is not number")

    const let PI = 3.14159265359

    // Get abs of number
    function abs(let num):
        throwException(num)

        return 0 - num

    // Get result of sum of two numbers
    function sum(let num, let num2):
        throwException(num)
        throwException(num2)

        return num + num2

    // Get result substration of two numbers
    function sub(let num, let num2):
        throwException(num)
        throwException(num2)

        return num - num2
        
    // Get result of multiplication of two numbers
    function mult(let num, let num2):
        throwException(num)
        throwException(num2)

        return num * num2

    // Get result of division of two numbers
    function div(let num, let num2):
        throwException(num)
        throwException(num2)

        if num2 == 0:
            Throw.exception("Can't divide by 0")
        return num / num2