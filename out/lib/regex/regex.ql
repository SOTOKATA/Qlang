import "$lib/core"

namespace regex: {
    function<String> replace(<String> input, <String> pattern, <String> replacement = "")
        => _native("std", "regex", "replace", input, pattern, replacement);
}