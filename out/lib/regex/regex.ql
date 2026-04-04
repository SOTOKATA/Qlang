import "$lib/core"

namespace regex: {
    function<String> replace(<String> input, <String> pattern, <String> replacement = "") 
        => #std.Regex.Replace(input, pattern, replacement);
}