import "$lib/core"

const object = new Object();
class Object: {
    function<Boolean> isNull(let obj) => _native("std", "object", "is_null", obj);

    function<String> getType() => _native("std", "object", "get_type", this);

    function<Boolean> isSimplify(val) => _native("std", "object", "is_simplify", val);

    function<String> toString(obj)
        => if obj is null ? "<null>" : _native("std", "string", "toString", obj);

    function<String> toString() => toString(this);
}

function<String> str(obj) => if object.toString(obj) ?? "<null>";

function<String> valueOrStrNull(value) => if object.isNull(value) ? "null" : value.toString();