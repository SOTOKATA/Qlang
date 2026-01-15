include "$lib/core"

class Object: {
    function isNull(let obj):
        return _native("std.object.is_null", obj);

    function isSimplify(const val): 
        return _native("std.object.is_simplify", val);

    function toString(const obj = nameof(this)):
        return _native("std.string.to_string", obj);
}

function str(const obj):
    return Object.toString(obj);

function boolCase(const condition, const trueResult, const falseResult):
    if condition: return trueResult; else: return falseResult;