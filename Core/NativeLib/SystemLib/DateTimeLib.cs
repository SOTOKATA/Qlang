namespace Core.NativeLib.SystemLib;

public class DateTimeLib : IQlangLib
{
    public string Name { get; } = "DateTimeLib";
    public string Version { get; } = "0.0.0";
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