namespace Core.NativeLib.SystemLib.Classes;

public class NumberClass : IQlangClass
{
    public string Name { get; init;  } = "number";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("try_parse", (Func<object, bool>)(number => number.ToString().TryParseNumber(out _))),
            ("random", (Func<int, int, int>)((num1, num2) => new Random().Next(num1, num2))),
            ("to_string", (Func<double, string, string>)((o, pattern) => o.ToString(pattern)))
        ];
    }
}