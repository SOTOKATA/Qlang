import "$lib/core"

const object = new Object();
class Object: {
    function<Boolean> isNull(let obj) => _native("std", "object", "is_null", obj);

    function<String> getType() => _native("std", "object", "get_type", this);

    function<Boolean> isSimplify(const val) => _native("std", "object", "is_simplify", val);

    function<String> toString(const obj)
        => if typeof(obj) == "null" ? "<null>" : _native("std", "string", "toString", obj);

    function<String> toString() => toString(this);
}

function<String> str(const obj) => object.toString(obj);

function<String> valueOrStrNull(const value) => if object.isNull(value) ? "null" : value.toString();