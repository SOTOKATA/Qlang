include "$lib/base"

class Math: {
    function max(const<Number> a, const<Number> b): {
        if a > b:
            return a;
        return b;
    }

    function min(const<Number> a, const<Number> b): {
        if a < b:
            return a;
        return b;
    }

    function abs(const<Number> n): {
        return 0-n;
    }
}