
class Object: {
    function isNull(let obj): {
        return _native("obj_is_null", obj);
    }

    function toString(): {
        return this;
    }
}