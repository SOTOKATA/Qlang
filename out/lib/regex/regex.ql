import "$lib/core"

namespace regex: {
    function<String> replace(const<String> input, const<String> pattern, const<String> replacement = ""): {
        return _native("std", "regex", "replace", input, pattern, replacement);
    }
}