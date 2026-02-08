using Core.Dynamic;

namespace Core.NativeLib.SystemLib.Classes;

public class StringClass : IQlangClass
{
    public string Name { get; init; } = "string";
    
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("to_string", (Func<object?, string?>)(str => str?.ToString())),
            ("create", (Func<string, int, string>)((str, index) => new string(str[0], index))),
            ("at", (Func<string, int, string>)((str, index) => str[index].ToString())),
            ("set_at", (Func<string, string, int, string>)((str, toAdd, index) =>
            {
                var arr = str.ToCharArray();
                arr[index] = toAdd[0];

                return new string(arr);
            })),
            ("is_primitive", (Func<object?, bool>)(str => str is string)),
            ("is_str", (Func<object?, bool>)(str => str is DynamicClass { ClassName: "String" })),
            ("is_null_or_empty", (Func<string, bool>)(string.IsNullOrEmpty)),
            ("is_null_or_whitespace", (Func<string, bool>)(string.IsNullOrWhiteSpace)),
            ("trim", (Func<string, string>)(msg => msg.Trim())),
            ("trim_start", (Func<string, string>)(msg => msg.TrimStart())),
            ("trim_end", (Func<string, string>)(msg => msg.TrimEnd())),
            ("trim_b", (Func<string, string, string>)((msg, str) => msg.Trim(str.FirstOrDefault()))),
            ("trim_start_b", (Func<string, string, string>)((msg, str) => msg.TrimStart(str.FirstOrDefault()))),
            ("trim_end_b", (Func<string, string, string>)((msg, str) => msg.TrimEnd(str.FirstOrDefault()))),
            ("substring", (Func<string, int, int, string>)((msg, start, length) => msg.Substring(start, length))),
            ("to_lower", (Func<string, string>)((str) => str.ToLower())),
            ("to_upper", (Func<string, string>)((str) => str.ToUpper())),
            ("split", (Func<string, string, List<object>>)((str, splitPattern) => str.Split(splitPattern).Cast<object>().ToList())),
            ("join", (Func<List<object?>, string, string>)((arr, joinPattern) => string.Join(joinPattern, arr))),
            ("length", (Func<string, double>)(msg => msg.Length)),
            ("index_of", (Func<string, string, int>)((str, pattern) => str.IndexOf(pattern, StringComparison.CurrentCulture))),
            ("last_index_of", (Func<string, string, int>)((str, pattern) => str.LastIndexOf(pattern, StringComparison.CurrentCulture))),
            ("format", (Func<string, List<object?>, string?>)((str, numArray) => string.Format(str, numArray.ToArray()))),
        ];
    }
}