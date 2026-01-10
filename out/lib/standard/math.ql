include "$lib/standard"
include "$lib/core"

namespace std:  {
    class Math: {
        function max(const<core::Number> a, const<core::Number> b): {
            if a > b:
                return a;
            return b;
        }

        function min(const<core::Number> a, const<core::Number> b): {
            if a < b:
                return a;
            return b;
        }

        function abs(const<core::Number> n): {
            return 0-n;
        }
    }
}