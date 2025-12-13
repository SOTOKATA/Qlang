include "$lib/base"
include "$lib/special/basetest"

function main(): {
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