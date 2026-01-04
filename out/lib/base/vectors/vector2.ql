include "$lib/base"
include "$lib/core"

class Vector2: {
    private let _x;
    private let _y;

    function toString(): {
        return String.new("X: " + _x + ", Y: " + _y);
    }

    function equals(let other): {
        if (other.X() == _x) && (other.Y() == _y): {
            return true;
        }
        return false;
    }

    function new(let x, let y): {
        _x = Parser.asNumber(x);
        _y = Parser.asNumber(y);
    }

    function X(): {
        return Parser.asNumber(_x);
    }

    function Y(): {
        return Parser.asNumber(_y);
    }
}