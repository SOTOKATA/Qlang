using Qlang.Core.ProjectManager.Project;

namespace Qlang.NativeLib.SystemLib;

public class DateTimeLib : IQlangLib
{
    public string Name { get; } = "DateTimeLib";
    public string Version { get; } = Project.Version;
    public string Author { get; } = "SOTOKATA";
    public string Class { get; } = "datetime";
    public string Namespace { get; } = "lib";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("now", (Func<DateTime>)(() => DateTime.Now)),
            ("wait", (Action<int>)(Thread.Sleep))
        ];
    }
}