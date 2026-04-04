using Core.Dynamic;

namespace Core.NativeLib.SystemLib.Classes;

public class StringClass : IQlangClass
{
    public string Name { get; init; } = "String";
    
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("ToString", (Func<object?, string?>)(str => str?.ToString())),
            ("Create", (Func<string, int, string>)((str, count) => new string(str.FirstOrDefault(), count))),
            ("At", (Func<string, int, string>)((str, index) => str[index].ToString())),
            ("SetAt", (Func<string, string, int, string>)((str, toAdd, index) =>
            {
                var arr = str.ToCharArray();
                arr[index] = toAdd.FirstOrDefault();
                return new string(arr);
            })),
            ("IsPrimitive", (Func<object?, bool>)(str => str is string)),
            ("IsStr", (Func<object?, bool>)(str => str is DynamicClass { ClassName: "String" })),
            ("IsNullOrEmpty", (Func<string?, bool>)(string.IsNullOrEmpty)),
            ("IsNullOrWhiteSpace", (Func<string?, bool>)(string.IsNullOrWhiteSpace)),
            ("Trim", (Func<string, string>)(msg => msg.Trim())),
            ("TrimStart", (Func<string, string>)(msg => msg.TrimStart())),
            ("TrimEnd", (Func<string, string>)(msg => msg.TrimEnd())),
            ("StartsWith", (Func<string, string, bool>)((s, s1) => s.StartsWith(s1))),
            ("EndsWith", (Func<string, string, bool>)((s, s1) => s.EndsWith(s1))),
            ("Trim_B", (Func<string, string, string>)((msg, str) => msg.Trim(str.FirstOrDefault()))),
            ("TrimStart_B", (Func<string, string, string>)((msg, str) => msg.TrimStart(str.FirstOrDefault()))),
            ("TrimEnd_B", (Func<string, string, string>)((msg, str) => msg.TrimEnd(str.FirstOrDefault()))),
            ("SubString", (Func<string, int, int, string>)((msg, start, length) => msg.Substring(start, length))),
            ("ToLower", (Func<string, string>)(str => str.ToLower())),
            ("ToUpper", (Func<string, string>)(str => str.ToUpper())),
            ("Split", (Func<string, string, List<object?>>)((str, splitPattern) => 
                str.Split(splitPattern, StringSplitOptions.None).Cast<object?>().ToList())),
            ("Join", (Func<List<object?>, string, string>)((arr, joinPattern) => 
                string.Join(joinPattern, arr))),
            ("Length", (Func<string, double>)(msg => (double)msg.Length)),
            ("IndexOf", (Func<string, string, int>)((str, pattern) => 
                str.IndexOf(pattern, StringComparison.CurrentCulture))),
            ("LastIndexOf", (Func<string, string, int>)((str, pattern) => 
                str.LastIndexOf(pattern, StringComparison.CurrentCulture))),
            ("Format", (Func<string, List<object?>, string?>)((str, args) => 
                string.Format(str, args.ToArray()))),
        ];
    }
}