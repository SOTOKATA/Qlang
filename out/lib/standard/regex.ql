include "$lib/standard"
include "$lib/core"

namespace std: {
    class Regex: {
        function replace(const<String> input, const<String> pattern, const<String> replacement = ""): {
            return _native("lib.regex.replace", input, pattern, replacement);
        }

        // TODO: create on compiler part
        // function match(let input, let pattern): {
        //     return _native("lib.regex.match", input, pattern);
        // }
    }
}