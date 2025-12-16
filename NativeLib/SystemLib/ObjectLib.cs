using Qlang.Core.ProjectManager.Project;

namespace Qlang.NativeLib.SystemLib;

public class ObjectLib : IQlangLib
{
    public string Name { get; } = "ObjectLib";
    public string Version { get; } = Project.Version;
    public string Author { get; } = "SOTOKATA";
    public string Class { get; } = "object";
    public string Namespace { get; } = "lib";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("is_null",  (Func<object?, bool>)(obj => obj is null)),
            ("is_simplify",  (Func<object?, bool>)(obj => obj is double or int or float or string or bool or List<object>))
        ];
    }
}