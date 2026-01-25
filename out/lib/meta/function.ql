import "$lib/meta"
import "$lib/standard"

namespace meta: {
    class FunctionMeta: {
        function isFunction(const func):
            return _native("std.meta.is_function", func);
    }
}