include "$lib/core"
include "$lib/base"
include "$lib/gui"

function main(): {
    Console.println("Hello, World!");
    
    Window.create(100, 100, "ews");

    while (Window.shouldClose() == false):
    {}
}