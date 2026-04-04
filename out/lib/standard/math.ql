import "$lib/standard"
import "$lib/core"

namespace std:  {
    const math = new Math();
    private class Math: {
        const MIN_VALUE = #std.Math.MinValue();
        const MAX_VALUE = #std.Math.MaxValue();
        const PI = #std.Math.PI();
        const E = #std.Math.E();
        const TAU = #std.Math.Tau();

        function<Number> max(<Number> a, <Number> b) => if a > b ? a : b;

        function<Number> min(<Number> a, <Number> b) => if a < b ? a : b;

        function<Number> rand(<Number> min, <Number> max): {
            if min >= max:
                throw.message("Min cannot be more than or equal to max.");

            return #std.Math.Random(min, max);
        }

        function<Number> abs(<Number> n) => 0-n;
        
        function<Number> pow(number, pow) => #std.Math.Pow(number, pow);

        function<Number> sin(<Number> radians):
            return #std.Math.Sin(radians);
 
        function<Number> cos(<Number> radians) => #std.Math.Cos(radians);

        function<Number> tan(<Number> radians): {
            const cos = cos(radians);

            if cos == 0:
                std::throw.message("Cos value cannot be zero!");

            return sin(radians) / cos;
        }
    
        function<Number> ctan(<Number> radians): {
            const sin = sin(radians);

            if sin == 0:
                std::throw.message("Sin value cannot be zero!");
        
            return cos(radians) / sin;
        }

        function round(<Number> value, <Number> afterDot = 0)
            => #std.Math.Round(value, afterDot);
    }
}