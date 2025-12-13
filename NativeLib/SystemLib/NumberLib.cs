using Qlang.Core.Lang;
using Qlang.Core.ProjectManager.Project;

namespace Qlang.NativeLib.SystemLib;

public class NumberLib : IQlangLib
{
    public string Name { get; } = "NumberLib";
    public string Version { get; } = Project.Version;
    public string Author { get; } = "SOTOKATA";
    public string Class { get; } = "number";
    public string Namespace { get; } = "lib";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("try_parse", (Func<object, bool>)(number => number.ToString().TryParseNumber(out _))),
            ("random", (Func<int, int, int>)((num1, num2) => new Random().Next(num1, num2))),
            ("to_string", (Func<double, string, string>)((o, pattern) => o.ToString(pattern)))
        ];
    }
}