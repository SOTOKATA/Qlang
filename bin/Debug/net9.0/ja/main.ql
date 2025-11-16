include "@lib/console"
function main():
    Console.clear()
    Console.println("Hello World!")
    let name = Console.readln()
    Console.println(name)


// include "@lib/console"
// include "@lib/throw"
// include "@lib/datatypes/number"

// function main():
//     Console.print("Write your name: ")

//     let name = Console.readln()

//     Console.println("Hello, " + name + "!")

//     let number1

//     do_while Number.isNumber(number1) == false: 
//         Console.println("Write first number: ")
//         number1 = Console.readln()

//         if Number.isNumber(number1) == false:
//             Console.println("Your input is not number!")

//     let number2

//     do_while Number.isNumber(number2) == false: 
//         Console.println("Write second number: ")
//         number2 = Console.readln()

//         if Number.isNumber(number2) == false:
//             Console.println("Your input is not number!")

//     Console.println("Result of division: " + div(number1, number2))

// function div(let num1, let num2):
//     return Mathematic.div(num1, num2)

// class Mathematic:
//     function div(let num1, let num2):
//         if num2 == 0:
//             Throw.exception("Can't divide by 0!")

//         return num1 / num2
