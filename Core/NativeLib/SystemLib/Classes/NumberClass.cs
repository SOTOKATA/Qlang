namespace Core.NativeLib.SystemLib.Classes;

public class NumberClass : IQlangClass
{
    public string Name { get; init;  } = "number";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("try_parse", (Func<object, bool>)(number => number.ToString().TryParseNumber(out _))),
            ("to_string", (Func<double, string, string>)((o, pattern) => o.ToString(pattern)))
        ];
    }
}