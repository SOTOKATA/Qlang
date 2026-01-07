namespace Core.NativeLib.SystemLib.Classes;

public class DateTimeClass : IQlangClass
{
    public string Name { get; init; } = "datetime";
    
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("now", (Func<DateTime>)(() => DateTime.Now)),
            ("wait", (Action<int>)(Thread.Sleep))
        ];
    }
}