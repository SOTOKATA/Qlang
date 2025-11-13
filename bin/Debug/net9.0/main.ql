include "@lib/console"
include "@lib/throw"
include "@lib/number"

function main():
    Console.print("Write your name: ")

    $name = Console.readln()

    Console.println("Hello, " + $name + "!")

    $number1 = "ds"

    while Number.isNumber($number1) == true: 
        Console.println("Write first number: ")
        $number1 = Console.readln()


    $number2 = ""

    do_while Number.isNumber($number2) == false: 
        Console.println("Write second number: ")
        $number2 = Console.readln()

    Console.println("Result of division: " + div($number1, $number2))

function div($num1, $num2):
    return Mathematic.div($num1, $num2)

class Mathematic:
    function div($num1, $num2):
        if $num2 == 0:
            Throw.exception("Can't divide by 0!")

        return $num1 / $num2
