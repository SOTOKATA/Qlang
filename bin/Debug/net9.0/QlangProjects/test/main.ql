include "$lib/base"

function main(): {
    Console.println("Hello World!");
}
// class Window: {
//     private let _width;
//     private let _height;

//     private let _name;

//     function new(const width, const height, const name): {
//         _width = width;
//         _height = height;
//         _name = name;
//     }

//     function create(const width, const height, const name): {
//         if width <= 0 || height <= 0 || String.isNullOrWhitespace(name) == true:
//             Throw.exception("Window params is not valid");    

//         _native("ui.window.create", name);

//         return Window.new(width, height, name);
//     }

//     function destroy(const window): {
//         if isWindow(window) == false:
//             Throw.exception("Param is not a Window class");

//         _native("ui.window.destroy", window);
//     }

//     function isWindow(const window = this): 
//         _native("ui.window.is_window", window);
// }