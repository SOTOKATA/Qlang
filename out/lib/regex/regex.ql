import "$lib/standard";
import "$lib/core";

namespace regex: {
    class Regex: {
        function replace(const<String> input, const<String> pattern, const<String> replacement = ""): {
            return _native("std.regex.replace", input, pattern, replacement);
        }

        // TODO: create on compiler part
        // function match(let input, let pattern): {
        //     return _native("std.regex.match", input, pattern);
        // }
    }
}