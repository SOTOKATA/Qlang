import "$lib/standard"
import "$lib/core"

namespace std:  {
    class Math: {
        const MIN_VALUE = _native("std", "math", "min_value");
        const MAX_VALUE = _native("std", "math", "max_value");
        const PI = _native("std", "math", "pi");
        const E = _native("std", "math", "e");
        const TAU = _native("std", "math", "tau");

        function max(const<Number> a, const<Number> b):
            return boolCase(a > b, a, b);

        function min(const<Number> a, const<Number> b):
            return boolCase(a < b, a, b);

        function rand(const<Number> min, const<Number> max): {
            if min >= max:
                Throw.message("Min cannot be more than or equal to max.");

            return _native("std", "math", "random", min, max);
        }

        function abs(const<Number> n): {
            return 0-n;
        }
        
        function pow(const number, const pow):
            return _native("std", "math", "pow", number, pow);

        function sin(const<Number> radians):
            return _native("std", "math", "sin", radians);
 
        function cos(const<Number> radians):
            return _native("std", "math", "cos", radians);

        function tan(const<Number> radians): {
            const cos = cos(radians);

            if cos == 0:
                std::Throw.message("Cos value cannot be zero!");

            return sin(radians) / cos;
        }
    
        function ctan(const<Number> radians): {
            const sin = sin(radians);

            if sin == 0:
                std::Throw.message("Sin value cannot be zero!");
        
            return cos(radians) / sin;
        }
    }
}