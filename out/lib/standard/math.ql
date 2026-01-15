include "$lib/standard"
include "$lib/core"

namespace std:  {
    class Math: {
        const PI = _native("std.math.pi");
        const E = _native("std.math.e");
        const TAU = _native("std.math.tau");

        function max(const<Number> a, const<Number> b):
            return boolCase(a > b, a, b);

        function min(const<Number> a, const<Number> b):
            return boolCase(a < b, a, b);

        function abs(const<Number> n): {
            return 0-n;
        }
        
        function pow(const number, const pow):
            return _native("std.math.pow", number, pow);

        function sin(const<Number> radians):
            return _native("std.math.sin", radians);
 
        function cos(const<Number> radians):
            return _native("std.math.cos", radians);

        function tan(const<Number> radians): {
            const cos = cos(radians);

            if cos == 0:
                Throw.exception("Cos value cannot be zero!");

            return sin(radians) / cos;
        }
    
        function ctan(const<Number> radians): {
            const sin = sin(radians);

            if sin == 0:
                Throw.exception("Sin value cannot be zero!");
        
            return cos(radians) / sin;
        }
    }
}