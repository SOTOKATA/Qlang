namespace Core.NativeLib.SystemLib.Classes;

public class EnvironmentClass : IQlangClass
{
    public string Name { get; init; } = "env";

    public List<(string name, Delegate body)> GetFunctions() => [
        ("current_directory", (Func<string>)(() => Environment.CurrentDirectory)),
        ("new_line", (Func<string>)(() => Environment.NewLine)),
        ("machine_name", (Func<string>)(() => Environment.MachineName)),
        ("process_path", (Func<string?>)(() => Environment.ProcessPath)),
        ("user_name", (Func<string?>)(() => Environment.UserName)),
        ("exit", (Action<int>)Environment.Exit),
        
    ];
}