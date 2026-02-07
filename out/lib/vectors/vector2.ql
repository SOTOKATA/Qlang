import "$lib/standard"
import "$lib/core"

namespace vector:  {
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

        function _createFrom(const object): {
            if typeof(object) == "Number":
                return Vector2.new(object, object);

            if typeof(object) == typeof(Vector2):
                return object;

            std::Throw.message("Undefined object");
        }

        function _operatorAddition(const obj1, const obj2): {
            return Vector2.new(obj1.X() + obj2.X(), obj1.Y() + obj2.Y());
        }

        function _operatorSubtraction(const obj1, const obj2):
            return Vector2.new(obj1.X() - obj2.X(), obj1.Y() - obj2.Y());

        function _operatorDivision(const obj1, const obj2):
            return Vector2.new(obj1.X() / obj2.X(), obj1.Y() / obj2.Y());

        function _operatorMultiplication(const obj1, const obj2):
            return Vector2.new(obj1.X() * obj2.X(), obj1.Y() * obj2.Y());

        function _operatorEqual(const obj1, const obj2):
            return obj1.X() == obj2.X() && obj1.Y() == obj2.Y(); 

        function _operatorNotEqual(const obj1, const obj2):
            return obj1.X() != obj2.X() && obj1.Y() != obj2.Y(); 

        function _operatorGreaterOrEqual(const obj1, const obj2):
            return obj1.X() >= obj2.X() && obj1.Y() >= obj2.Y(); 

        function _operatorLessOrEqual(const obj1, const obj2):
            return obj1.X() <= obj2.X() && obj1.Y() <= obj2.Y(); 

        function _operatorGreater(const obj1, const obj2):
            return obj1.X() > obj2.X() && obj1.Y() > obj2.Y(); 

        function _operatorLess(const obj1, const obj2):
            return obj1.X() < obj2.X() && obj1.Y() < obj2.Y(); 

        function _operatorEqual(const obj1, const obj2):
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