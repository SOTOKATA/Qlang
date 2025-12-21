using Qlang.Core.Lang.Dynamic;
using Project = Qlang.Core.ProjectManager.Project.Project;

namespace Qlang.NativeLib.SystemLib;

public class StringLib : IQlangLib
{
    public string Name { get; } = "StringLib";
    public string Version { get; } = Project.Version;
    public string Author { get; } = "SOTOKATA";
    public string Class { get; } = "string";
    public string Namespace { get; } = "lib";
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
            ("substring", (Func<string, int, int, string>)((msg, start, length) => msg.Substring(start, length))),
            ("to_lower", (Func<string, string>)((str) => str.ToLower())),
            ("to_upper", (Func<string, string>)((str) => str.ToUpper())),
            ("split", (Func<string, string, List<object>>)((str, splitPattern) => str.Split(splitPattern).Cast<object>().ToList())),
            ("join", (Func<List<object?>, string, string>)((arr, joinPattern) => string.Join(joinPattern, arr))),
            ("length", (Func<string, double>)(msg => msg.Length))
        ];
    }
}