include "$lib/standard"
include "$lib/core"

namespace std:  {
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

        function ___create_from___(const object): {
            _native("std.console.write", str("type = " + object.toString()));
            if (Number.isNumber(object)):
                return Vector2.new(object, object);

            if (typeof(object) == typeof(Vector2)):
                return object;

            Throw.exception("Undefined object");
        }

        function ___operator_plus___(const obj1, const obj2): {
            return Vector2.new(obj1.X() + obj2.X(), obj1.Y() + obj2.Y());
        }

        function ___operator_minus___(const obj1, const obj2):
            return Vector2.new(obj1.X() - obj2.X(), obj1.Y() - obj2.Y());

        function ___operator_slash___(const obj1, const obj2):
            return Vector2.new(obj1.X() / obj2.X(), obj1.Y() / obj2.Y());

        function ___operator_star___(const obj1, const obj2):
            return Vector2.new(obj1.X() * obj2.X(), obj1.Y() * obj2.Y());

        function ___operator_equal_equal___(const obj1, const obj2):
            return obj1.X() == obj2.X() && obj1.Y() == obj2.Y(); 

        function ___operator_not_equal___(const obj1, const obj2):
            return obj1.X() != obj2.X() && obj1.Y() != obj2.Y(); 

        function ___operator_greater_equal___(const obj1, const obj2):
            return obj1.X() >= obj2.X() && obj1.Y() >= obj2.Y(); 

        function ___operator_less_equal___(const obj1, const obj2):
            return obj1.X() <= obj2.X() && obj1.Y() <= obj2.Y(); 

        function ___operator_greater___(const obj1, const obj2):
            return obj1.X() > obj2.X() && obj1.Y() > obj2.Y(); 

        function ___operator_less___(const obj1, const obj2):
            return obj1.X() < obj2.X() && obj1.Y() < obj2.Y(); 

        function ___operator_equal_equal___(const obj1, const obj2):
            return obj1.X() == obj2.X() && obj1.Y() == obj2.Y(); 

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
}