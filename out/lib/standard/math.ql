import "$lib/standard"
import "$lib/core"

namespace std:  {
    class Math: {
        const MIN_VALUE = _native("std", "math", "min_value");
        const MAX_VALUE = _native("std", "math", "max_value");
        const PI = _native("std", "math", "pi");
        const E = _native("std", "math", "e");
        const TAU = _native("std", "math", "tau");

        function<Number> max(const<Number> a, const<Number> b):
            return boolCase(a > b, a, b);

        function<Number> min(const<Number> a, const<Number> b):
            return boolCase(a < b, a, b);

        function<Number> rand(const<Number> min, const<Number> max): {
            if min >= max:
                Throw.message("Min cannot be more than or equal to max.");

            return _native("std", "math", "random", min, max);
        }

        function<Number> abs(const<Number> n): {
            return 0-n;
        }
        
        function<Number> pow(const number, const pow):
            return _native("std", "math", "pow", number, pow);

        function<Number> sin(const<Number> radians):
            return _native("std", "math", "sin", radians);
 
        function<Number> cos(const<Number> radians):
            return _native("std", "math", "cos", radians);

        function<Number> tan(const<Number> radians): {
            const cos = cos(radians);

            if cos == 0:
                std::Throw.message("Cos value cannot be zero!");

            return sin(radians) / cos;
        }
    
        function<Number> ctan(const<Number> radians): {
            const sin = sin(radians);

            if sin == 0:
                std::Throw.message("Sin value cannot be zero!");
        
            return cos(radians) / sin;
        }

        function round(const<Number> value, const<Number> afterDot = 0):
            return _native("std", "math", "round", value, afterDot);
    }
}