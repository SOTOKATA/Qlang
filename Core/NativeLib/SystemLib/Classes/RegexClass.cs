using System.Text.RegularExpressions;

namespace Core.NativeLib.SystemLib.Classes;

public class RegexClass : IQlangClass
{
    public string Name { get; init; } = "Regex";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("Replace", (Func<string?, string, string, string?>)Regex.Replace)
        ];
    }
}