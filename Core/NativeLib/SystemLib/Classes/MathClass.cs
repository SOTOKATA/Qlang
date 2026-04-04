namespace Core.NativeLib.SystemLib.Classes;

public class MathClass : IQlangClass
{
    public string Name { get; init; } = "Math";

    public List<(string name, Delegate body)> GetFunctions() => [
        ("MinValue", (Func<double>)(()=>double.MinValue)),
        ("MaxValue", (Func<double>)(()=>double.MaxValue)),
        ("Pow", (Func<double, double, double>) Math.Pow),
        ("Sin", (Func<double, double>) Math.Sin),
        ("Cos", (Func<double, double>) Math.Cos),
        ("E", (Func<double>)(() => Math.E)),
        ("PI", (Func<double>)(() => Math.PI)),
        ("Tau", (Func<double>)(() => Math.Tau)),
        ("Random", (Func<int, int, int>)((num1, num2) => new Random().Next(num1, num2))),
        ("Round", (Func<double, int, double>)Math.Round),
    ];
}