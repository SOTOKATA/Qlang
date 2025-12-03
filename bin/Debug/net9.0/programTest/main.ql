include "$lib/base"
include "$lib/special/snake"

function main(): {
    SnakeGame.run();
    // let string = "Hello, World!";
    // let strClass = String.new(string);

    // // will call exception
    // // Because 'string' is primitive type
    // string.length();
   

    // // will not call exception
    // // Because 'strClass' is String class instance
    // strClass.length();
}