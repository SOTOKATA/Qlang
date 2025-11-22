include "@lib/datatypes/advanced/vector2"
include "@lib/console"
include "@lib/datatypes/string"

function main():
    let vect = Vector2.new()

    // String (class)
    let name = String.new("Hello World!")

    Console.println(name)
    Console.println(name.length())
    
    vect.set(1, 1)

    // String (value)
    Console.println("dynamic: " + vect.toString())
    Console.println("static: " + Vector2.toString())