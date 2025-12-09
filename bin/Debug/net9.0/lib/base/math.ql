include "$lib/base"

class Math: {
    function max(let a, let b): {
        if (Number.isNumber(a) == false): {
            Throw.exception("Param is not a number");
        }

        if (Number.isNumber(b) == false): {
            Throw.exception("Param is not a number");
        }

        if a > b: {
            return a;
        }

        return b;
    }

    function min(let a, let b): {
        if (Number.isNumber(a) == false): {
            Throw.exception("Param is not a number");
        }

        if (Number.isNumber(b) == false): {
            Throw.exception("Param is not a number");
        }

        if a < b: {
            return a;
        }

        return b;
    }

    function abs(let n): {
        if (Number.isNumber(n) == false): {
            Throw.exception("Param is not a number");
        }

        return 0-n;
    }
}