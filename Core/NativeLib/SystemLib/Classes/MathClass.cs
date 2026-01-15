namespace Core.NativeLib.SystemLib.Classes;

public class MathClass : IQlangClass
{
    public string Name { get; init; } = "math";

    public List<(string name, Delegate body)> GetFunctions() => [
        ("pow", (Func<double, double, double>) Math.Pow),
        ("sin", (Func<double, double>) Math.Sin),
        ("cos", (Func<double, double>) Math.Cos),
        ("e", (Func<double>)(() => Math.E)),
        ("pi", (Func<double>)(() => Math.PI)),
        ("tau", (Func<double>)(() => Math.Tau)),
    ];
}