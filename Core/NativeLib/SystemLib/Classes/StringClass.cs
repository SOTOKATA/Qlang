using Core.Dynamic;

namespace Core.NativeLib.SystemLib.Classes;

public class StringClass : IQlangClass
{
    public string Name { get; init; } = "string";
    
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("toString", (Func<object?, string?>)(str => str?.ToString())),
            ("create", (Func<string, int, string>)((str, index) => new string(str[0], index))),
            ("at", (Func<string, int, string>)((str, index) => str[index].ToString())),
            ("setAt", (Func<string, string, int, string>)((str, toAdd, index) =>
            {
                var arr = str.ToCharArray();
                arr[index] = toAdd[0];

                return new string(arr);
            })),
            ("isPrimitive", (Func<object?, bool>)(str => str is string)),
            ("isStr", (Func<object?, bool>)(str => str is DynamicClass { ClassName: "String" })),
            ("isNullOrEmpty", (Func<string, bool>)(string.IsNullOrEmpty)),
            ("isNullOrWhitespace", (Func<string, bool>)(string.IsNullOrWhiteSpace)),
            ("trim", (Func<string, string>)(msg => msg.Trim())),
            ("trimStart", (Func<string, string>)(msg => msg.TrimStart())),
            ("startsWith", (Func<string, string, bool>)((s, s1) => s.StartsWith(s1))),
            ("endsWith", (Func<string, string, bool>)((s, s1) => s.EndsWith(s1))),
            ("trimEnd", (Func<string, string>)(msg => msg.TrimEnd())),
            ("trim_b", (Func<string, string, string>)((msg, str) => msg.Trim(str.FirstOrDefault()))),
            ("trimStart_b", (Func<string, string, string>)((msg, str) => msg.TrimStart(str.FirstOrDefault()))),
            ("trimEnd_b", (Func<string, string, string>)((msg, str) => msg.TrimEnd(str.FirstOrDefault()))),
            ("subString", (Func<string, int, int, string>)((msg, start, length) => msg.Substring(start, length))),
            ("toLower", (Func<string, string>)((str) => str.ToLower())),
            ("toUpper", (Func<string, string>)((str) => str.ToUpper())),
            ("split", (Func<string, string, List<object?>>)((str, splitPattern) => str.Split(splitPattern).Cast<object>().ToList())),
            ("join", (Func<List<object?>, string, string>)((arr, joinPattern) => string.Join(joinPattern, arr))),
            ("length", (Func<string, double>)(msg => msg.Length)),
            ("indexOf", (Func<string, string, int>)((str, pattern) => str.IndexOf(pattern, StringComparison.CurrentCulture))),
            ("lastIndexOf", (Func<string, string, int>)((str, pattern) => str.LastIndexOf(pattern, StringComparison.CurrentCulture))),
            ("format", (Func<string, List<object?>, string?>)((str, numArray) => string.Format(str, numArray.ToArray()))),
        ];
    }
}