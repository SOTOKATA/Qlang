
include "$lib/base"

class Object: {
    function isNull(let obj): {
        return _native("lib.obj_is_null", obj);
    }

    function toString(): {
        return this;
    }
}