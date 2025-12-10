include "$lib/base"

class Regex: {
    function replace(let input, let pattern, let replacement = ""): {
        return _native("lib.regex_replace", input, pattern, replacement);
    }

    function match(let input, let pattern): {
        return _native("lib.regex_match", input, pattern);
    }
}