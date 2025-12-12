include "$lib/base"
// native "D:/Projects/Console/Qlang/UISupport/bin/Debug/net9.0/"

function main(): {
    const screen = PDCurses.new();

    screen.addstr(1, 5, "Hello World!");

    screen.refresh();
}

class PDCurses: {
    private const _width = Console.width();
    private const _height = Console.height();
    private let _screen;

    function new(): {
        _screen = Array.new([]);
        for let y = 0; y < _width; y = y + 1: { 
            _screen.push(Array.new([]));

            for let x = 0; x < _height; x = x + 1:
                _screen.at(y).push(" ");
        }
    }
    
    function addstr(const x, const y, let str): {
        if String.isString(str) == false:
            str = String.new(str);

        if x < 0 || (x + str.length()) > _width:
            Throw.exception("X must be less then " + _width + " and more than 0");

        if y < 0 || y > _height:
            Throw.exception("Y must be less then " + _height + " and more than 0");

        for let l = 0; l < str.length(); l = l + 1:
            _screen.at(x + l).setAt(y, str.charAt(l));
            // Console.println(str.charAt(l));

            //_screen.at(x + l).setAt(y, str.charAt(l));
    }

    function refresh(): {
        Console.clear();
        for let x = 0; x < _width; x = x + 1:
           Console.print(String.join("", _screen.at(x)));

    }
}

class Window: {
    private let _items;

    function new(): {
        _items = Array.new([]);
    }

    function addstr(const x, const y, const str): {
        
    }

    function draw(): {

    }
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