include "$lib/base"

class Math: {
    function max(let a, let b): {
        if a > b: {
            return a;
        }

        return b;
    }

    function min(let a, let b): {
        if a < b: {
            return a;
        }

        return b;
    }
}