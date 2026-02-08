import "$lib/standard"
import "$lib/core"

namespace regex: {
    function replace(const<String> input, const<String> pattern, const<String> replacement = ""): {
        return _native("std", "regex", "replace", input, pattern, replacement);
    }
}