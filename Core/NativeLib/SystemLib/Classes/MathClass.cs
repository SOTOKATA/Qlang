namespace Core.NativeLib.SystemLib.Classes;

public class MathClass : IQlangClass
{
    public string Name { get; init; } = "math";

    public List<(string name, Delegate body)> GetFunctions() => [
        ("min_value", (Func<double>)(()=>double.MinValue)),
        ("max_value", (Func<double>)(()=>double.MaxValue)),
        ("pow", (Func<double, double, double>) Math.Pow),
        ("sin", (Func<double, double>) Math.Sin),
        ("cos", (Func<double, double>) Math.Cos),
        ("e", (Func<double>)(() => Math.E)),
        ("pi", (Func<double>)(() => Math.PI)),
        ("tau", (Func<double>)(() => Math.Tau)),
        ("random", (Func<int, int, int>)((num1, num2) => new Random().Next(num1, num2))),
        ("round", (Func<double, int, double>)(Math.Round)),
    ];
}