import "$lib/core"

class Object: {
    function<Boolean> isNull(let obj):
        return _native("std", "object", "is_null", obj);

    function<String> getType():
        return _native("std", "object", "get_type", this);

    function<Boolean> isSimplify(const val): 
        return _native("std", "object", "is_simplify", val);

    function<String> toString(const obj): {
        if isNull(obj):
            return "<null>";
            
        return _native("std", "string", "to_string", obj);
    }

    function<String> toString(): {
        if isNull(this):
            return "<null>";
            
        return _native("std", "string", "to_string", this);
    } 
}

function<String> str(const obj):
    return Object.toString(obj);

function boolCase(const condition, const trueResult, const falseResult):
    if condition: return trueResult; else: return falseResult;

function<String> valueOrStrNull(const value):
    if Object.isNull(value): return "null"; else: return value.toString();