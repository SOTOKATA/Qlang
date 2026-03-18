import "$lib/core"

const object = new Object();
class Object: {
    function<Boolean> isNull(let obj) => _native("std", "object", "is_null", obj);

    function<String> getType() => _native("std", "object", "get_type", this);

    function<Boolean> isSimplify(const val): 
        return _native("std", "object", "is_simplify", val);

    function<String> toString(const obj): {
        if isNull(obj):
            return "<null>";
            
        return _native("std", "string", "toString", obj);
    }

    function<String> toString(): {
        if isNull(this):
            return "<null>";
            
        return _native("std", "string", "toString", this);
    } 
}

function<String> str(const obj):
    return object.toString(obj);

function<String> valueOrStrNull(const value):
    return if object.isNull(value) ? "null" : value.toString();