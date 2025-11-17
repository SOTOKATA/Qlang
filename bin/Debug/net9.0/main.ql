include "@lib/datatypes"

function main():
    let array = Array.empty()

    array = Array.append(array, "Hello World!")

    Console.println(Array.at(array, 0))