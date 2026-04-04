import "$lib/core"

const object = new Object();
class Object: {
    function<Boolean> isNull(let obj) => #std.Object.IsNull(obj);

    function<Boolean> isSimplify(val) => #std.Object.IsSimplify(val);

    function<String> toString(obj)
        => if obj is null ? "<null>" : #std.String.ToString(obj);

    function<String> toString() => toString(this);
}

function<String> str(obj) => if object.toString(obj) ?? "<null>";