using System.Text.RegularExpressions;

namespace Core.NativeLib.SystemLib.Classes;

public class RegexClass : IQlangClass
{
    public string Name { get; init; } = "regex";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("replace", (Func<string?, string, string, string?>)Regex.Replace)
        ];
    }
}