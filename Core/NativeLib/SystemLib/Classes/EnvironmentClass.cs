namespace Core.NativeLib.SystemLib.Classes;

public class EnvironmentClass : IQlangClass
{
    public string Name { get; init; } = "Env";

    public List<(string name, Delegate body)> GetFunctions() => [
        ("GetCurrentDirectory", (Func<string>)(() => Environment.CurrentDirectory)),
        ("SetCurrentDirectory", (Action<string>)((str) => Environment.CurrentDirectory = str)),
        ("NewLine", (Func<string>)(() => Environment.NewLine)),
        ("MachineName", (Func<string>)(() => Environment.MachineName)),
        ("ProcessPath", (Func<string?>)(() => Environment.ProcessPath)),
        ("UserName", (Func<string?>)(() => Environment.UserName)),
        ("Exit", (Action<int>)Environment.Exit),
    ];
}