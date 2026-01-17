import "$lib/core"

class Object: {
    const QL_SYSTEM_CLASS_NAME_OVERRIDE = "Example";
    function isNull(let obj):
        return _native("std.object.is_null", obj);

    function getType():
        return _native("std.object.get_type", this);

    function isSimplify(const val): 
        return _native("std.object.is_simplify", val);

    function toString(const obj = nameof(QL_SYSTEM_CLASS_NAME_OVERRIDE)):
        return _native("std.string.to_string", obj);
}

function str(const obj):
    return Object.toString(obj);

function boolCase(const condition, const trueResult, const falseResult):
    if condition: return trueResult; else: return falseResult;