using Qlang.Core.Lang.Dynamic.Exceptions;
using Qlang.Core.ProjectManager.Project;

namespace Qlang.NativeLib.SystemLib;

public class ExceptionLib : IQlangLib
{
    public string Name { get; } = "ExceptionLib";
    public string Version { get; } = Project.Version;
    public string Author { get; } = "SOTOKATA";
    public string Class { get; } = "exception";
    public string Namespace { get; } = "lib";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("throw", (Action<string>)(msg => throw new QlangRuntimeException(msg, null)))
        ];
    }
}