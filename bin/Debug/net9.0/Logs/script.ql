include "@lib/base"
include "@lib/special/demo"

function main(): {
    let arr = Array.new(["Hello World!", 1]);
    
    for let i = 0; i < 2; i = i + 1: {
        Console.println(arr.at(i));
    }
    Console.println(String.append(["Hello W", "orld", "!"]));
}
