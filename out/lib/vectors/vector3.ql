import "$lib/standard"
import "$lib/core"

namespace vector:  {
    class Vector3: {
        private let _x;
        private let _y;
        private let _z;

        function toString(): {
            return String.new("X: " + _x + ", Y: " + _y + ", Z: " + _z);
        }

        function equals(let other): {
            if (other.X() == _x) && (other.Y() == _y) && (other.Z() == _z): {
                return true;
            }
            return false;
        }

        function _createFrom(const object): {
            if Number.isNumber(object):
                return Vector2.new(object, object);

            if typeof(object) == typeof(Vector3):
                return object;

            std::Throw.message("Undefined object");
        }

        function _operatorAddition(const obj1, const obj2): {
            return Vector3.new(obj1.X() + obj2.X(), obj1.Y() + obj2.Y(), obj1.Z() + obj2.Z());
        }

        function _operatorSubtraction(const obj1, const obj2):
            return Vector3.new(obj1.X() - obj2.X(), obj1.Y() - obj2.Y(), obj1.Z() - obj2.Z());

        function _operatorDivision(const obj1, const obj2):
            return Vector3.new(obj1.X() / obj2.X(), obj1.Y() / obj2.Y(), obj1.Z() / obj2.Z());

        function _operatorMultiplication(const obj1, const obj2):
            return Vector3.new(obj1.X() * obj2.X(), obj1.Y() * obj2.Y(), obj1.Z() * obj2.Z());

        function _operatorEqual(const obj1, const obj2):
            return obj1.X() == obj2.X() && obj1.Y() == obj2.Y() && obj1.Z() == obj2.Z(); 

        function _operatorNotEqual(const obj1, const obj2):
            return obj1.X() != obj2.X() && obj1.Y() != obj2.Y() && obj1.Z() != obj2.Z(); 

        function _operatorGreaterOrEqual(const obj1, const obj2):
            return obj1.X() >= obj2.X() && obj1.Y() >= obj2.Y() && obj1.Z() >= obj2.Z(); 

        function _operatorLessOrEqual(const obj1, const obj2):
            return obj1.X() <= obj2.X() && obj1.Y() <= obj2.Y() && obj1.Z() <= obj2.Z(); 

        function _operatorGreater(const obj1, const obj2):
            return obj1.X() > obj2.X() && obj1.Y() > obj2.Y() && obj1.Z() > obj2.Z(); 

        function _operatorLess(const obj1, const obj2):
            return obj1.X() < obj2.X() && obj1.Y() < obj2.Y() && obj1.Z() < obj2.Z();

        function new(const x, const y, const z): {
            _x = Parser.asNumber(x);
            _y = Parser.asNumber(y);
            _z = Parser.asNumber(z);
        }

        function X(): {
            return Parser.asNumber(_x);
        }

        function Y(): {
            return Parser.asNumber(_y);
        }

        function Y(): {
            return Parser.asNumber(_z);
        }
    }
}