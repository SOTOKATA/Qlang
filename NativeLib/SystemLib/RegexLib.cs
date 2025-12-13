using System.Text.RegularExpressions;
using Qlang.Core.ProjectManager.Project;

namespace Qlang.NativeLib.SystemLib;

public class RegexLib : IQlangLib
{
    public string Name { get; } = "RegexLib";
    public string Version { get; } = Project.Version;
    public string Author { get; } = "SOTOKATA";
    public string Class { get; } = "regex";
    public string Namespace { get; } = "lib";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("replace", (Func<string?, string, string, string?>)Regex.Replace)
        ];
    }
}