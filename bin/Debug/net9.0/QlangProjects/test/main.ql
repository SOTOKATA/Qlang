include "$lib/base"
include "$lib/special/basetest"

function main(): {
    Console.clear();

    let screen = PDCurses.new(30, Console.height(), "Message log:", ".");

    screen.addStr(1, 1, "> Alexa: Hello World!");
    screen.addStr(1, 2, "> You: Hello World!");
    screen.addStr(1, 3, "> Alexa: Hello World!");
    screen.addStr(1, 4, "> You: Hello World!");

    screen.refresh();

    /// ACTOR
    screen = PDCurses.new(70, Console.height() / 1.5, "Actor:", " ");
    screen.setPosition(29, 0);

    const arr = File.getContent("image.txt").split("\n");

    for let i = 0; i < arr.length(); i = i + 1:
        screen.addStr(0, i, String.new(arr.at(i)).trim());

    screen.refresh();

    /// HELP/OTHER
    screen = PDCurses.new(37, Console.height(), "FUCK AND TREAT v.1.53.2", ".");
    screen.setPosition(98, 0);

    screen.addStr(1, 1, "CTRL+");
    screen.addStr(1, 2, "1. Save");
    screen.addStr(1, 3, "2. Load");
    screen.addStr(1, 4, "2. Cheat");
    screen.addStr(1, 5, "3. Exit");

    screen.refresh();

    screen = PDCurses.new(70, Console.height()  + 1 - Parser.asInt(Console.height() / 1.5), "Actions:", ".");
    screen.setPosition(29, Parser.asInt(Console.height() / 1.5) - 1);

    screen.addStr(2, 1, "1. [SHOP] And need to buy something.");
    screen.addStr(2, 2, "2. [ATTACK] I will kill you!");
    screen.addStr(2, 3, "3. [SEX] I will fuck you!");
    screen.addStr(2, 4, "4. [EXIT] Good bye.");

    screen.refresh();

    Console.readkey();
}

class PDCurses: {
    private let _width;
    private let _height;
    private let _x = 0;
    private let _y = 0;
    private let _name;
    private let _backgroundChar = " ";

    private let _screen;

    let drawWindow = true;

    function new(const width = Console.width(), const height = Console.height(), const name = "Window", const backChar = " "): {
        if width < 2 || height < 2:
            Throw.exception("Size must be more than or equal to (0, 0)");

        _width = width;
        _height = height;
        _name = name;
        _backgroundChar = backChar;

        _screen = Array.new([]);
        for let y = 0; y < _height; y = y + 1:
            _screen.push(String.new(_backgroundChar, _width));
    }

    private function getRelativeX(const x): 
        return x + _x;

    private function getRelativeY(const y): 
        return y + _y;

    function getWidth():
        return _width;

    function getHeight():
        return _height;

    function setPosition(const x, const y): {
        if x < 0 || x >= Console.width() || x + _width > Console.width():
            Throw.exception("X position is not valid");

        if y < 0 || y >= Console.width() || y + _height > Console.height():
            Throw.exception("Y position is not valid");

        _x = x;
        _y = y;
    }

    // function setBackgroundChar(let str): {
    //     str = String.new(str);
    //     if str.length() == 0:
    //         Throw.exception("String must be more than 0");

    //     _backgroundChar = str.charAt(0);
    // }

    function setName(const name):
        _name = name;
    
    function addStr(const x, const y, let str): {
        if String.isString(str) == false:
            str = String.new(str);

        if y < 0 || y >= _height:
            Throw.exception("Y must be less then screen height and more than 0");

        const length = str.length();
        if x < 0 || x + length >= _width:
            Throw.exception("X must be less then screen width and more than 0");

        const line = _screen.at(y);
        for let l = 0; l < length; l = l + 1:
            line.setAt(x + l, str.charAt(l));
    }

    function refresh(): {
        Console.cursorVisible(true);    

        for let x = 0; x < _screen.length(); x = x + 1: {
            Console.setCursorPosition(getRelativeX(0), getRelativeY(x));
            Console.print(_screen.at(x));
        }

        if drawWindow == false:
            return;

        Console.setCursorPosition(getRelativeX(0), getRelativeY(0));
        Console.print("+");    
        Console.print(String.new("-", _width - 2));    
        Console.println("+");

        Console.setCursorPosition(getRelativeX(0) + 0, getRelativeY(0) + _height - 1);
        Console.print("+");    
        Console.print(String.new("-", _width - 2));    
        Console.print("+");

        for let i = 1; i < _height - 1; i = i + 1: {
            Console.setCursorPosition(getRelativeX(0), getRelativeY(0) + i);
            Console.print("|"); 
            Console.setCursorPosition(getRelativeX(0) + _width - 1, getRelativeY(0) + i);
            Console.print("|");
        }
           
        if _width <= String.new(_name).length() + 2:
            return;

        Console.setCursorPosition(getRelativeX(2), getRelativeY(0));
        Console.print(_name);
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