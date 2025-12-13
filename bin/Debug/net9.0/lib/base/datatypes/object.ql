
include "$lib/base"

class Object: {
    function isNull(let obj): {
        return _native("lib.object.is_null", obj);
    }

    function toString(): {
        return this;
    }
}