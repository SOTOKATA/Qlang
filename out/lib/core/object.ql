include "$lib/core"

class Object: {
    function isNull(let obj):
        return _native("lib.object.is_null", obj);

    function isSimplify(const val): 
        return _native("lib.object.is_simplify", val);

    function toString(const obj = nameof(this)):
        return _native("lib.string.to_string", obj);
}

function str(const obj):
    return Object.toString(obj);

function boolCase(const condition, const trueResult, const falseResult):
    if condition: return trueResult; else: return falseResult;