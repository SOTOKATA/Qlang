namespace Core.NativeLib.SystemLib.Classes;

public class EnvironmentClass : IQlangClass
{
    public string Name { get; init; } = "env";

    public List<(string name, Delegate body)> GetFunctions() => [
        ("getCurrentDirectory", (Func<string>)(() => Environment.CurrentDirectory)),
        ("setCurrentDirectory", (Action<string>)((str) => Environment.CurrentDirectory = str)),
        ("newLine", (Func<string>)(() => Environment.NewLine)),
        ("machineName", (Func<string>)(() => Environment.MachineName)),
        ("processPath", (Func<string?>)(() => Environment.ProcessPath)),
        ("userName", (Func<string?>)(() => Environment.UserName)),
        ("exit", (Action<int>)Environment.Exit),
    ];
}