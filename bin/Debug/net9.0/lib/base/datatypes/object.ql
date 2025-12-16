
include "$lib/base"

class Object: {
    function isNull(let obj): {
        return _native("lib.object.is_null", obj);
    }

    function isSimplify(const val): return _native("lib.object.is_simplify", val);

    function toString(): {
        return nameof(this);
    }
}